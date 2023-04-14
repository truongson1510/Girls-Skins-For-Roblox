using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UniRx;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

//  ---------------------------------------------
//  Author:     DatDz <steven@atsoft.io> 
//              AVT <an.viettrung.m@gmail.com>             
//  Copyright (c) 2022 AT Soft
// ----------------------------------------------
namespace ATSoft.Ads
{
    public class Advertisements : Singleton<Advertisements>, IService
    {
        private const string removeAds = "RemoveAds";

        public static bool initialized;
        private static GameObject go;

        [SerializeField] private bool enableAds;
        [SerializeField] private bool isTestAds;
        [SerializeField] private bool enableDebugger;
        //[SerializeField] private bool enableAppTrackingTransparency;

        [OnValueChanged("ChangeTypeAdvertiserHandle")] [SerializeField]
        private SupportedAdvertisers typeAdvertiser = SupportedAdvertisers.None;

        [OnValueChanged("ChangePublisherHandle")] [SerializeField]
        private ATPublisher publisher = ATPublisher.InHouse;

        [Header("--- Settings Ads ---")] [TableList] public List<Advertiser> advertisers;

        [HideInInspector] public bool ResumeFromAds = false;
        private Advertiser advertiser;
        private IDisposable obInter;
        private ICustomAds selectedAdvertiser;
        private bool canRepeatLoadAOA = false;
        private Subject<bool> InternetSubject = new Subject<bool>();
        private IObservable<bool> internetObservable;
        
        public bool IsTestAds => isTestAds;

        /// <summary>
        /// automatically disables banner and interstitial ads
        /// </summary>
        /// <param name="remove">if true, no banner and interstitials will be shown in your app</param>
        public void RemoveAds(bool remove)
        {
            if (remove == true)
            {
                PlayerPrefs.SetInt(removeAds, 1);
                //if banner is active and user bought remove ads the banner will automatically hide
                HideBanner();
            }
            else
            {
                PlayerPrefs.SetInt(removeAds, 0);
            }
        }


        /// <summary>
        /// check if ads are not disabled by user
        /// </summary>
        /// <returns>true if ads should be displayed</returns>
        public bool CanShowAds()
        {
            if (!PlayerPrefs.HasKey(removeAds))
            {
                return true;
            }
            else
            {
                if (PlayerPrefs.GetInt(removeAds) == 0)
                {
                    return true;
                }
            }

            return false;
        }
        
// #if AOA_TYPE_ADMOB
//         private void Update()
//         {
//             if (!enableAds || isTestAds) return;
//
//             if (canRepeatLoad)
//             {
//                 if (countLoadAOA >= 3) return;
//                 if (timer >= timeOut)
//                 {
//                     Debug.Log("=== LoadAOAIfNotAvailable ===");
//                     LoadAOAIfNotAvailable();
//                     countLoadAOA++;
//                     timer = 0;
//                 }
//                 else
//                 {
//                     timer += Time.deltaTime;
//                 }
//             }
//         }
// #endif

        private void Start()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes all the advertisers from the plugin
        /// Should be called only once at the beginning of your app
        /// </summary>
        public void Initialize()
        {
            Debug.Log("Initialize " + (typeof(Advertisements)));

            if (!enableAds) return;

            if (initialized == false)
            {
                initialized = true;

                StartCoroutine(WaitForConsent(ContinueInitialization));
            }
        }

        private IEnumerator WaitForConsent(UnityAction Continue)
        {
            Continue?.Invoke();
            yield return null;
        }

        private void ContinueInitialization()
        {
            switch (typeAdvertiser)
            {
                case SupportedAdvertisers.Admob:
#if AD_TYPE_ADMOB
                    var admobComp = gameObject.AddComponent<CustomAdmob>();
                    var tempAdvertiserAdmob =
                        advertisers.FirstOrDefault(condition => condition.advertiser == SupportedAdvertisers.Admob);
                    if (tempAdvertiserAdmob != null)
                        advertiser = new Advertiser(admobComp, SupportedAdvertisers.Admob,
                            tempAdvertiserAdmob.advertiserSettings);
#endif
                    break;
                case SupportedAdvertisers.AppLovin:
#if AD_TYPE_APPLOVIN
                    var appLovinComp = gameObject.AddComponent<CustomAppLovin>();
                    var tempAdvertiserAppLovin =
                        advertisers.FirstOrDefault(condition => condition.advertiser == SupportedAdvertisers.AppLovin);
                    if (tempAdvertiserAppLovin != null)
                        advertiser =
                            new Advertiser(appLovinComp, SupportedAdvertisers.AppLovin,
                                tempAdvertiserAppLovin.advertiserSettings);
#endif
                    break;
                case SupportedAdvertisers.IronSource:
#if AD_TYPE_IS
                    var ironSourceComp = gameObject.AddComponent<CustomIronSource>();
                    var tempAdvertiserIronSource =
                        advertisers.FirstOrDefault(condition => condition.advertiser == SupportedAdvertisers.IronSource);
                    if (tempAdvertiserIronSource != null)
                        advertiser =
                            new Advertiser(ironSourceComp, SupportedAdvertisers.IronSource,
                                tempAdvertiserIronSource.advertiserSettings);
#endif
                    break;
                default:
                    Debug.LogWarning("AT Soft - Failed to Set up Mobile Ads - UnKnow type Advertiser");
                    break;
            }

            advertiser?.advertiserScript.InitializeAds(advertiser.advertiserSettings, isTestAds, enableDebugger);
            selectedAdvertiser  = advertiser?.advertiserScript;
            canRepeatLoadAOA = true;
            
            internetObservable = Observable.Interval(TimeSpan.FromSeconds(1), Scheduler.MainThreadIgnoreTimeScale)
                .Select(_ => Application.internetReachability != NetworkReachability.NotReachable);

            internetObservable.DistinctUntilChanged().Subscribe(_ => { InternetSubject.OnNext(_); });
            
            internetObservable.Where(x => x == InitializedFirstStepAds())
                .Timeout(TimeSpan.FromSeconds(60))
                .Take(1)
                .Subscribe(_ =>
                {
                    Debug.Log(this + " InitInterstitial + InitRewardVideo");
                    InitInterstitial();
                    InitRewardVideo();
                }).AddTo(this);
        }

        private bool InitializedFirstStepAds()
        {
            return selectedAdvertiser != null && selectedAdvertiser.IsBannerAvailable() &&
                   selectedAdvertiser.IsAppOpenAdAvailable();
        }

        #region === INTERSTITIAL ===
        
        public void InitInterstitial()
        {
            selectedAdvertiser?.InitInterstitial();
        }

        /// <summary>
        /// Displays an interstitial video based on your mediation settings
        /// </summary>
        /// <param name="InterstitialClosed">callback triggered when interstitial video is closed</param>
        public void ShowInterstitial(UnityAction InterstitialClosed = null, string Placement = "",
            bool autoDisableLoading = true)
        {
            if (!enableAds)
            {
                InterstitialClosed?.Invoke();
                return;
            }

            //if ads are disabled by user -> do nothing
            if (CanShowAds() == false)
            {
                InterstitialClosed?.Invoke();
                return;
            }

            //ICustomAds selectedAdvertiser = GetInterstitialAdvertiser();
            if (selectedAdvertiser != null)
            {
                Debug.Log("Interstitial loaded from " + selectedAdvertiser);

                var showSuccess =
                    selectedAdvertiser.ShowInterstitial(InterstitialClosed, Placement, autoDisableLoading);
                //LogEvent
#if PUB_RK //Rocket
                AnalyticsManager.LogEventShowInterstitial(showSuccess ? 1 : 0, Placement);
                if (showSuccess) AnalyticsManager.LogEventImpdauInterPassed();
#elif PUB_AD1 //Ad1
                if (showSuccess)
                {
                    AnalyticsManager.LogEventShowInterstitial();
                    AnalyticsManager.LogEventImpdauInterPassed();
                }

#elif PUB_FAL // Falcon
                AnalyticsManager.LogEventShowInterstitial(showSuccess ? 1 : 0, Placement);
                //if(showSuccess) AnalyticsManager.LogEventImpdauInterPassed();
#else //In-house
                AnalyticsManager.LogEventShowInterstitial(showSuccess ? 1 : 0, Placement);
                if (showSuccess) AnalyticsManager.LogEventImpdauInterPassed();
#endif
            }
        }

        private ICustomAds GetInterstitialAdvertiser()
        {
            return advertiser?.advertiserScript;
        }

        /// <summary>
        /// Check if any interstitial is available
        /// </summary>
        /// <returns>true if at least one interstitial is available</returns>
        public bool IsInterstitialAvailable()
        {
            //if ads are disabled by user -> interstitial is not available
            if (CanShowAds() == false)
            {
                return false;
            }

            return advertiser != null && selectedAdvertiser.IsInterstitialAvailable();
        }

        #endregion

        #region === REWARDED VIDEO ===

        public void InitRewardVideo()
        {
            selectedAdvertiser?.InitRewardVideo();
        }

        /// <summary>
        /// Displays a rewarded video based on your mediation settings
        /// </summary>
        /// <param name="CompleteMethod">callback triggered when video reward finished - if bool param is true => video was not skipped</param>
        public bool ShowRewardedVideo(UnityAction<bool> CompleteMethod, string Placement)
        {
            if (!enableAds)
            {
                CompleteMethod?.Invoke(false);
                return false;
            }

            // ICustomAds selectedAdvertiser = GetRewardedVideoAdvertiser();

            if (selectedAdvertiser != null)
            {
                Debug.Log("Rewarded video loaded from " + selectedAdvertiser);

                var showSuccess = selectedAdvertiser.ShowRewardVideo(CompleteMethod, Placement);
                //LogEvent
#if PUB_RK //Rocket
                AnalyticsManager.LogEventShowReward(showSuccess ? 1 : 0, Placement);
                if(showSuccess) AnalyticsManager.LogEventImpdauRewardPassed();
#elif PUB_AD1 //Ad1
                if (showSuccess)
                {
                    AnalyticsManager.LogEventShowReward();
                    AnalyticsManager.LogEventImpdauRewardPassed();
                }
#elif PUB_FAL //Fal
                AnalyticsManager.LogEventShowReward(showSuccess ? 1 : 0, Placement);
                //if (showSuccess) AnalyticsManager.LogEventImpdauRewardPassed();
#else //In-house
                AnalyticsManager.LogEventShowReward(showSuccess ? 1 : 0, Placement);
                if (showSuccess) AnalyticsManager.LogEventImpdauRewardPassed();
#endif
                return showSuccess;
            }

            AdsNotAvailable.Instance.Show();
            return false;
        }

        private ICustomAds GetRewardedVideoAdvertiser()
        {
            return advertiser?.advertiserScript;
        }

        /// <summary>
        /// Check if any rewarded video is available to be played
        /// </summary>
        /// <returns>true if at least one rewarded video is available</returns>
        public bool IsRewardVideoAvailable()
        {
            return advertiser != null && selectedAdvertiser.IsRewardVideoAvailable();
        }

        #endregion

        #region === BANNER ===

        private bool isBannerOnScreen;
        private bool hideBanners;

        /// <summary>
        /// Displays a banner based on your mediation settings
        /// </summary>
        /// <param name="position">can be Top or Bottom</param>
        public void ShowBanner(BannerPosition position, BannerType bannerType = BannerType.SmartBanner)
        {
            //if ads are disabled by user -> do nothing
            if (!enableAds || CanShowAds() == false)
            {
                return;
            }

            hideBanners = false;
            LoadBanner(position, bannerType);
        }

        /// <summary>
        /// Loads banner for display
        /// </summary>
        /// <param name="position"></param>
        /// <param name="bannerType"></param>
        private void LoadBanner(BannerPosition position, BannerType bannerType = BannerType.SmartBanner)
        {
            //ICustomAds selectedAdvertiser = GetBannerAdvertiser();
            if (selectedAdvertiser != null)
            {
                Debug.Log("Banner loaded from " + selectedAdvertiser);

                selectedAdvertiser.ShowBanner(position, bannerType, BannerDisplayedResult);
            }
            else
            {
                isBannerOnScreen = false;

                Debug.LogError("No Banners Available");
            }
        }

        private void BannerDisplayedResult(bool succesfullyDisplayed, BannerPosition position, BannerType bannerType = BannerType.SmartBanner)
        {
            if (succesfullyDisplayed == false)
            {
                Debug.Log("Banner failed to load -> trying another advertiser");


                if (hideBanners == false)
                {
                    LoadBanner(position, bannerType);
                }
                else
                {
                    Debug.Log("Stop Loading Banners");
                }
            }
            else
            {
                isBannerOnScreen = true;

                Debug.Log("Banner is on screen");
            }
        }

        private ICustomAds GetBannerAdvertiser()
        {
            return advertiser?.advertiserScript;
        }


        /// <summary>
        /// Hides the active banner
        /// </summary>
        public void HideBanner()
        {
            Debug.Log("Hide Banners");

            hideBanners = true;

            selectedAdvertiser.HideBanner();

            isBannerOnScreen = false;
        }

        /// <summary>
        /// Check if a banner is visible
        /// </summary>
        /// <returns>true if banner is visible</returns>
        public bool IsBannerOnScreen()
        {
            return isBannerOnScreen;
        }

        #endregion
        
        #region === AOA ===

        public bool IsAppOpenAdAvailable()
        {
            return advertiser != null && selectedAdvertiser.IsAppOpenAdAvailable();
        }
        
        public void LoadAOAIfNotAvailable()
        {
            Debug.Log("Call LoadAOAIfNotAvailable");
            if (!enableAds || !initialized) return;
            
            selectedAdvertiser?.LoadAppOpenAd(() => ShowAppOpenAd());
        }
        
        public bool ShowAppOpenAd(UnityAction appOpenAdClosed = null)
        {
            if (!enableAds)
            {
                appOpenAdClosed?.Invoke();
                return true;
            }
            
            if(!initialized)
            {
                return false;
            }
            
            if (selectedAdvertiser != null)
            {
                Debug.Log("AOA loaded from " + selectedAdvertiser);
                
                var showSuccess = selectedAdvertiser.ShowAppOpenAd(appOpenAdClosed);
                if (showSuccess) canRepeatLoadAOA = false;
                return showSuccess;
            }
            
            return false;
        }
        
        private bool didShowAoALoadingTime;
        private float lastCheck = 0;

        public void ShowAOAIfLoadedInLoadingTime()
        {
            if (didShowAoALoadingTime || lastCheck + 0.5f > Time.time)
                return;

            lastCheck = Time.time;
            didShowAoALoadingTime = ShowAppOpenAd();
        }
        
        #endregion
        
        private void OnApplicationPause(bool pause)
        {
            if (!pause && !ResumeFromAds && enableAds)
            {
                ShowAppOpenAd();
            }
        }


#if UNITY_EDITOR
        
        private void ChangeTypeAdvertiserHandle()
        {
            var buildTargetGroup = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);


            switch (typeAdvertiser)
            {
                case SupportedAdvertisers.None:
                    AddPreprocessorDirectiveAdType("", buildTargetGroup);
                    break;
                case SupportedAdvertisers.Admob:
                    AddPreprocessorDirectiveAdType("AD_TYPE_ADMOB", buildTargetGroup);
                    break;
                case SupportedAdvertisers.AppLovin:
                    AddPreprocessorDirectiveAdType("AD_TYPE_APPLOVIN", buildTargetGroup);
                    break;
                case SupportedAdvertisers.IronSource:
                    AddPreprocessorDirectiveAdType("AD_TYPE_IS", buildTargetGroup);
                    break;
                default:
                    AddPreprocessorDirectiveAdType("", buildTargetGroup);
                    break;
            }
        }

        private void ChangePublisherHandle()
        {
            var buildTargetGroup = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
// #if UNITY_IOS
//             AddPreprocessorDirective("USE_ATT", !enableAppTrackingTransparency, buildTargetGroup);
// #endif
            switch (publisher)
            {
                case ATPublisher.RK:
                    AddPreprocessorDirective("PUB_RK", buildTargetGroup);
                    break;
                case ATPublisher.Ad1:
                    AddPreprocessorDirective("PUB_AD1", buildTargetGroup);
                    break;
                case ATPublisher.Fal:
                    AddPreprocessorDirective("PUB_FAL", buildTargetGroup);
                    break;
                default:
                    AddPreprocessorDirective("", buildTargetGroup);
                    break;
            }
        }
        
        private void AddPreprocessorDirective(string directive, bool remove, BuildTargetGroup target)
        {
            string textToWrite = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);

            if (remove)
            {
                if (textToWrite.Contains(directive))
                {
                    textToWrite = textToWrite.Replace(directive, "");
                }
            }
            else
            {
                if (!textToWrite.Contains(directive))
                {
                    if (textToWrite == "")
                    {
                        textToWrite += directive;
                    }
                    else
                    {
                        textToWrite += ";" + directive;
                    }
                }
            }

            PlayerSettings.SetScriptingDefineSymbolsForGroup(target, textToWrite);
            EditorUtility.DisplayDialog("Scripting Define Symbols", $"Add Scripting Define Symbols: {directive} success <3", "OK");

        }

        private void AddPreprocessorDirective(string directive, BuildTargetGroup target)
        {
            string textToWrite = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);

            switch (directive)
            {
                case "PUB_RK":
                    textToWrite = textToWrite.Replace("PUB_AD1", "").Replace("PUB_FAL", "");
                    break;
                case "PUB_AD1":
                    textToWrite = textToWrite.Replace("PUB_RK", "").Replace("PUB_FAL", "");
                    break;
                case "PUB_FAL":
                    textToWrite = textToWrite.Replace("PUB_RK", "").Replace("PUB_AD1", "");
                    break;
                default:
                    textToWrite = textToWrite.Replace("PUB_RK", "").Replace("PUB_AD1", "").Replace("PUB_FAL", "");
                    break;
            }

            if (!textToWrite.Contains(directive))
            {
                if (textToWrite == "")
                {
                    textToWrite += directive;
                }
                else
                {
                    textToWrite += ";" + directive;
                }
            }

            PlayerSettings.SetScriptingDefineSymbolsForGroup(target, textToWrite);
            
            EditorUtility.DisplayDialog("Scripting Define Symbols", $"Add Scripting Define Symbols: {directive} success <3", "OK");
        }

        private void AddPreprocessorDirectiveAdType(string directive, BuildTargetGroup target)
        {
            string textToWrite = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);

            switch (directive)
            {
                case "AD_TYPE_ADMOB":
                    textToWrite = textToWrite.Replace("AD_TYPE_APPLOVIN", "").Replace("AD_TYPE_IS", "");//.Replace("AOA_TYPE_APPLOVIN", "");
                    break;
                case "AD_TYPE_APPLOVIN":
                    textToWrite = textToWrite.Replace("AD_TYPE_ADMOB", "").Replace("AD_TYPE_IS", "");//.Replace("AOA_TYPE_ADMOB", "");
                    break;
                case "AD_TYPE_IS":
                    textToWrite = textToWrite.Replace("AD_TYPE_ADMOB", "").Replace("AD_TYPE_APPLOVIN", "");
                    break;
                default:
                    textToWrite = textToWrite.Replace("AD_TYPE_ADMOB", "").Replace("AD_TYPE_APPLOVIN", "")
                        .Replace("AD_TYPE_IS", "");//.Replace("AOA_TYPE_ADMOB", "").Replace("AOA_TYPE_APPLOVIN", "");
                    break;
            }

            if (!textToWrite.Contains(directive))
            {
                if (textToWrite == "")
                {
                    textToWrite += directive;
                }
                else
                {
                    textToWrite += ";" + directive;
                }
            }

            PlayerSettings.SetScriptingDefineSymbolsForGroup(target, textToWrite);

            EditorUtility.DisplayDialog("Scripting Define Symbols", $"Add Scripting Define Symbols: {directive} success <3", "OK");
        }
#endif
    }

    public enum ATPublisher
    {
        InHouse,
        RK,
        Ad1,
        Fal
    }
}
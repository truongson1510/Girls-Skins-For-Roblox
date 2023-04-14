using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UniRx;

//  ---------------------------------------------
//  Author:     DatDz <steven@atsoft.io> 
//  Copyright (c) 2022 AT Soft
// ----------------------------------------------

#if AD_TYPE_IS
namespace ATSoft.Ads
{
    public class CustomIronSource : BaseAds, ICustomAds
    {
        private bool bannerLoaded;
        
        private const float reloadTime = 2;
        private bool rewardedVideoCompleted;
        private bool directedForChildren;
        private bool enableDebugger;
        
#if UNITY_ANDROID
        private string ID_TEST_APP = "1764abf15";
#elif UNITY_IOS
        private string ID_TEST_APP = "17455d1ed";
#else
        private string ID_TEST_APP = "unexpected_platform";
#endif

        /// <summary>
        /// Initializing IronSource
        /// </summary>
        /// <param name="platformSettings">contains all required settings for this publisher</param>
        public void InitializeAds(List<AdvertiserSettings> platformSettings, bool isTestAd, bool enableDebugger)
        {
            if (initialized == false)
            {
                Debug.Log("IronSource Start Initialization");
                
                //get settings
#if UNITY_ANDROID
                AdvertiserSettings settings =
                    platformSettings.First(cond => cond.platform == SupportedPlatforms.Android);
#elif UNITY_IOS
                AdvertiserSettings settings = platformSettings.First(cond => cond.platform == SupportedPlatforms.IOS);
#else
                AdvertiserSettings settings = new AdvertiserSettings(SupportedPlatforms.Windows,"", "", "","");
#endif
                //apply settings
                var appKey = !isTestAd ? settings.appId : ID_TEST_APP;

                //verify settings
                Debug.Log("IronSource plugin Version: " + IronSource.pluginVersion());

                //Dynamic config example
                IronSourceConfig.Instance.setClientSideCallbacks(true);

                string id = IronSource.Agent.getAdvertiserId();
                Debug.Log("IronSource GetAdvertiserId : " + id);
                
                IronSource.Agent.validateIntegration();
                IronSource.Agent.init(appKey);
                //IronSource.Agent.init(appKey, IronSourceAdUnits.BANNER, IronSourceAdUnits.INTERSTITIAL, IronSourceAdUnits.REWARDED_VIDEO);

                IronSourceEvents.onBannerAdLoadedEvent += BannerAdLoadedEvent;
                IronSourceEvents.onBannerAdLoadFailedEvent += BannerAdLoadFailedEvent;
                IronSourceEvents.onBannerAdClickedEvent += BannerAdClickedEvent;
                IronSourceEvents.onBannerAdScreenPresentedEvent += BannerAdScreenPresentedEvent;
                IronSourceEvents.onBannerAdScreenDismissedEvent += BannerAdScreenDismissedEvent;
                IronSourceEvents.onBannerAdLeftApplicationEvent += BannerAdLeftApplicationEvent;
                LoadBanner();

                InitInterstitial();
                InitRewardVideo();
                initialized = true;
                Debug.Log(this + " Init Complete: ");
            }
        }


        #region === INTERSTITIAL ===

        /// <summary>
        /// Check if IronSource interstitial is available
        /// </summary>
        /// <returns>true if an interstitial is available</returns>
        public bool IsInterstitialAvailable()
        {
            return initialized && IronSource.Agent.isInterstitialReady();
        }

        public void InitInterstitial()
        {
            IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
            IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
            IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
            IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
            IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
            IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
            IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;
            LoadInterstitial();
        }

        /// <summary>
        /// Show IronSource interstitial
        /// </summary>
        /// <param name="InterstitialClosed">callback called when user closes interstitial</param>
        public bool ShowInterstitial(UnityAction InterstitialClosed, string Placement, bool autoDisableLoading  = true)
        {
            if (IsInterstitialAvailable() && interstitialReadyTime <= Time.time)
            {
                OnInterstitialClosed = InterstitialClosed;
                autoDisableLoadingInter = autoDisableLoading;
                LoadingAdsCanvas.Instance.ShowLoading();
                obInter?.Dispose();
                obInter = Observable.Timer(TimeSpan.FromSeconds(0.5f), Scheduler.MainThreadIgnoreTimeScale)
                    .Take(1)
                    .Subscribe(_ =>
                    {
                        Advertisements.Instance.ResumeFromAds = true;
                        IronSource.Agent.showInterstitial(interstitialId);
                        Time.timeScale = 0; // Pause game
                    }).AddTo(this);
                return true;
            }
            
            Debug.Log("Interstitial not available or not ready Capping-Time");
            InterstitialClosed?.Invoke();
            LoadInterstitial();
            return false;
        }

        /// <summary>
        /// Loads IronSource interstitial
        /// </summary>
        private void LoadInterstitial()
        {
            Debug.Log("IronSource Start Loading Interstitial");

            IronSource.Agent.loadInterstitial();
        }

        /// <summary>
        /// IronSource specific event triggered after an interstitial was loaded
        /// </summary>
        private void InterstitialAdReadyEvent()
        {
            Debug.Log("IronSource Interstitial Loaded");

            currentRetryInterstitial = 0;
            OnInterstitialAvailable();
        }
        
        //IronSource: Invoked right before the Interstitial screen is about to open.
        void InterstitialAdShowSucceededEvent()
        {
            currentRetryInterstitial = 0;
        }

        /// <summary>
        /// IronSource: Invoked when the interstitial ad closed and the user goes back to the application screen.
        /// </summary>
        private void InterstitialAdClosedEvent()
        {
            Debug.Log("IronSource Reload Interstitial");

            Time.timeScale = 1; // Resume game
            interstitialReadyTime = Time.time + interstitialCappingTime;

            //reload interstitial
            LoadInterstitial();

            //trigger complete event
            StartCoroutine(CompleteMethodInterstitial());
        }

        /// <summary>
        ///  Because IronSource has some problems when used in multi threading applications with Unity a frame needs to be skipped before returning to application
        /// </summary>
        /// <returns></returns>
        private IEnumerator CompleteMethodInterstitial()
        {
            yield return new WaitForSeconds(1f);
            
            Advertisements.Instance.ResumeFromAds = false;

            if (OnInterstitialClosed != null)
            {
                OnInterstitialClosed();
                OnInterstitialClosed = null;
            }
            
            if (autoDisableLoadingInter)
            {
                LoadingAdsCanvas.Instance.HideLoading();
            }
        }

        /// <summary>
        /// IronSource: Invoked when the initialization process has failed.
        /// </summary>
        /// <param name="error"></param>
        private void InterstitialAdLoadFailedEvent(IronSourceError error)
        {
            Debug.Log(this + " Interstitial Failed To Load " + error.getCode() + "-" + error.getDescription());

            //try again to load a rewarded video
            if (currentRetryInterstitial < maxRetryCount)
            {
                currentRetryInterstitial++;
                Debug.Log("IronSource RETRY " + currentRetryInterstitial);
                StartCoroutine(ReloadInterstitial(reloadTime));
            }
        }
        
        /// <summary>
        /// IronSource specific event triggered if an interstitial failed to show
        /// </summary>
        /// <param name="error"></param>
        private void InterstitialAdShowFailedEvent(IronSourceError error)
        {
            Debug.Log(this + " Interstitial Failed To Show " + error.getCode() + "-" + error.getDescription());
            
            Time.timeScale = 1; // Resume game
            LoadingAdsCanvas.Instance.HideLoading();
            Advertisements.Instance.ResumeFromAds = false;
            
            OnInterstitialFailedToShow(error.getDescription());
            
            //try again to load a rewarded video
            if (currentRetryInterstitial < maxRetryCount)
            {
                currentRetryInterstitial++;
                Debug.Log("IronSource RETRY " + currentRetryInterstitial);
                StartCoroutine(ReloadInterstitial(reloadTime));
            }
        }
        
        //IronSource: Invoked when end user clicked on the interstitial ad
        void InterstitialAdClickedEvent()
        {
            AnalyticsManager.LogEventClickInterAds();
        }
        
        //IronSource: Invoked when the Interstitial Ad Unit has opened
        void InterstitialAdOpenedEvent()
        {
        }

        private IEnumerator ReloadInterstitial(float reloadTime)
        {
            yield return new WaitForSeconds(reloadTime);
            LoadInterstitial();
        }

        #endregion

        #region === REWARDED VIDEO ===

        /// <summary>
        /// Check if IronSource rewarded video is available
        /// </summary>
        /// <returns>true if a rewarded video is available</returns>
        public bool IsRewardVideoAvailable()
        {
            return initialized && IronSource.Agent.isRewardedVideoAvailable();
        }
        
        public void InitRewardVideo()
        {
            IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
            IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
            IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
            IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
            IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
            IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;
            IronSourceEvents.onRewardedVideoAdClickedEvent += RewardedVideoAdClickedEvent;
            LoadRewardedVideo();
        }

        public void LoadRewardedVideo()
        {
            IronSource.Agent.loadRewardedVideo();
        }

        public bool ShowRewardVideo(UnityAction<bool> CompleteMethod, string Placement)
        {
            if (IsRewardVideoAvailable())
            {
                OnRewardVideoCompleteMethod = CompleteMethod;
                Advertisements.Instance.ResumeFromAds = true;
                Time.timeScale = 0; // Pause game
                IronSource.Agent.showRewardedVideo();
                return true;
            }

            LoadRewardedVideo();
            ShowMessage("Ads not ready yet!");
            return false;
        }

        //IronSource: Invoked when the Rewarded Video failed to show
        private void RewardedVideoAdShowFailedEvent(IronSourceError error)
        {
            Debug.LogError("IronSource Rewarded Video OnAdFailedToShow " + error.getCode() + "-" + error.getDescription());

            Time.timeScale = 1; // Resume game
            Advertisements.Instance.ResumeFromAds = false;
            
            OnRewardVideoFailedToShow(error.getDescription());
            LoadRewardedVideo();
        }

        //IronSource: Invoked when the video ad starts playing.
        private void RewardedVideoAdStartedEvent()
        {
            Debug.Log("IronSource Rewarded Video OnAdOpening");
        }
        
        //IronSource: Invoked when the RewardedVideo ad view has opened.
        void RewardedVideoAdOpenedEvent()
        {
            Debug.Log(this + "IronSource Rewarded Video Displayed");
        }

        /// <summary>
        ///IronSource: Invoked when the RewardedVideo ad view is about to be closed. Your activity will now regain its focus.
        /// </summary>
        private void RewardedVideoAdClosedEvent()
        {
            Debug.Log("IronSource Rewarded Video OnAdClosed");
            
            Time.timeScale = 1; // Resume game
            Advertisements.Instance.ResumeFromAds = false;
            
            //load another rewarded video
            LoadRewardedVideo();
        }

        /// <summary>
        /// IronSource: specific event triggered after a rewarded video was fully watched
        /// </summary>
        /// <param name="placement"></param>
        private void RewardedVideoAdRewardedEvent(IronSourcePlacement placement)
        {
            Debug.Log($"IronSource Rewarded Video Watched: reward {placement.getRewardAmount()}");

            Time.timeScale = 1; // Resume game
            
            StartCoroutine(CompleteMethodRewardedVideo(true));
        }
        
        /// <summary>
        /// Because IronSource has some problems when used in multi-threading applications with Unity a frame needs to be skipped before returning to application
        /// </summary>
        /// <returns></returns>
        private IEnumerator CompleteMethodRewardedVideo(bool val)
        {
            yield return null;
            if (OnRewardVideoCompleteMethod != null)
            {
                OnRewardVideoCompleteMethod(val);
                OnRewardVideoCompleteMethod = null;
                OnRewardVideoCompleted();
            }

            yield return new WaitForSeconds(1.0f);
            
            Advertisements.Instance.ResumeFromAds = false;
        }

        /// <summary>
        /// IronSource: Invoked when there is a change in the ad availability status.
        /// </summary>
        /// <param name="available">value will change to true when rewarded videos are available</param>
        private void RewardedVideoAvailabilityChangedEvent(bool available)
        {
            if (available)
            {
                Debug.Log("IronSource Rewarded Video Loaded");
                //Value will change to true when rewarded videos are available
                OnRewardVideoAvailable();
            }
            else
            {
                Debug.Log("IronSource Rewarded Video Load - Failed");
                //Value will change to false when no videos are available
            }
        }
        
        void RewardedVideoAdClickedEvent (IronSourcePlacement placement)
        {
            Debug.Log ("IronSource Rewarded Video Clicked, name = " + placement.getRewardName());
            
            AnalyticsManager.LogEventClickRewardedVideoAds();
        }

        #endregion

        #region === BANNER ===

        /// <summary>
        /// Check if IronSource banner is available
        /// </summary>
        /// <returns>true if a banner is available</returns>
        public bool IsBannerAvailable()
        {
            return true;
        }

        /// <summary>
        /// Loads an IronSource banner
        /// </summary>
        /// <param name="position">display position</param>
        /// <param name="bannerType">can be normal banner or smart banner</param>
        private void LoadBanner(BannerPosition position = BannerPosition.BOTTOM, BannerType bannerType = BannerType.SmartBanner)
        {
            Debug.Log("IronSource Start Loading Banner");
            
            if(bannerLoaded) HideBanner();
            
            var bannerPosition = position == BannerPosition.BOTTOM ? IronSourceBannerPosition.BOTTOM : IronSourceBannerPosition.TOP;
            var bannerSize = bannerType == BannerType.Banner ? IronSourceBannerSize.BANNER : IronSourceBannerSize.SMART;
            IronSource.Agent.loadBanner(bannerSize, bannerPosition);
        }

        /// <summary>
        /// Show IronSource banner
        /// </summary>
        /// <param name="position"> can be TOP or BOTTOM</param>
        ///  /// <param name="bannerType"> can be Banner or SmartBanner</param>
        public void ShowBanner(BannerPosition position = BannerPosition.BOTTOM, BannerType bannerType = BannerType.SmartBanner,
            UnityAction<bool, BannerPosition, BannerType> DisplayResult = null)
        {
            LoadBanner(position, bannerType);
        }

        /// <summary>
        /// Hides IronSource banner
        /// </summary>
        public void HideBanner()
        {
            Debug.Log("IronSource Hide banner");
            
            if (DisplayResult != null)
            {
                DisplayResult(false, bannerPosition, bannerType);
                DisplayResult = null;
            }
            IronSource.Agent.destroyBanner();
        }

        /// <summary>
        /// Invoked once the banner has loaded
        /// </summary>
        private void BannerAdLoadedEvent()
        {
            Debug.Log("IronSource Banner Loaded");

            bannerLoaded = true;
            if (DisplayResult != null)
            {
                DisplayResult(true, bannerPosition, bannerType);
                DisplayResult = null;
            }
        }

        /// <summary>
        /// IronSource specific event triggered after banner failed to load
        /// </summary>
        /// <param name="error"></param>
        private void BannerAdLoadFailedEvent(IronSourceError error)
        {
            Debug.Log("IronSource Banner -> Error code: " + error.getCode() + "-" + error.getDescription());
            
            bannerLoaded = false;
            if (DisplayResult != null)
            {
                DisplayResult(false, bannerPosition, bannerType);
                DisplayResult = null;
            }
        }
        
        //Invoked when end user clicks on the banner ad
        void BannerAdClickedEvent()
        {
            AnalyticsManager.LogEventClickBannerAds();
        }
        
        //Notifies the presentation of a full screen content following user click
        void BannerAdScreenPresentedEvent()
        {
        }

        //Notifies the presented screen has been dismissed
        void BannerAdScreenDismissedEvent()
        {
        }

        //Invoked when the user leaves the app
        void BannerAdLeftApplicationEvent()
        {
        }

        #endregion
        
        #region === AOA ===
        
        public bool IsAppOpenAdAvailable()
        {
            return false;
        }
        
        public void LoadAppOpenAd(UnityAction actionLoadDone = null)
        {
            //todo
        }

        public bool ShowAppOpenAd(UnityAction AppOpenAdClosed)
        {
            //todo
            return false;
        }
        
        private void HandleAppOpenAdMessage(GameMessages.AppOpenAdMessage msg)
        {
            if (msg.AppOpenAdState != AppOpenAdState.TIME_OUT)
                return;

            appOpenAdLoaded = null;
        }
        
        // public void OnAppOpenDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        // {
        //     //todo
        // }
        //
        // private void OnAppOpenLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        // {
        //     //todo
        // }
        //
        // private void OnAppOpenLoadedFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        // {
        //     //todo
        // }
        //
        // public void OnAppOpenClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        // {
        //     Debug.Log("Recorded ad impression");
        //     //todo
        // }
        //
        // private void OnAppOpenDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        // {
        //     Debug.Log("Displayed app open ad");
        // }
        //
        // private void OnAppOpenPaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        // {
        //     Debug.LogFormat("Received paid event. (currency: {0}, value: {1}",
        //         adInfo.CreativeIdentifier, adInfo.Revenue);
        // }
        //
        // private void OnAppOpenDisplayedFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        // {
        //     
        // }
        
        #endregion
        
        private void OnApplicationFocus(bool focus)
        {
            if (focus == true)
            {
                if (IsInterstitialAvailable() == false)
                {
                    if (currentRetryInterstitial == maxRetryCount)
                    {
                        LoadInterstitial();
                    }
                }
            }
        }
        
        private void OnApplicationPause(bool isPaused)
        {
            IronSource.Agent.onApplicationPause(isPaused);
        }
    }
}
#endif
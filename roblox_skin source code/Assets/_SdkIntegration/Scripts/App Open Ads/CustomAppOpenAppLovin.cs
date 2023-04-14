//  ---------------------------------------------
//  Author:     DatDz <steven@atsoft.io> 
//  Copyright (c) 2022 AT Soft
// ----------------------------------------------

//using AppsFlyerSDK;
using UnityEngine;
using UnityEngine.Events;

#if AOA_TYPE_APPLOVIN
namespace ATSoft.Ads
{
    public class CustomAppOpenAppLovin : BaseAppOpenAds, ICustomAppOpenAds
    {
        public void InitializeAds(AppOpenAdvertiserSettings appOpenAdvertiserSettings, bool enableTestAd)
        {
            //apply settings
            this.enableTestAd = enableTestAd;
            appId = appOpenAdvertiserSettings.appId;
            ID_TIER_1 = appOpenAdvertiserSettings.ID_TIER_1;
            ID_TIER_2 = appOpenAdvertiserSettings.ID_TIER_2;
            ID_TIER_3 = appOpenAdvertiserSettings.ID_TIER_3;
            
            MaxSdkCallbacks.OnSdkInitializedEvent += ApplovinInitialized;
            
            //Initialize the SDK
            MaxSdk.SetSdkKey(appId);
            //Set UserId by AppsFlyer
            MaxSdk.SetUserId(AppsFlyer.getAppsFlyerId());
            MaxSdk.InitializeSdk();

            Debug.Log(this + " " + "Start Initialization");
            Debug.Log(this + " SDK key: " + appId);
        }
        private void Start()
        {
            GameUtils.AddHandler<GameMessages.AppOpenAdMessage>(HandleAppOpenAdMessage);
        }

        public bool IsAppOpenAdAvailable()
        {
            var ready = MaxSdk.IsAppOpenAdReady(ID_TIER_1);
            Debug.Log("ready " + ready);
            return ready;
        }

        public void LoadAppOpenAd(UnityAction actionLoadDone = null)
        {
            MaxSdk.LoadAppOpenAd(ID_TIER_1);
            appOpenAdLoaded = actionLoadDone;
        }

        public bool ShowAppOpenAd(UnityAction AppOpenAdClosed)
        {
            Debug.Log("ShowAppOpenAd " + (typeof(CustomAppOpenAppLovin)));
            
            if (enableTestAd)
            {
                appOpenAdClosed?.Invoke();
                return false;
            }

            if (IsAppOpenAdAvailable())
            {
                MaxSdk.ShowAppOpenAd(ID_TIER_1);
                return true;
            }

            LoadAppOpenAd();
            return false;
        }

        private void ApplovinInitialized(MaxSdk.SdkConfiguration obj)
        {
            MaxSdkCallbacks.AppOpen.OnAdHiddenEvent += OnAppOpenDismissedEvent;
            MaxSdkCallbacks.AppOpen.OnAdClickedEvent += OnAppOpenClickedEvent;
            MaxSdkCallbacks.AppOpen.OnAdRevenuePaidEvent += OnAppOpenPaidEvent;
            MaxSdkCallbacks.AppOpen.OnAdDisplayedEvent += OnAppOpenDisplayedEvent;
            MaxSdkCallbacks.AppOpen.OnAdDisplayFailedEvent += OnAppOpenDisplayedFailedEvent;
            MaxSdkCallbacks.AppOpen.OnAdLoadedEvent += OnAppOpenLoadedEvent;
            MaxSdkCallbacks.AppOpen.OnAdLoadFailedEvent += OnAppOpenLoadedFailedEvent;
 
            LoadAppOpenAd(() => ShowAppOpenAd(null));
        }
        
        public void OnAppOpenDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            isShowingAd = false;
            GameUtils.RaiseMessage(new GameMessages.AppOpenAdMessage {AppOpenAdState = AppOpenAdState.CLOSED});
            this.appOpenAdClosed?.Invoke();
            MaxSdk.LoadAppOpenAd(ID_TIER_1);
        }
        
        private void OnAppOpenLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            if (appOpenAdLoaded != null)
            {
                appOpenAdLoaded.Invoke();
                appOpenAdLoaded = null;
            }
        }
        
        private void OnAppOpenLoadedFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            MaxSdk.LoadAppOpenAd(ID_TIER_1);
        }
        
        public void OnAppOpenClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("Recorded ad impression");
            AnalyticsManager.LogEventClickAOAAds();
        }
        
        private void OnAppOpenDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("Displayed app open ad");
            isShowingAd = true;
        }
        
        private void OnAppOpenPaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.LogFormat("Received paid event. (currency: {0}, value: {1}",
                adInfo.CreativeIdentifier, adInfo.Revenue);
        }

        private void OnAppOpenDisplayedFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
        {
            
        }
        
        private void HandleAppOpenAdMessage(GameMessages.AppOpenAdMessage msg)
        {
            if (msg.AppOpenAdState != AppOpenAdState.TIME_OUT)
                return;

            appOpenAdLoaded = null;
        }
    }
}
#endif
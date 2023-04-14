using System.Collections.Generic;
using UnityEngine.Events;

//  ---------------------------------------------
//  Author:     DatDz <steven@atsoft.io> 
//  Copyright (c) 2022 AT Soft
// ----------------------------------------------
namespace ATSoft.Ads
{
    public interface ICustomAds
    {
        void InitializeAds(List<AdvertiserSettings> platformSettings, bool enableTestAd, bool enableDebugger);
        bool IsRewardVideoAvailable();
        void InitRewardVideo();
        bool ShowRewardVideo(UnityAction<bool> completeMethod, string placement);
        bool IsInterstitialAvailable();
        void InitInterstitial();
        bool ShowInterstitial(UnityAction interstitialClosed, string placement, bool autoDisableLoading);
        bool IsBannerAvailable();
        void ShowBanner(BannerPosition position, BannerType bannerType, UnityAction<bool, BannerPosition, BannerType> DisplayResult);
        void HideBanner();
        bool IsAppOpenAdAvailable();
        void LoadAppOpenAd(UnityAction actionLoadDone);
        bool ShowAppOpenAd(UnityAction appOpenAdClosed);
    }
    
    public enum BannerPosition
    {
        TOP,
        BOTTOM
    }

    public enum BannerType
    {
        Banner,
        SmartBanner,
        /// <summary>
        /// Only works for Admob
        /// </summary>
        Adaptive
    }
}
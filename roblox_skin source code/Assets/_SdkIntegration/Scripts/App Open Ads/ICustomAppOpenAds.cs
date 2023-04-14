using UnityEngine.Events;

//  ---------------------------------------------
//  Author:     DatDz <steven@atsoft.io> 
//  Copyright (c) 2022 AT Soft
// ----------------------------------------------
namespace ATSoft.Ads
{
    public interface ICustomAppOpenAds
    {
        void InitializeAds(AppOpenAdvertiserSettings appOpenAdvertiserSettings, bool enableTestAd);
        bool IsAppOpenAdAvailable();
        void LoadAppOpenAd(UnityAction actionLoadDone);
        bool ShowAppOpenAd(UnityAction AppOpenAdClosed);
    }
}
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

//  ---------------------------------------------
//  Author:     DatDz <steven@atsoft.io> 
//  Copyright (c) 2022 AT Soft
// ----------------------------------------------
namespace ATSoft.Ads
{
    [System.Serializable]
    public class Advertiser
    {
        public ICustomAds advertiserScript;
        public SupportedAdvertisers advertiser;
        public List<AdvertiserSettings> advertiserSettings;

        public Advertiser(ICustomAds _advertiserScript, SupportedAdvertisers _advertiser,
            List<AdvertiserSettings> _advertiserSettings)
        {
            this.advertiserScript = _advertiserScript;
            this.advertiser = _advertiser;
            this.advertiserSettings = _advertiserSettings;
        }
    }

    public enum SupportedAdvertisers
    {
        None = 0,
        Admob = 1,
        AppLovin = 2,
        IronSource = 3
    }

    public enum SupportedPlatforms
    {
        Android,
        IOS,
        Windows
    }

    [System.Serializable]
    public class AdvertiserSettings
    {
        public SupportedPlatforms platform;
        [Tooltip("~ SDK key in MAX Applovin")] public string appId;
        [TitleGroup("Setting Ad")] public string idBanner;
        [TitleGroup("Setting Ad")] public string idInterstitial;
        [TitleGroup("Setting Ad")] public string idRewarded;
        //[Separator("=== App Open Ad ===")]
        [TitleGroup("Setting App Open Ad")] public string ID_TIER_1;
        [TitleGroup("Setting App Open Ad")] public string ID_TIER_2;
        [TitleGroup("Setting App Open Ad")] public string ID_TIER_3;

        public AdvertiserSettings(SupportedPlatforms platform, string appId, string idBanner, string idInterstitial,
            string idRewarded, string ID_TIER_1, string ID_TIER_2, string ID_TIER_3)
        {
            this.platform = platform;
            this.idBanner = idBanner;
            this.idInterstitial = idInterstitial;
            this.idRewarded = idRewarded;
            this.ID_TIER_1 = ID_TIER_1;
            this.ID_TIER_2 = ID_TIER_2;
            this.ID_TIER_3 = ID_TIER_3;
        }
    }
}
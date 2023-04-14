using System.Collections.Generic;
using Firebase.Analytics;
using UnityEngine;

namespace ATSoft
{
    public class FirebaseStringEvent
    {
#if PUB_FAL //Fal
        public const string level_engage = "level_engage";
        public const string level_engage_passed = "level_engage_passed";
        public const string impdau_inter_passed = "impdau_inter_passed";
        public const string impdau_reward_passed = "impdau_reward_passed";

        public const string level_start = "level_start";
        public const string level_passed = "level_complete";
        public const string level_failed = "level_fail";
        
        public const string show_interstitial_ads = "ad_inter_show";
        public const string interstitial_ads_available = "ad_inter_load";
        public const string interstitial_ads_fail_show = "ad_inter_fail";
        public const string ad_click_interstitial = "ad_inter_click";
        
        public const string show_rewarded_ads = "ads_reward_show";
        public const string ad_click_reward = "ads_reward_click";
        public const string rewarded_ads_available = "ads_reward_offer";
        public const string rewarded_ads_fail_show = "ads_reward_fail";
        
        public const string ad_click_banner = "ad_click_banner";
        
#else
        public const string level_engage = "level_engage";
        public const string level_engage_passed = "level_engage_passed";
        public const string impdau_inter_passed = "impdau_inter_passed";
        public const string impdau_reward_passed = "impdau_reward_passed";

        public const string level_start = "level_start";
        public const string level_passed = "level_passed";
        public const string level_failed = "level_failed";

        public const string show_interstitial_ads = "show_interstitial_ads";
        public const string interstitial_ads_available = "ad_inter_load";
        public const string interstitial_ads_fail_show = "ad_inter_fail";
        public const string ad_click_interstitial = "ad_click_interstitial";

        public const string show_rewarded_ads = "show_rewarded_ads";
        public const string rewarded_ads_available = "ads_reward_offer";
        public const string rewarded_ads_fail_show = "ads_reward_fail";
        public const string ad_click_reward = "ad_click_reward";
        
        public const string ad_click_banner = "ad_click_banner";
#endif

        public const string daysPlaying = "days_playing";
        public const string ad_click_AOA = "ad_OnAdDidRecordImpression_aoa";

        #region ------------------------ GANE EVENT ------------------------------

        public const string ads_double_power = "ads_double_power";
        public const string ads_offer_weapon = "ads_offer_weapon";

        public const string start_main_mode_level = "start_main_mode_level_";
        public const string win_main_mode_level = "win_main_mode_level_";
        public const string lose_main_mode_level = "lose_main_mode_level_";

        public const string start_extra_mode_level = "start_extra_mode_level_";
        public const string win_extra_mode_level = "win_extra_mode_level_";
        public const string lose_extra_mode_level = "lose_extra_mode_level_";

        public const string skip_main_mode_level = "skip_main_mode_level_";
        public const string replay_main_mode_level = "replay_main_mode_level_";

        public const string ads_win_main_mode_spin = "ads_win_main_mode_spin";
        public const string ads_win_extra_mode_spin = "ads_win_extra_mode_spin";
        public const string skip_extra_mode_level = "skip_extra_mode_level_";

        public const string build_house = "build_house_";
        public const string free_spin = "free_spin";
        public const string ads_spin = "ads_spin";

        #endregion --------------------------------------------------------------
    }

    public class AppsFlyerStringEvent
    {
        public const string af_inters_api_called = "af_inters_api_called";
        public const string af_inters_ad_eligible = "af_inters_ad_eligible";
        public const string af_inters_displayed = "af_inters_displayed";
        
        public const string af_rewarded_ad_eligible = "af_rewarded_ad_eligible";
        public const string af_rewarded_api_called = "af_rewarded_api_called"; 
        public const string af_rewarded_ad_displayed = "af_rewarded_ad_displayed";
        public const string af_rewarded_ad_completed = "af_rewarded_ad_completed";
    }

    public class AnalyticsManager : MonoBehaviour
    {
        private static FirebaseManager m_FirebaseManager => FirebaseManager.Instance;

        public static void SetUserPropertyDayPlaying()
        {
            // Day Playing
            var daysPlayed = PlayerPrefs.GetInt("days_played", 0);
            var latestDayUpdateCount = PlayerPrefs.GetInt("latest_day", -1);

            if (latestDayUpdateCount != System.DateTime.Now.DayOfYear)
            {
                daysPlayed += 1;
                latestDayUpdateCount = System.DateTime.Now.DayOfYear;
                
                PlayerPrefs.SetInt("days_played", daysPlayed);
                PlayerPrefs.SetInt("latest_day", latestDayUpdateCount);
                
                if (m_FirebaseManager != null) m_FirebaseManager.SetUserProperty(FirebaseStringEvent.daysPlaying, daysPlayed.ToString());
            }
        }

        #region ------------------------ FUNC EVENT ------------------------------

        /// <summary>
        /// 
        ///     1, start game mode chính
        /// 
        /// </summary>
        /// <param name="level"></param>
        public static void GameLogEventStartLevelMain(int level)
        {
            m_FirebaseManager?.LogAnalyticsEvent(
                $"{FirebaseStringEvent.start_main_mode_level}{level}",
                new Parameter("Level", level));
            
            //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, level.ToString());
        }

        /// <summary>
        /// 
        ///     2, win game mode chính (chỉ cần win)
        /// 
        /// </summary>
        /// <param name="level"></param>
        public static void GameLogEventCompleteLevelMain(int level)
        {
            m_FirebaseManager?.LogAnalyticsEvent(
                $"{FirebaseStringEvent.win_main_mode_level}{level}",
                new Parameter("Level", level));
            
            //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, level.ToString());
        }

        /// <summary>
        /// 
        ///     3, lost game mode chính
        /// 
        /// </summary>
        /// <param name="level"></param>
        public static void GameLogEventFailLevelMain(int level)
        {
            m_FirebaseManager?.LogAnalyticsEvent(
                $"{FirebaseStringEvent.lose_main_mode_level}{level}",
                new Parameter("Level", level));
            
            //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, level.ToString());
        }

        /// <summary>
        /// 
        ///     4, start game mode phụ
        /// 
        /// </summary>
        /// <param name="level"></param>
        public static void GameLogEventStartLevelExtra(int level)
        {
            m_FirebaseManager?.LogAnalyticsEvent(
                $"{FirebaseStringEvent.start_extra_mode_level}{level}",
                new Parameter("Level", level));

            var fields = new Dictionary<string, object> {{"Level", level}};
            //GameAnalytics.NewDesignEvent (FirebaseStringEvent.start_extra_mode_level, fields);
        }

        /// <summary>
        /// 
        ///     5, mode phụ
        /// 
        /// </summary>
        /// <param name="level"></param>
        public static void GameLogEventCompleteLevelExtra(int level)
        {
            m_FirebaseManager?.LogAnalyticsEvent(
                $"{FirebaseStringEvent.win_extra_mode_level}{level}",
                new Parameter("Level", level));
            
            var fields = new Dictionary<string, object> {{"Level", level}};
            //GameAnalytics.NewDesignEvent (FirebaseStringEvent.win_extra_mode_level, fields);
        }

        /// <summary>
        /// 
        ///     6, mode phụ
        /// 
        /// </summary>
        /// <param name="level"></param>
        public static void GameLogEventFailLevelExtra(int level)
        {
            m_FirebaseManager?.LogAnalyticsEvent(
                $"{FirebaseStringEvent.lose_extra_mode_level}{level}",
                new Parameter("Level", level));
            
            var fields = new Dictionary<string, object> {{"Level", level}};
            //GameAnalytics.NewDesignEvent (FirebaseStringEvent.lose_extra_mode_level, fields);
        }

        /// <summary>
        /// 
        ///     7, bấm build nhà thứ X trong mode phụ
        /// 
        /// </summary>
        /// <param name="house"></param>
        public static void GameLogEventBuildHouseExtra(int house)
        {
            m_FirebaseManager?.LogAnalyticsEvent(
                $"{FirebaseStringEvent.build_house}{house}",
                new Parameter("HouseId", house));
            
            var fields = new Dictionary<string, object> {{"HouseId", house}};
            //GameAnalytics.NewDesignEvent (FirebaseStringEvent.build_house, fields);
        }

        /// <summary>
        /// 
        ///     8, skip level mode chính (chỉ cần bấm vào là check)
        /// 
        /// </summary>
        /// <param name="level"></param>
        public static void GameLogSkipMainLevel(int level)
        {
            m_FirebaseManager?.LogAnalyticsEvent(
                $"{FirebaseStringEvent.skip_main_mode_level}{level}",
                new Parameter("Level", level));
            
            var fields = new Dictionary<string, object> {{"Level", level}};
            //GameAnalytics.NewDesignEvent (FirebaseStringEvent.skip_main_mode_level, fields);
        }

        /// <summary>
        /// 
        ///     9, replay level mode chính (chỉ cần bấm vào là check)
        /// 
        /// </summary>
        /// <param name="level"></param>
        public static void GameLogReplayMainLevel(int level)
        {
            m_FirebaseManager?.LogAnalyticsEvent(
                $"{FirebaseStringEvent.replay_main_mode_level}{level}",
                new Parameter("Level", level));
            
            var fields = new Dictionary<string, object> {{"Level", level}};
            //GameAnalytics.NewDesignEvent (FirebaseStringEvent.replay_main_mode_level, fields);
        }

        /// <summary>
        /// 
        ///     10, Kiểm tra số lần bấm xem ads nhận thêm coin ở màn hình win 
        /// 
        /// </summary>
        public static void GameLogAdsWinMainSpin()
        {
            m_FirebaseManager?.LogAnalyticsEvent(
                $"{FirebaseStringEvent.ads_win_main_mode_spin}");
            
            //GameAnalytics.NewDesignEvent (FirebaseStringEvent.ads_win_main_mode_spin);
        }

        /// <summary>
        /// 
        ///     11, 
        /// 
        /// </summary>
        public static void GameLogAdsWinExtraSpin()
        {
            m_FirebaseManager?.LogAnalyticsEvent(
                $"{FirebaseStringEvent.ads_win_extra_mode_spin}");
            
            //GameAnalytics.NewDesignEvent (FirebaseStringEvent.ads_win_extra_mode_spin);
        }

        /// <summary>
        /// 
        ///     12, Skip extra mode button click
        /// 
        /// </summary>
        /// <param name="level"></param>
        public static void GameLogSkipExtraLevel(int level)
        {
            m_FirebaseManager?.LogAnalyticsEvent(
                $"{FirebaseStringEvent.skip_extra_mode_level}{level}",
                new Parameter("Level", level));
            
            var fields = new Dictionary<string, object> {{"Level", level}};
            //GameAnalytics.NewDesignEvent (FirebaseStringEvent.skip_extra_mode_level, fields);
        }

        /// <summary>
        /// 
        ///     13, Free spin button click (Menu)
        /// 
        /// </summary>
        public static void GameLogFreeSpin()
        {
            m_FirebaseManager?.LogAnalyticsEvent(
                $"{FirebaseStringEvent.free_spin}");
            
            //GameAnalytics.NewDesignEvent (FirebaseStringEvent.free_spin);
        }

        /// <summary>
        /// 
        ///     14, Ads spin button click (Menu)
        /// 
        /// </summary>
        public static void GameLogAdsSpin()
        {
            m_FirebaseManager?.LogAnalyticsEvent(
                $"{FirebaseStringEvent.ads_spin}");
            
            //GameAnalytics.NewDesignEvent(FirebaseStringEvent.ads_spin);
        }

        /// <summary>
        /// 
        ///     15, Ads Double Power click
        /// 
        /// </summary>
        /// <param name="level"></param>
        public static void DoublePowerButton(int level)
        {
            m_FirebaseManager?.LogAnalyticsEvent(
                $"{FirebaseStringEvent.ads_double_power}{level}",
                new Parameter("Level", level));

            var fields = new Dictionary<string, object> { { "Level", level } };
            //GameAnalytics.NewDesignEvent(FirebaseStringEvent.ads_double_power, fields);
        }

        /// <summary>
        /// 
        ///     16, Ads Offer Weapon Button click
        /// 
        /// </summary>
        /// <param name="level"></param>
        public static void OfferButtonClick(int level)
        {
            m_FirebaseManager?.LogAnalyticsEvent(
                $"{FirebaseStringEvent.ads_offer_weapon}{level}",
                new Parameter("Level", level));

            var fields = new Dictionary<string, object> { { "Level", level } };
            //GameAnalytics.NewDesignEvent(FirebaseStringEvent.ads_offer_weapon, fields);
        }

        #endregion --------------------------------------------------------------

        public static void LogEventLevelEngage(int level, int result)
        {
            m_FirebaseManager?.LogAnalyticsEvent(
                FirebaseStringEvent.level_engage,
                new Parameter("Level", level),
                new Parameter("Result", result == 0 ? "Lose" : "Win"));
        }

        public static void LogEventLevelEngagePassed(int engageWithLevel)
        {
            m_FirebaseManager?.LogAnalyticsEvent(
                FirebaseStringEvent.level_engage_passed,
                new Parameter("EngageWithLevel", engageWithLevel));
        }

        public static void LogEventImpdauInterPassed()
        {
            int impdauPassed = PlayerPrefs.GetInt("impdau_inter_passed", 0);
            impdauPassed++;
            PlayerPrefs.SetInt("impdau_inter_passed", impdauPassed);

            m_FirebaseManager?.LogAnalyticsEvent(
                FirebaseStringEvent.impdau_inter_passed,
                new Parameter("ImpdauPassed", impdauPassed));

            // Log appsflyer
            var para = new Dictionary<string, string>();
            para.Add("ImpdauPassed", impdauPassed.ToString());

            ////AppsFlyer.sendEvent(FirebaseStringEvent.impdau_inter_passed, para);
        }

        public static void LogEventImpdauRewardPassed()
        {
            int impdauPassed = PlayerPrefs.GetInt("impdau_reward_passed", 0);
            impdauPassed++;
            PlayerPrefs.SetInt("impdau_reward_passed", impdauPassed);

            m_FirebaseManager?.LogAnalyticsEvent(
                FirebaseStringEvent.impdau_reward_passed,
                new Parameter("ImpdauPassed", impdauPassed));

            // Log appsflyer
            var para = new Dictionary<string, string>();
            para.Add("ImpdauPassed", impdauPassed.ToString());

            //AppsFlyer.sendEvent(FirebaseStringEvent.impdau_reward_passed, para);
        }

        public static void LogEventLevelStart(int level)
        {
            m_FirebaseManager?.LogAnalyticsEvent(FirebaseStringEvent.level_start,
                new Parameter("Level", level));
        }
        
        public static void LogEventLevelStart(int level, int current_gold = 0)
        {
            m_FirebaseManager?.LogAnalyticsEvent(FirebaseStringEvent.level_start,
                new Parameter("level", level),
                new Parameter("current_gold", current_gold));
        }

        public static void LogEventLevelPassed(int level)
        {
            m_FirebaseManager?.LogAnalyticsEvent(FirebaseStringEvent.level_passed,
                new Parameter("Level", level));

            if (m_FirebaseManager != null) m_FirebaseManager.SetUserProperty("level_reach", level.ToString());
        }
        
        public static void LogEventLevelPassed(int level, int time_played = 0)
        {
            m_FirebaseManager?.LogAnalyticsEvent(FirebaseStringEvent.level_passed,
                new Parameter("level", level),
                new Parameter("time_played", time_played));

            if (m_FirebaseManager != null) m_FirebaseManager.SetUserProperty("level_reach", level.ToString());
        }

        public static void LogEventLevelFailed(int level)
        {
            m_FirebaseManager?.LogAnalyticsEvent(FirebaseStringEvent.level_failed,
                new Parameter("Level", level));
        }
        
        public static void LogEventLevelFailed(int level, int fail_count = 0)
        {
            m_FirebaseManager?.LogAnalyticsEvent(FirebaseStringEvent.level_failed,
                new Parameter("level", level),
                new Parameter("fail_count", fail_count));
        }

        public static void LogEventClickInterAds()
        {
            m_FirebaseManager?.LogAnalyticsEvent(FirebaseStringEvent.ad_click_interstitial);
        }
        
        public static void LogEventClickRewardedVideoAds()
        {
            m_FirebaseManager?.LogAnalyticsEvent(FirebaseStringEvent.ad_click_reward);
        }
        
        public static void LogEventClickBannerAds()
        {
            m_FirebaseManager?.LogAnalyticsEvent(FirebaseStringEvent.ad_click_banner);
        }
        
        public static void LogEventClickAOAAds()
        {
            m_FirebaseManager?.LogAnalyticsEvent(FirebaseStringEvent.ad_click_AOA);
        }

        public static void LogEventShowInterstitial() => LogEventShowInterstitial(1, "");
        public static void LogEventShowInterstitial(int hasAds, string placement)
        {
#if PUB_FAL //Fal
            m_FirebaseManager?.LogAnalyticsEvent(FirebaseStringEvent.show_interstitial_ads,
                new Parameter("placement", placement));

            var para = new Dictionary<string, string>();
            //AppsFlyer.sendEvent(AppsFlyerStringEvent.af_inters_ad_eligible, para);
            
            if(hasAds >= 1)
            {
                //AppsFlyer.sendEvent(AppsFlyerStringEvent.af_inters_displayed, para);
            }
#else
             m_FirebaseManager?.LogAnalyticsEvent(
                 FirebaseStringEvent.show_interstitial_ads,
                 new Parameter("has_ads", hasAds == 0 ? "No" : "Yes"),
                 new Parameter("internet_available", HasInternet()),
                 new Parameter("placement", placement));
             
             //GameAnalytics.NewAdEvent(GAAdAction.Show, GAAdType.Interstitial,"admob", placement);
             
             var para = new Dictionary<string, string>();
             //AppsFlyer.sendEvent(FirebaseStringEvent.show_interstitial_ads, para);
#if USE_ADJUST
             AdjustManager.Instance.TrackAdEvent(AdjustEventKeys.ad_inters_ad_eligible);
             if (hasAds >= 1)
             {
                 AdjustManager.Instance.TrackAdEvent(AdjustEventKeys.ad_inters_displayed);
             }
#endif
#endif
        }
        
        public static void LogEventInterstitialAvailable()
        {
            m_FirebaseManager?.LogAnalyticsEvent(FirebaseStringEvent.interstitial_ads_available);

            var para = new Dictionary<string, string>();
            //AppsFlyer.sendEvent(AppsFlyerStringEvent.af_inters_api_called, para);
#if USE_ADJUST
             AdjustManager.Instance.TrackAdEvent(AdjustEventKeys.ad_inters_api_called);
#endif
        }
        
        public static void LogEventInterstitialFailToShow(string errormsg)
        {
            m_FirebaseManager?.LogAnalyticsEvent(FirebaseStringEvent.interstitial_ads_fail_show,
                new Parameter("errormsg", errormsg));
            
            //GameAnalytics.NewAdEvent(GAAdAction.FailedShow, GAAdType.Interstitial,"admob","");
        }

        public static void LogEventShowReward() => LogEventShowReward(1, "");
        public static void LogEventShowReward(int hasAds, string placement)
        {
#if PUB_FAL
            m_FirebaseManager?.LogAnalyticsEvent(FirebaseStringEvent.show_rewarded_ads,
                new Parameter("placement", placement));

            var para = new Dictionary<string, string>();
            //AppsFlyer.sendEvent(AppsFlyerStringEvent.af_rewarded_ad_eligible, para);
            
            if(hasAds >= 1)
            {
                //AppsFlyer.sendEvent(AppsFlyerStringEvent.af_rewarded_ad_displayed, para);
            }
#else
            m_FirebaseManager?.LogAnalyticsEvent(
                FirebaseStringEvent.show_rewarded_ads,
                new Parameter("has_ads", hasAds == 0 ? "No" : "Yes"),
                new Parameter("internet_available", HasInternet()),
                new Parameter("placement", placement));
            
            //GameAnalytics.NewAdEvent(GAAdAction.Show, GAAdType.RewardedVideo,"admob", placement);

            var para = new Dictionary<string, string>();
            //AppsFlyer.sendEvent(FirebaseStringEvent.show_rewarded_ads, para);
#if USE_ADJUST
            AdjustManager.Instance.TrackAdEvent(AdjustEventKeys.ad_rewarded_ad_eligible);
            if (hasAds >= 1)
            {
                AdjustManager.Instance.TrackAdEvent(AdjustEventKeys.ad_rewarded_displayed);
            }
#endif
#endif
        }
        
        public static void LogEventRewardVideoAvailable()
        {
            m_FirebaseManager?.LogAnalyticsEvent(FirebaseStringEvent.rewarded_ads_available);

            var para = new Dictionary<string, string>();
            //AppsFlyer.sendEvent(AppsFlyerStringEvent.af_rewarded_api_called, para);
#if USE_ADJUST
             AdjustManager.Instance.TrackAdEvent(AdjustEventKeys.ad_rewarded_api_called);
#endif
        }

        public static void LogEventRewardVideoFailToShow(string errormsg)
        {
            m_FirebaseManager?.LogAnalyticsEvent(FirebaseStringEvent.rewarded_ads_fail_show,
                new Parameter("errormsg", errormsg));
            
            //GameAnalytics.NewAdEvent(GAAdAction.FailedShow, GAAdType.RewardedVideo,"admob","");
        }
        
        public static void LogEventRewardAdComplete()
        {
            var para = new Dictionary<string, string>();
            //AppsFlyer.sendEvent(AppsFlyerStringEvent.af_rewarded_ad_completed, para);
        }

        private static string HasInternet()
        {
            return Application.internetReachability == NetworkReachability.NotReachable ? "No" : "Yes";
        }
    }
}
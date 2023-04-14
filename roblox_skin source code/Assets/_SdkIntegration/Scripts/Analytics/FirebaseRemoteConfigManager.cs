using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using UnityEngine;
using UnityEngine.Events;

namespace ATSoft
{
    public class FirebaseRemoteConfigManager
    {
        public static int gameMultiplier = 5;
        public static int interstitialCappingTime = 0;
        public static int interstitialStartLevel = 2;
        public static List<int> levelRateGamePopUp = new List<int>();

        private UnityAction mFetchSuccessCallback;

        private static void SetupDefaultConfigs(Dictionary<string, object> defaults)
        {
            if (defaults == null) return;
            try
            {
                FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults);
                var cs = FirebaseRemoteConfig.DefaultInstance.ConfigSettings;
                FirebaseRemoteConfig.DefaultInstance.SetConfigSettingsAsync(cs);
            }
            catch (Exception)
            {
                // ignored
            }
        }
        public static ConfigValue GetValues(string key)
        {
            return FirebaseRemoteConfig.DefaultInstance.GetValue(key);
        }

        public void FetchData(UnityAction fetchSuccessCallback)
        {
            mFetchSuccessCallback = fetchSuccessCallback;
            // FetchAsync only fetches new data if the current data is older than the provided
            // timespan.  Otherwise it assumes the data is "recent enough", and does nothing.
            // By default the timespan is 12 hours, and for production apps, this is a good
            // number.  For this example though, it's set to a timespan of zero, so that
            // changes in the console will always show up immediately.
            try
            {
                var fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(
                    TimeSpan.Zero);
                fetchTask.ContinueWithOnMainThread(FetchComplete);

                //FirebaseCache.Instance.gameMultiplier = gameMultiplier;
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void FetchComplete(Task fetchTask)
        {
            if (fetchTask.IsCanceled)
            {
                Debug.Log("Fetch canceled.");
            }
            else if (fetchTask.IsFaulted)
            {
                Debug.Log("Fetch encountered an error.");
            }
            else if (fetchTask.IsCompleted)
            {
                Debug.Log("Fetch completed successfully!");
            }
            var info = FirebaseRemoteConfig.DefaultInstance.Info;
            switch (info.LastFetchStatus)
            {
                case LastFetchStatus.Success:
                    var taskActiveAsync = FirebaseRemoteConfig.DefaultInstance.FetchAndActivateAsync();
                    taskActiveAsync.ContinueWithOnMainThread((Action<Task<bool>>) SetupRemoteConfigs);
                    Debug.Log($"Remote data loaded and ready (last fetch time {info.FetchTime})");
                    mFetchSuccessCallback?.Invoke();
                    break;
                case LastFetchStatus.Failure:
                    switch (info.LastFetchFailureReason)
                    {
                        case FetchFailureReason.Error:
                            Debug.Log("Fetch failed for unknown reason");
                            break;
                        case FetchFailureReason.Throttled:
                            Debug.Log("Fetch throttled until " + info.ThrottledEndTime);
                            break;
                    }
                    break;
                case LastFetchStatus.Pending:
                    Debug.Log("Latest Fetch call still pending.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static void SetupDefaultConfigs()
        {
            var defaults = new Dictionary<string, object>
            {
                {Keys.ConfigGameMultiplier, 1},
                {Keys.ConfigAdCappingTime, 0},
                {Keys.ConfigIntersStartLevel, 2},
                {Keys.ConfigPopupRateUsLevel, "5,10,15,20,25,30,35,40,45,50,55,60,65"},
            };

            SetupDefaultConfigs(defaults);
        }
        
        private static void SetupRemoteConfigs(Task fetchTask)
        {
            if (!fetchTask.IsCompleted) return;

            gameMultiplier = (int)FirebaseRemoteConfig.DefaultInstance.GetValue(Keys.ConfigGameMultiplier).LongValue;
            interstitialCappingTime = (int)FirebaseRemoteConfig.DefaultInstance.GetValue(Keys.ConfigAdCappingTime).LongValue;
            interstitialStartLevel = (int)FirebaseRemoteConfig.DefaultInstance.GetValue(Keys.ConfigIntersStartLevel).LongValue;
            var popUpRateGame = FirebaseRemoteConfig.DefaultInstance.GetValue(Keys.ConfigPopupRateUsLevel).StringValue.Split(',');
            levelRateGamePopUp?.Clear();
            foreach (var t in popUpRateGame)
            {
                levelRateGamePopUp.Add(int.Parse(t));
            }
        }
    }
}

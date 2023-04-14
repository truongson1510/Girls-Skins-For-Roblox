using ATSoft;
using ATSoft.Ads;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class AdvertisementSample : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            UnityAction actionComplete = delegate()
            {
                Debug.Log("ShowInterstitial");
                SceneName sceneNameToLoad = SceneName.Menu;
                SceneManager.LoadScene(sceneNameToLoad.ToString(), LoadSceneMode.Single);
            };
            Advertisements.Instance.ShowInterstitial(actionComplete);
        }
        else if (Input.GetKeyUp(KeyCode.B))
        {
            Advertisements.Instance.ShowBanner(BannerPosition.BOTTOM, BannerType.SmartBanner);
        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            UnityAction<bool> actionComplete = delegate(bool isSuccess)
            {
                Debug.Log("ShowRewardedVideo " + isSuccess);
            };
            Advertisements.Instance.ShowRewardedVideo(actionComplete, "placement");
        }
    }

    public void ShowBanner()
    {
        Advertisements.Instance.ShowBanner(BannerPosition.BOTTOM, BannerType.Banner);
    }
    
    public void ShowSmartBanner()
    {
        Advertisements.Instance.ShowBanner(BannerPosition.BOTTOM, BannerType.SmartBanner);
    }
    
    public void ShowAdaptiveBanner()
    {
        Advertisements.Instance.ShowBanner(BannerPosition.BOTTOM, BannerType.Adaptive);
    }

    public void ShowInter()
    {
        UnityAction actionComplete = delegate()
        {
            Debug.Log("ShowInterstitial");
            SceneName sceneNameToLoad = SceneName.Menu;
            SceneManager.LoadScene(sceneNameToLoad.ToString(), LoadSceneMode.Single);
        };
        Advertisements.Instance.ShowInterstitial(actionComplete);
    }

    public void ShowRewardVideo()
    {
        UnityAction<bool> actionComplete = delegate(bool isSuccess)
        {
            Debug.Log("ShowRewardedVideo " + isSuccess);
        };
        Advertisements.Instance.ShowRewardedVideo(actionComplete, "placement");
    }
}
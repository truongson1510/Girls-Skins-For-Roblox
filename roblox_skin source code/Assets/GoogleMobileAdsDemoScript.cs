using System;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;

// Example script showing how to invoke the Google Mobile Ads Unity plugin.
public class GoogleMobileAdsDemoScript : MonoBehaviour
{   
    public static InterstitialAd interstitial;
    public static BannerView bannerdown;
    private RewardedAd rewardedAd;

 private static GoogleMobileAdsDemoScript _instance;
    public static GoogleMobileAdsDemoScript Instance {get{return _instance;}}
    public void Awake()
    { 
        
         if(_instance !=null) {
            Destroy(gameObject);
         }else{
            _instance=this;
            DontDestroyOnLoad(gameObject);
         }
    }
void Start(){
    RequestInterstitial();
        //reword loading 
      //  LoadingAds();
        //Fuction with reword 
        // Called when the user should be rewarded for interacting with the ad.
       // this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
     //   this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        //RequestBannerBottom();
      
}
    // Returns an ad request with custom ad targeting.
    private static AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
            //.AddTestDevice(AdRequest.TestDeviceSimulator)
            //.AddTestDevice("0123456789ABCDEF0123456789ABCDEF")
            .AddKeyword("game")
            //.SetGender(Gender.Male)
            //.SetBirthday(new DateTime(1985, 1, 1))
            //.TagForChildDirectedTreatment(false)
            .AddExtra("color_bg", "9B30FF")
            .Build();
    }


    //INTERSTITIAL ___________________________________________________________________________________
    public  void ShowInterstitial()
    {
        //if (!Show.In_Review)
        //{

        if (interstitial.IsLoaded())
        {
            interstitial.Show();
            RequestInterstitial();
            MonoBehaviour.print("show ads");
        }
        else
        {
            RequestInterstitial();
            MonoBehaviour.print("Interstitial is not ready yet");
        }
        //}
    }


    public static void RequestInterstitial()
    {
        //if (!Show.In_Review)
        //{

        // These ad units are configured to always serve test ads.
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";

        // Create an interstitial.
        interstitial = new InterstitialAd(adUnitId);

        // Load an interstitial ad.
        interstitial.LoadAd(CreateAdRequest());
        //}
    }


    // BANNER DOWN ___________________________________________________________________________________


    /**public static void RequestBannerBottom()
	{
        //if (!Show.In_Review)
        //{

            //string bannerId = "ca-app-pub-3940256099942544/6300978111";
            string bannerId = "ca-app-pub-2300650880847820/7406750177";
            bannerdown = new BannerView(bannerId, AdSize.SmartBanner, AdPosition.Bottom);
            AdRequest adResquest = new AdRequest.Builder().Build();
            bannerdown.LoadAd(adResquest);
        //}
	}

	/*public static void ShowBannerBottom()
	{
        //if (!Show.In_Review)
        {
            bannerdown.Show();
        }
	}

	public static void DestroyBannerBottom()
	{
		bannerdown.Destroy();
	}

    **/

     // Reword ads  ___________________________________________________________________________________
     //


/*
 * private void LoadingAds(){
   string  adUnitId = "ca-app-pub-2300650880847820/8528260153";

        this.rewardedAd = new RewardedAd(adUnitId);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);

}
public void ShowRewordVidio()
{
  if (this.rewardedAd.IsLoaded()) {
    this.rewardedAd.Show();
      LoadingAds();
  }else{
     LoadingAds();
      Debug.Log("Not load ads");
  }
}
public bool LoadingReword(){
    return this.rewardedAd.IsLoaded();
}
////////////
 public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdClosed event received");
    }
////////////
public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);
    }


    */



}
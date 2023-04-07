
using UnityEngine;
using System.Collections.Generic;

public class RatingUIController : MonoBehaviour
{
    public string packageName;
    public string appleAppId;

    private string linkApp;

    [SerializeField] private GameObject RatingBanner;

    List<StarIcon> ratingIcons = new List<StarIcon>();

    private int ratingIconCount;
    /// private int currentRate;

    private void Start()
    {
        Observer.Instance.AddObserver(StringCollection.EVENT_STAR_CLICK, ReloadUI);

#if UNITY_EDITOR
        linkApp = "https://play.google.com/store/apps/details?id=" + packageName;
#elif UNITY_IOS
        linkApp = "itms-apps://itunes.apple.com/app/id" + appleAppId;
#elif UNITY_ANDROID
        linkApp = "market://details?id=" + packageName;
#endif

        /// Get child objects from Warning-Banner
        ratingIconCount = RatingBanner.transform.childCount;

        for (int i = 0; i < ratingIconCount; i++)
        {
            ratingIcons.Add(RatingBanner.transform.GetChild(i).gameObject.GetComponent<StarIcon>());
        }

        /// Removable
        /// ReloadUI(currentRate);
    }

    private void ReloadUI(object currentRate)
    {
        int rating = (int)currentRate;

        for (int i = 0; i < ratingIconCount; i++)
        {
            if (i < rating)
                ratingIcons[i].TurnOnStar(true);
            else
                ratingIcons[i].TurnOnStar(false);
        }
    }

    public void OpenStore()
    {
        PlayerPrefs.SetInt(StringCollection.DATA_RATED, 1);
        Application.OpenURL(linkApp);
    }

    private void OnDestroy()
    {
        Observer.Instance.RemoveObserver(StringCollection.EVENT_STAR_CLICK, ReloadUI);
    }
}

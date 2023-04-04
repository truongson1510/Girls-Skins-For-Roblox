using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] boyOptions;
    public GameObject[] girlOptions;
    public GameObject caption , text ,ui;
    // Start is called before the first frame update
    void Start()
    {
        initializeMode();
    }

    private void initializeMode()
    {
        foreach (GameObject item in girlOptions)
            item.SetActive(true);

        if (PlayerPrefs.GetInt("player") == 0)
            foreach (GameObject item in girlOptions)
                item.SetActive(true);
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takePicture()
    {
        ScreenCapture.CaptureScreenshot("character");
        ui.SetActive(false);
        StartCoroutine(CRSaveScreenshot());
        caption.GetComponent<Animator>().SetTrigger("Capture");
        text.SetActive(false);
        //StartCoroutine(animateCaption());
        Debug.Log("SHOW INTER HERE: ");
    }
    IEnumerator animateCaption()
    {
        //LeanTween.scale(caption, new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
        yield return new WaitForSeconds(0.2f);
        //LeanTween.scale(caption, new Vector3(1f, 1f, 1f), 0.2f);
    }

    public void loadSceenHome(string nameSceen)
    {
        SceneManager.LoadScene(nameSceen);
    }



    IEnumerator CRSaveScreenshot()
    {
        yield return new WaitForEndOfFrame();
        Texture2D txt = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        txt.ReadPixels(new Rect(0 , 0 , Screen.width , Screen.height ), 0 , 0 );
        txt.Apply();
        string name = "ScreenGame" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";

        NativeGallery.SaveImageToGallery(txt , "MyGame" ,name );
        ui.SetActive(true);
        /*
        yield return new WaitForEndOfFrame();


        string myFileName = "Screenshot" + System.DateTime.Now.Hour + System.DateTime.Now.Minute + System.DateTime.Now.Second + ".png";
        string myDefaultLocation = Application.persistentDataPath + "/" + myFileName;
        string myFolderLocation = "/storage/emulated/0/DCIM/Camera/JCB/";  //EXAMPLE OF DIRECTLY ACCESSING A CUSTOM FOLDER OF THE GALLERY
        string myScreenshotLocation = myFolderLocation + myFileName;

        //ENSURE THAT FOLDER LOCATION EXISTS
        if (!System.IO.Directory.Exists(myFolderLocation))
        {
            System.IO.Directory.CreateDirectory(myFolderLocation);
        }

        ScreenCapture.CaptureScreenshot(myFileName);
        //MOVE THE SCREENSHOT WHERE WE WANT IT TO BE STORED

        yield return new WaitForSeconds(0.3f);

        System.IO.File.Move(myDefaultLocation, myScreenshotLocation);

        //REFRESHING THE ANDROID PHONE PHOTO GALLERY IS BEGUN
        AndroidJavaClass classPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject objActivity = classPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass classUri = new AndroidJavaClass("android.net.Uri");
        AndroidJavaObject objIntent = new AndroidJavaObject("android.content.Intent", new object[2] { "android.intent.action.MEDIA_MOUNTED", classUri.CallStatic<AndroidJavaObject>("parse", "file://" + myScreenshotLocation) });
        objActivity.Call("sendBroadcast", objIntent);*/
    }

    }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.iOS;



public class MenuChanger : MonoBehaviour
{





    public void To_Gender_Selection()
    {
        SceneManager.LoadScene("Style");
        Debug.Log("SHOW INTER HERE: ");



    }


    public void To_MapDance_Scene()
    {
        SceneManager.LoadScene("Map_Dance_scene");
        Debug.Log("SHOW INTER HERE: ");



    }

    public void HOME_Button()
    {
        SceneManager.LoadScene("1_Start_scene");



    }




    public void OnClickPause()
    {
        Time.timeScale = 0f;
    }


 



    public void RateUs()
    {
      //  Device.RequestStoreReview();


    }


    public void PrivacyButton2()
    {
        Application.OpenURL("https://google.com");

    }
}

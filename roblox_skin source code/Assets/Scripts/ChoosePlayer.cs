using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoosePlayer : MonoBehaviour
{
    public GameObject[] characters;
    public GameObject[] girlOption;
    public GameObject[] boyOption;
    int currCharacter=0;
    public void changePlayer()
    {
        foreach (GameObject item in characters) item.SetActive(false);
        foreach (GameObject item in boyOption) item.SetActive(false);
        foreach (GameObject item in girlOption) item.SetActive(false);
        if(currCharacter==0)
        {
            characters[1].SetActive(true);
            currCharacter = 1;
            foreach (GameObject item in boyOption) item.SetActive(true);
        }
        else
        {
            characters[0].SetActive(true);
            currCharacter = 0;
            foreach (GameObject item in girlOption) item.SetActive(true);
        }
        
    }
    public void loadNextScean()
    {
        PlayerPrefs.SetInt("player", currCharacter);
        SceneManager.LoadScene("Style");
    }
}

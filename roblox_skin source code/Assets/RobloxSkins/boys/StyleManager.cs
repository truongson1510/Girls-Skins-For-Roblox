using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StyleManager : MonoBehaviour
{
    [Header("player")]
    [SerializeField]
    GameObject body , legs , feet , face;

    [Header("styles")]
    [SerializeField] Material[] clothes , faces;
    [SerializeField]
    GameObject[] hair, hats , glasses , Backgrounds;

    public void changeHaire(int index)
    {
        if (index < hair.Length)
        {
            foreach (GameObject item in hair)
                item.SetActive(false);
            hair[index].SetActive(true);
        }
    }

    public void changeTshirt(int index)
    {
        for(int i = 0; i < body.transform.childCount; i++)
        {
            body.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().material = clothes[index];
        }
    }
    public void changePants(int index)
    {
        for(int i = 0; i < legs.transform.childCount; i++)
        {
            legs.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().material = clothes[index];
        }
    }
    
    public void changeShoe(int index)
    {
        for(int i = 0; i < feet.transform.childCount; i++)
        {
            feet.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().material = clothes[index];
        }
    }

    public void changeHat(int index)
    {
        if (index < hats.Length)
        {
            foreach (GameObject item in hats)
                item.SetActive(false);
            if(index!=0) hats[index].SetActive(true);
        }
    }

    public void changeGlasses(int index)
    {
        if (index < glasses.Length)
        {
            foreach (GameObject item in glasses)
                item.SetActive(false);
            if (index != 0) glasses[index].SetActive(true);
        }
    }

    public void changeFace(int index)
    {
        face.GetComponent<MeshRenderer>().material = faces[index];
    }

    public void changeBackground(int index)
    {
        if (index < Backgrounds.Length)
        {
            foreach (GameObject item in Backgrounds)
                item.SetActive(false);
            if (index != 0) Backgrounds[index].SetActive(true);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapsManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] maps;
    public void changeMap(int index)
    {
        if (index < maps.Length)
        {
            foreach (GameObject Item in maps)
                Item.SetActive(false);

            maps[index].SetActive(true);
        }
    }
}

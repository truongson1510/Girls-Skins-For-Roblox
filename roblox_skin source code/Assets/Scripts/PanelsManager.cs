using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelsManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] stylePanels;
    // Start is called before the first frame update
    public void changePanel(int index)
    {
        if (index < stylePanels.Length)
        {
            foreach (GameObject item in stylePanels)
                item.SetActive(false);
            stylePanels[index].SetActive(true);
        }
    }
}

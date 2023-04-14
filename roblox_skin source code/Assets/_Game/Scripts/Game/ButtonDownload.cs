using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using System;

public class ButtonDownload : MonoBehaviour
{
    #region Inspector Variables

    #endregion

    #region Member Variables

    private Button button;

    private ItemData pantData;
    private ItemData shirtData;

    #endregion

    #region Properties
    #endregion

    #region Unity Methods

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SaveToGallery);
    }

    #endregion

    #region Public Methods
    #endregion

    #region Protected Methods
    #endregion

    #region Private Methods

    private void UpdateData()
    {
        pantData = GameManager.Instance.pantData;
        shirtData = GameManager.Instance.shirtData;
    }

    private void SaveToGallery()
    {
        // Update current data
        UpdateData();

        // Save Pant skin
        NativeGallery.SaveImageToGallery(pantData.robloxTexture, "Roblox Skins", pantData.robloxTexture.name);
        NativeGallery.SaveImageToGallery(shirtData.robloxTexture, "Roblox Skins", shirtData.robloxTexture.name);
    }

    #endregion
}

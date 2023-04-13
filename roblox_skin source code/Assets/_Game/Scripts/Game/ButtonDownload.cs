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

    private ItemData pantData;
    private ItemData shirtData;

    #endregion

    #region Member Variables

    private Button button;

    #endregion

    #region Properties
    #endregion

    #region Unity Methods

    private void Awake()
    {
        button = GetComponent<Button>();
#if UNITY_ANDROID
        button.onClick.AddListener(SaveImageToPicturesFolderAndroid);
#elif UNITY_IOS
        button.onClick.AddListener(SaveImageToPicturesFolderIOS);
#else
            
#endif
    }

    #endregion

    #region Public Methods
    #endregion

    #region Protected Methods
    #endregion

    #region Private Methods

    private void UpdateData()
    {
        pantData    = GameManager.Instance.pantData;
        shirtData   = GameManager.Instance.shirtData;
    }

    public void SaveImageToPicturesFolderAndroid()
    {
        UpdateData();

        // Check if the WRITE_EXTERNAL_STORAGE permission is granted
        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            // If the permission is granted, call the SaveImageToPicturesFolderInternal() function
            SaveImageToPicturesFolderInternal(pantData);
            SaveImageToPicturesFolderInternal(shirtData);
        }
        else
        {
            // If the permission is not granted, request it from the user
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
    }

    private void SaveImageToPicturesFolderInternal(ItemData data)
    {
        // Convert the texture to PNG bytes
        byte[] bytes = data.robloxTexture.EncodeToPNG();

        // Create a file name for the saved image
        string fileName = $"{data.name}_{data.robloxTexture.name}.png";

        // Define the destination path on the Android Pictures folder
        string destinationPath = Path.Combine($"{Application.dataPath}", fileName);

        // Write the bytes to the destination path
        File.WriteAllBytes(destinationPath, bytes);

        // Refresh the Android Media Scanner to show the saved image in the Gallery app
        using (AndroidJavaClass mediaScanner = new AndroidJavaClass("android.media.MediaScannerConnection"))
        {
            mediaScanner.CallStatic("scanFile", new string[] { destinationPath }, new string[] { "image/png" }, null);
        }

        // Print a debug message
        Debug.Log("Image saved to Android Pictures folder: " + destinationPath);
    }

    public void SaveImageToPicturesFolderIOS()
    {
        UpdateData();

        SaveImageToPhotosLibraryiOS(pantData);
        SaveImageToPhotosLibraryiOS(shirtData);
    }

    public void SaveImageToPhotosLibraryiOS(ItemData data)
    {
        // Convert the texture to PNG bytes
        byte[] bytes = data.robloxTexture.EncodeToPNG();

        // Create a file name for the saved image
        string fileName = $"{data.name}_{data.robloxTexture.name}.png";

        // Define the destination path on iOS temporary directory
        string destinationPath = Path.Combine(Application.temporaryCachePath, fileName);

        // Write the bytes to the destination path
        File.WriteAllBytes(destinationPath, bytes);

        // Save the image to the Photos library
        StartCoroutine(SaveImageToPhotosLibrary(destinationPath));
    }

    private IEnumerator SaveImageToPhotosLibrary(string imagePath)
    {
        // Request authorization to access Photos library
        yield return new WaitForEndOfFrame();
        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(imagePath, "My Gallery", "Image.png");

        // Check the result of the save operation
        if (permission == NativeGallery.Permission.Granted)
        {
            Debug.Log("Image saved to iOS Photos library");
        }
        else
        {
            Debug.Log("Failed to save image to iOS Photos library");
        }

        // Delete the temporary image file
        File.Delete(imagePath);
    }





    #endregion
}

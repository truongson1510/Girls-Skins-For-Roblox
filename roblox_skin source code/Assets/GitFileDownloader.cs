
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using Sirenix.OdinInspector;

public class GitFileDownloader : MonoBehaviour
{
    public string repositoryUrl; // The base URL of the Git repository
    public string folderPath; // The relative path of the folder within the repository
    public string localDirectory; // The local directory to save the downloaded files

    [Button]
    public void DownloadFolder()
    {
        string folderUrl = repositoryUrl + "/tree/main/" + folderPath; // Construct the URL of the folder
        Debug.Log($"folder URL: <color=orange> {folderUrl} </color>");
        StartCoroutine(DownloadFiles(folderUrl)); // Start the download coroutine
    }

    IEnumerator DownloadFiles(string folderUrl)
    {
        UnityWebRequest request = UnityWebRequest.Get(folderUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Failed to download folder: " + request.error);
            yield break;
        }

        string[] fileUrls = ExtractFileUrls(request.downloadHandler.text);
        foreach (string fileUrl in fileUrls)
        {
            UnityWebRequest fileRequest = UnityWebRequest.Get(fileUrl);
            yield return fileRequest.SendWebRequest();

            if (fileRequest.result == UnityWebRequest.Result.ConnectionError || fileRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Failed to download file: " + fileRequest.error);
                continue;
            }

            string fileName = ExtractFileName(fileUrl);
            string localPath = localDirectory + "/" + fileName;
            System.IO.File.WriteAllBytes(localPath, fileRequest.downloadHandler.data);
            Debug.Log("Downloaded file: " + fileName);
        }

        Debug.Log("Download complete!");
    }

    string[] ExtractFileUrls(string folderHtml)
    {
        // Extract the raw file URLs from the HTML of the folder
        // This may require parsing and string manipulation based on the HTML structure of the hosting platform
        // Here's a simple example assuming the raw file URLs are enclosed in <a> tags with href attributes
        string[] splitHtml = folderHtml.Split(new string[] { "<a href=\"" }, System.StringSplitOptions.RemoveEmptyEntries);
        string[] fileUrls = new string[splitHtml.Length - 1];
        for (int i = 1; i < splitHtml.Length; i++)
        {
            int endIndex = splitHtml[i].IndexOf("\"");
            fileUrls[i - 1] = repositoryUrl + "/" + splitHtml[i].Substring(0, endIndex);
        }
        return fileUrls;
    }

    string ExtractFileName(string fileUrl)
    {
        // Extract the file name from the file URL
        int lastSlashIndex = fileUrl.LastIndexOf("/");
        return fileUrl.Substring(lastSlashIndex + 1);
    }
}

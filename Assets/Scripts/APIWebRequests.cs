using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public static class APIWebRequests {

    private class WebRequestsMonoBehaviour : MonoBehaviour { }

    private static WebRequestsMonoBehaviour _webRequestsMonoBehaviour;

    private static void Init() {
        if (_webRequestsMonoBehaviour == null) {
            GameObject gameObject = new GameObject("WebRequests");
            _webRequestsMonoBehaviour = gameObject.AddComponent<WebRequestsMonoBehaviour>();
        }
    }

    public static void Get(string url, Action<string> onError, Action<string> onSuccess) {
        Init();
        _webRequestsMonoBehaviour.StartCoroutine(GetCoroutine(url, onError, onSuccess));
    }

    private static IEnumerator GetCoroutine(string url, Action<string> onError, Action<string> onSuccess) {
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(url)) {
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError ||
                unityWebRequest.result == UnityWebRequest.Result.DataProcessingError ||
                unityWebRequest.result == UnityWebRequest.Result.ProtocolError) {
                // Error
                onError(unityWebRequest.error);
            } else {
                onSuccess(unityWebRequest.downloadHandler.text);
            }
        }
    }
    
    public static void PostJson(string url, string jsonData, Action<string> onError, Action<string> onSuccess) {
        Init();
        _webRequestsMonoBehaviour.StartCoroutine(GetCoroutinePostJson(url, jsonData, onError, onSuccess));
    }
    

    private static IEnumerator GetCoroutinePostJson(string url, string jsonData, Action<string> onError, Action<string> onSuccess) {
        using (UnityWebRequest unityWebRequest = new UnityWebRequest(url, "POST")) {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            unityWebRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            unityWebRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            unityWebRequest.SetRequestHeader("Content-Type", "application/json");

            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError ||
                unityWebRequest.result == UnityWebRequest.Result.DataProcessingError ||
                unityWebRequest.result == UnityWebRequest.Result.ProtocolError) {
                // Error
                onError(unityWebRequest.error);
            } else {
                onSuccess(unityWebRequest.downloadHandler.text);
            }
        }
    }
}

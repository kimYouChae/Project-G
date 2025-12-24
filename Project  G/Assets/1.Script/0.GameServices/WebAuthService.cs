using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebAuthService : IAuthService
{
    private string baseUrl;
    private string loginUrl = "Login/Login";

    public WebAuthService(string url) 
    {
        this.baseUrl = url;
    }

    public void AuthService(string steamID, string nick, string country)
    {
        UnityWebRequest request = UnityWebRequest.Get(baseUrl + loginUrl);

        // StartCorutine(StartRequest(request));

    }

    IEnumerator StartRequest(UnityWebRequest request) 
    {
        // 요청보내기 (비동기)
        yield return request.SendWebRequest();

        string responseText = request.downloadHandler.text;
    }
}

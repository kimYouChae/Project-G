using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class LoginRequestDTO
{
    public string SteamID { get; set; }
    public string NickName { get; set; }
    public string Country { get; set; }
}


public class WebAuthService : IAuthService
{
    private string baseUrl;
    private static string loginUrl = "Login/Login";

    public WebAuthService(string url) 
    {
        this.baseUrl = url;
    }

    public void AuthService(string steamID, string nick, string country)
    {
        LoginRequestDTO loginRequestDTO = new LoginRequestDTO() 
        {
            SteamID = steamID,
            NickName = nick,
            Country = country
        };

        string requestJson = JsonUtility.ToJson(loginRequestDTO);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestJson);

        var request = new UnityWebRequest(baseUrl + loginUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);

        CoroutineHandler.Instance.Run(StartRequest(request));
    }

    IEnumerator StartRequest(UnityWebRequest request) 
    {
        // 요청보내기 (비동기)
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
            yield break;
        }
        
        // 받은 요청을 string 타입으로
        string responseText = request.downloadHandler.text;
        Debug.Log(responseText);

        // ##TODO : 여기서 responseText를 Json으로 파싱해서 UserData에 담아줘야함.
    }
}

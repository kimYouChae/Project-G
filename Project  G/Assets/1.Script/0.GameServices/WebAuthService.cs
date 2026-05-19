using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;
using Newtonsoft.Json;

#region DTO
public class LoginRequestDTO
{
    public string SteamID;
    public string NickName;
    public string Country;
}

[Serializable]
public class LoginResponseDTO
{
    // 맵 타입별 점수 리스트 
    public List<UserMapTypeByScoreDTO> userScoreData;
    // 신규 유저 유무
    public bool isNewer;
}

[Serializable]
public class UserMapTypeByScoreDTO
{
    public int mapType;
    public float bestScore;
    public int bestStage;
    public DateTime createdAt;
}

#endregion

public class WebAuthService : IAuthService
{
    private string loginUrl = "";

    public WebAuthService(string url) 
    {
        this.loginUrl = url;
    }

    public IEnumerator AuthService(long steamID, string nick, string country)
    {
        LoginRequestDTO loginRequestDTO = new LoginRequestDTO() 
        {
            SteamID = steamID.ToString(),
            NickName = nick,
            Country = country
        };

        string requestJson = JsonUtility.ToJson(loginRequestDTO);

        yield return WebRequestCore.CommonLogic<LoginResponseDTO>
            (
                requestJson,
                loginUrl,
                HttpRequestType.Post,
                LoginUser,
                () => LoginFailed()
            );
    }

    private void LoginFailed()
    {
        
    }

    private void LoginUser(LoginResponseDTO loginResponse) 
    {
        if (loginResponse.isNewer == false) 
        {
            Debug.Log("WebAuthService : 기존 유저 로그인 성공 ");

            // UserDataManager에 값 넣어주기
            List<UserMapTypeByScoreDTO> typebyscore = loginResponse.userScoreData;
            for(int i = 0; i < typebyscore.Count; i++) 
            {
                var user = typebyscore[i];

                UserDataManager.Instance.SetScoreByMapType((MapType)user.mapType, user.bestScore, user.bestStage);
            }
            return;
        }

        // 새로운 유저이면
        Debug.Log("WebAuthService : 새로운 유저 로그인 성공 ");
    }
}

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
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestJson);

        var request = new UnityWebRequest(loginUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.SetRequestHeader("Content-Type", "application/json");
        request.downloadHandler = new DownloadHandlerBuffer(); // 응답 바디 확인용

        //request.timeout = 10;
        //request.useHttpContinue = false;

        yield return CoroutineHandler.Instance.Run(StartRequest(request));
    }

    IEnumerator StartRequest(UnityWebRequest request) 
    {
        Debug.Log($"[HTTP] 요청 코루틴 시작 URL = {request.url}");

        // 요청보내기 (비동기)
        yield return request.SendWebRequest();
        Debug.Log($"[{nameof(WebAuthService)}] / responseBody={request.downloadHandler?.text}");

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
            yield break;
        }
        
        // 받은 요청을 string 타입으로
        string responseText = request.downloadHandler.text;

        Pasing(responseText);
    }

    private void Pasing(string responseText) 
    {
        // LoginAPi의 응답은 APi Response 타입의 Json임 
        ApiResponse<LoginResponseDTO> apiResponse 
            = JsonConvert.DeserializeObject<ApiResponse<LoginResponseDTO>>(responseText);

        if (apiResponse == null) 
        {
            Debug.Log($"WebAuthService : 유저 정보 파싱 중에 오류 발생 , Json으로 변환 불가 \n {responseText}");
            return;
        }

        if (apiResponse.success == false) 
        {
            Debug.Log($"WebAuthService : 유저 로그인 실패 \n {responseText}");
            return;
        }

        // 기존 유저이면 
        LoginResponseDTO loginResponse = apiResponse.data;

        if (loginResponse == null)
        {
            Debug.Log("ApiResponse클래스의 data 타입 : LoginResponseDTO로 직렬화 하는데 실패했습니다");
            return;
        }

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

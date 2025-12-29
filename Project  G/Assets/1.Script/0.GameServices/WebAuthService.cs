using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

#region DTO
public class LoginRequestDTO
{
    public string SteamID;
    public string NickName;
    public string Country;
}

public class LoginResponseDTO<T>
{
    public bool Success;
    public bool IsNewUser;
    public T Data;
}

public class UserMapTypeByScoreDTO
{
    public int MapType;
    public float Score;
}
#endregion

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

        Pasing(responseText);
    }

    private void Pasing(string json) 
    {
        LoginResponseDTO<List<UserMapTypeByScoreDTO>> loginResponseDTO
            = JsonUtility.FromJson<LoginResponseDTO<List<UserMapTypeByScoreDTO>>>(json);

        if (loginResponseDTO == null) 
        {
            Debug.Log($"WebAuthService : 유저 정보 파싱 중에 오류 발생 , Json으로 변환 불가 \n {json}");
            return;
        }

        if (loginResponseDTO.Success == false) 
        {
            Debug.Log($"WebAuthService : 유저 로그인 실패 \n {json}");
            return;
        }

        // 기존 유저이면 
        if (!loginResponseDTO.IsNewUser) 
        {
            Debug.Log("WebAuthService : 기존 유저 로그인 성공 ");
            // UserDataManager에 값 넣어주기
            List<UserMapTypeByScoreDTO> typebyscore = loginResponseDTO.Data;
            for(int i = 0; i < typebyscore.Count; i++) 
            {
                MapType type = (MapType)typebyscore[i].MapType;
                float score = typebyscore[i].Score;

                UserDataManager.Instance.SetScoreByMapType(type, score);
            }
            return;
        }

        // 새로운 유저이면
        Debug.Log("WebAuthService : 새로운 유저 로그인 성공 ");
    }
}

using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#region DTOs

public class RequestUserRankDTO
{
    public string SteamId;
    public int MapType;
}

public class RequestRankerDTO
{
    public int MapType;
}

[Serializable]
public class UserRankDTO
{
    public string player1_nick;
    public string player2_nick;
    public int mapType;
    public float score;
    public int stage;
    public DateTime createdAt;
    public int ranking;
}
#endregion

public class WebRankingService : IRankingService
{
    private string baseUrl;
    private static string userRankUrl = "Rank/GetRank";
    private static string rankerUrl = "Rank/GetRankers";

    private IRankingModel rankingModel;

    public WebRankingService(string url, IRankingModel rankingModel)
    {
        this.baseUrl = url;
        this.rankingModel = rankingModel;
    }

    // 내 랭킹 보기
    public IEnumerator GetMyRankingService(long myId, int mapType)
    {
        RequestUserRankDTO requestDTO = new RequestUserRankDTO()
        {
            SteamId = myId.ToString(),
            MapType = mapType
        };

        string requestJson = JsonUtility.ToJson(requestDTO);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestJson);

        var request = new UnityWebRequest(baseUrl + userRankUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);

        yield return CoroutineHandler.Instance.Run(
            StartRequest(request,
            (string responseText) =>
                {
                    UserInfoPasing(responseText);
                },
            (string errorText) =>
                {
                    Debug.Log($"WebRanking : 유저 랭킹 정보 APi 오류 ID: {myId} , MapType: {mapType} / 오류코드: {errorText}");
                }
        ));
    }

    // 랭커들 보기
    public IEnumerator GetRankerService(int mapType)
    {
        RequestRankerDTO requestDTO = new RequestRankerDTO()
        {
            MapType = mapType
        };

        string requestJson = JsonUtility.ToJson(requestDTO);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestJson);

        var request = new UnityWebRequest(baseUrl + rankerUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);

        yield return CoroutineHandler.Instance.Run(
            StartRequest(request,
            (string responseText) =>
            {
                RankersPasing(responseText);
            },
            (string errorText) =>
            {
                Debug.Log($"WebRanking : 랭커 정보 APi 오류 MapType: {mapType} / 오류코드: {errorText}");
            }
        ));
    }

    IEnumerator StartRequest(UnityWebRequest request, Action<string> success, Action<string> failed)
    {
        // 공통 요청 세팅
        request.SetRequestHeader("Content-Type", "application/json");
        request.downloadHandler = new DownloadHandlerBuffer(); // 응답 바디 확인용

        // 요청보내기 (비동기)
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            failed?.Invoke(request.error);
            yield break;
        }

        // 받은 요청을 string 타입으로
        string responseText = request.downloadHandler.text;

        // API의 응답은 API Response 타입의 Json임 . 
        ApiResponse<object> apiResponse = JsonConvert.DeserializeObject<ApiResponse<object>>(responseText);
        if (apiResponse == null)
        {
            failed?.Invoke($"랭킹 API 오류 발생 , Json으로 변환 불가 \n {responseText}");
            yield break;
        }
        if (apiResponse.success == false)
        {
            failed?.Invoke($"랭킹 API 오류 발생 , 실패 \n {responseText}");
            yield break;
        }

        // 성공하면 success Action 실행 
        success?.Invoke(responseText);

    }

    private void UserInfoPasing(string json) 
    {
        ApiResponse<UserRankDTO> obj = JsonConvert.DeserializeObject<ApiResponse<UserRankDTO>>(json);

        if (obj == null)
        {
            Debug.LogError($"ApiResponse 파싱 실패 : {nameof(WebRankingService)}");
            return;
        }

        rankingModel.SetUserRanker(obj.data);
    }

    private void RankersPasing(string json) 
    {
        ApiResponse<List<UserRankDTO>> obj = JsonConvert.DeserializeObject<ApiResponse<List<UserRankDTO>>>(json);

        if (obj == null)
        {
            Debug.LogError($"ApiResponse 파싱 실패 : {nameof(WebRankingService)}");
            return;
        }

        rankingModel.SetRankers(obj.data);
    }
}

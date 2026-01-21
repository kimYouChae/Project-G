using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEditor.LightingExplorerTableColumn;

#region DTOs

public class RequestUserRankDTO
{
    public string SteamId { get; set; }
    public int MapType { get; set; }
}

public class RequestRankerDTO
{
    public int MapType { get; set; }
}

[Serializable]
public class UserRankDTO
{
    public string myNickName;
    public string otherNickName;
    public float score;
    public int rank;
}
#endregion

public class WebRankingService : IRankingService
{
    private string baseUrl;
    private static string userRankUrl = "GetRank";
    private static string rankerUrl = "GetRankers";

    private UserRankDTO myRankDto;
    private List<UserRankDTO> rankersDto;

    public UserRankDTO MyRankDto { get => myRankDto; }
    public List<UserRankDTO> RankersDto { get => rankersDto; }

    public WebRankingService(string url)
    {
        this.baseUrl = url;
    }

    // 내 랭킹 보기
    public IEnumerator GetMyRankingService(string myId, int mapType)
    {
        RequestUserRankDTO requestDTO = new RequestUserRankDTO()
        {
            SteamId = myId,
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

        // Chart 관련 API의 응답은 API Response 타입의 Json임 . 
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

        this.myRankDto = obj.data;
    }

    private void RankersPasing(string json) 
    {
        ApiResponse<List<UserRankDTO>> obj = JsonConvert.DeserializeObject<ApiResponse<List<UserRankDTO>>>(json);

        if (obj == null)
        {
            Debug.LogError($"ApiResponse 파싱 실패 : {nameof(WebRankingService)}");
            return;
        }

        this.rankersDto = obj.data;
    }
}

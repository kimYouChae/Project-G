using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#region DTO

public class UpdateUserRequestDTO
{
    public string MatchId;
    public long MySteamId;
    public long PartnerId;
    public int MapType;
    public float Score;
    public int Stage;
}

[Serializable]
public class BestScoreUpdateResponse
{
    public List<UserBestScoreResult> results;
}

[Serializable]
public class UserBestScoreResult
{
    public long steamId;
    public bool isUpdated;
    public float score;
    public int stage;
}
#endregion

public class WebGameDataService : IGameDataService
{
    private string gamdDataUrl = "";

    private IGameDataModel gameDataModel;

    public WebGameDataService(string url, IGameDataModel gameDataModel)
    {
        this.gamdDataUrl = url;
        this.gameDataModel = gameDataModel;
    }

    public IEnumerator UpdateGameDataService(string matchid, int mapType, long myId, long partnerId, float score, int stage)
    {
        UpdateUserRequestDTO updateUserInfoDTO = new UpdateUserRequestDTO() 
        {
            MatchId = matchid,
            MySteamId = myId,
            PartnerId = partnerId,
            MapType = (int)mapType,
            Score = score,
            Stage = stage
        };

        string requestJson = JsonUtility.ToJson(updateUserInfoDTO);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestJson);

        var request = new UnityWebRequest(gamdDataUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.SetRequestHeader("Content-Type", "application/json");
        request.downloadHandler = new DownloadHandlerBuffer(); // 응답 바디 확인용

        yield return CoroutineHandler.Instance.Run(StartRequest(request));
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

        // API의 응답은 API Response 타입의 Json임 . 
        string responseText = request.downloadHandler.text;

        // 응답은 APi Response 타입의 Json임
        ApiResponse<BestScoreUpdateResponse> apiResponse
            = JsonConvert.DeserializeObject<ApiResponse<BestScoreUpdateResponse>>(responseText);
        if (apiResponse == null)
        {
            Debug.Log($"게임Data API 오류 발생 , Json으로 변환 불가 \n {responseText}");
            yield break;
        }
        if (apiResponse.success == false)
        {
            Debug.Log($"게임Data API 오류 발생 , 실패 \n {responseText}");
            yield break;
        }

        // 성공하면 
        GameDataPasing(apiResponse.data);
    }

    private void GameDataPasing(BestScoreUpdateResponse response) 
    {
        // 모델에 값 넣기 
        gameDataModel.SetGameData(response);
    }
}

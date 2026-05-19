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

        yield return WebRequestCore.CommonLogic<BestScoreUpdateResponse>
        (
            requestJson,
            gamdDataUrl,
            HttpRequestType.Post,
            UpdateGameData,
            () => UpdateGameDataFailed()
        );
    }

    private void UpdateGameData(BestScoreUpdateResponse apiResponse) 
    {
        // 모델에 값 넣기 
        gameDataModel.SetGameData(apiResponse);
    }

    private void UpdateGameDataFailed() 
    {
        
    }
}

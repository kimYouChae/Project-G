using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#region DTOs

public class RequestUserRankDTO
{
    public long SteamId;
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
    private string userRankUrl = "";
    private string rankersUrl = "";

    private IRankingModel rankingModel;

    public WebRankingService(string userRankUrl, string rankersUrl  ,IRankingModel rankingModel)
    {
        this.userRankUrl = userRankUrl;
        this.rankersUrl = rankersUrl;
        this.rankingModel = rankingModel;
    }

    // 내 랭킹 보기
    public IEnumerator GetMyRankingService(long myId, int mapType)
    {
        RequestUserRankDTO requestDTO = new RequestUserRankDTO()
        {
            SteamId = myId,
            MapType = mapType
        };

        string requestJson = JsonConvert.SerializeObject(requestDTO);

        yield return WebRequestCore.CommonLogic<UserRankDTO>
        (
            requestJson,
            userRankUrl,
            HttpRequestType.Post,
            MyRanksParsing,
            () => GetRankFailed(myId, mapType)
        );
    }

    // 랭커들 보기
    public IEnumerator GetRankerService(int mapType)
    {
        RequestRankerDTO requestDTO = new RequestRankerDTO()
        {
            MapType = mapType
        };

        string requestJson = JsonConvert.SerializeObject(requestDTO);

        yield return WebRequestCore.CommonLogic<List<UserRankDTO>>
        (
            requestJson,
            rankersUrl,
            HttpRequestType.Post,
            RankersPasing,
            () => GetRankersFailed(mapType)
        );
    }
    private void GetRankFailed(long id , int mapType)
    {
        Debug.Log($"[ {id} ] 에 해당하는 유저 랭킹 가져오기 실패! \n" +
            $" MapType: {(MapType)mapType}");

        rankingModel.SetUserRanker(null);
    }

    private void GetRankersFailed(int mapType) 
    {
        Debug.Log($"[ { (MapType)mapType} ] 에 해당하는 랭커 정보 가져오기 실패! ");

        // Model에 실패 여부 담아두기 
        rankingModel.SetIsSuccess(false);
    }

    private void MyRanksParsing(UserRankDTO rankResponse) 
    {
        rankingModel.SetUserRanker(rankResponse);
    }

    private void RankersPasing(List<UserRankDTO> rankResponse) 
    {
        rankingModel.SetIsSuccess(true);
        rankingModel.SetRankers(rankResponse);
    }
}

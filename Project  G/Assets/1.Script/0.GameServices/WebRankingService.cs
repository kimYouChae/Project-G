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
    private string userRankUrl = "";
    private string rankersUrl = "";

    private IRankingModel rankingModel;

    [Header("===출력용 데이터===")]
    private long printUserId;  // 출력용 유저 아이디
    private MapType printMapType;   // 출력용 맵 타입
    private MapType printRankersMapType;    // 출력용 맵 타입

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
            SteamId = myId.ToString(),
            MapType = mapType
        };

        string requestJson = JsonUtility.ToJson(requestDTO);

        // 출력용 데이터 저장
        printUserId = myId;
        printMapType = (MapType)mapType;

        yield return WebRequestCore.CommonLogic<UserRankDTO>
        (
            requestJson,
            userRankUrl,
            HttpRequestType.Post,
            MyRanksParsing,
            GetRankFailed
        );
    }

    // 랭커들 보기
    public IEnumerator GetRankerService(int mapType)
    {
        RequestRankerDTO requestDTO = new RequestRankerDTO()
        {
            MapType = mapType
        };

        string requestJson = JsonUtility.ToJson(requestDTO);

        // 출력용 데이터 저장
        printRankersMapType = (MapType)mapType; 

        yield return WebRequestCore.CommonLogic<List<UserRankDTO>>
        (
            requestJson,
            rankersUrl,
            HttpRequestType.Post,
            RankersPasing,
            GetRankersFailed
        );
    }
    private void GetRankFailed()
    {
        Debug.Log($"[ {printUserId} ] 에 해당하는 유저 랭킹 가져오기 실패! \n" +
            $" MapType: {printMapType}");
    }

    private void GetRankersFailed() 
    {
        Debug.Log($"[ {printRankersMapType }] 에 해당하는 랭커 정보 가져오기 실패! ");
    }

    private void MyRanksParsing(UserRankDTO rankResponse) 
    {
        rankingModel.SetUserRanker(rankResponse);
    }

    private void RankersPasing(List<UserRankDTO> rankResponse) 
    {
        rankingModel.SetRankers(rankResponse);
    }
}

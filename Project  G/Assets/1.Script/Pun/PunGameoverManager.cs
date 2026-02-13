using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class MatchContext 
{
    // 게임 업데이트 API에 필요한 데이터 
    public string synchedGameIDGuid;
    public MapType synchedGameMapType;
    public float synchedScore;
    public int synchedStage;
}

public class PunGameoverManager : Singleton<PunGameoverManager>
{
    [SerializeField]
    private MatchContext matchContext;

    public string SynchedGameIDGuid { set => matchContext.synchedGameIDGuid = value; }
    public MapType SynchedGameMapType {  set => matchContext.synchedGameMapType = value; }
    public float SynchedScore {  set => matchContext.synchedScore = value; }
    public int SynchedStage {  set => matchContext.synchedStage = value; }

    protected override void Singleton_Awake()
    {
        matchContext = new MatchContext();
    }

    public void GameOver() 
    {
        // 1. 점수 / 시간 동기화
        StageScoreRaiseEvent();

        // 2. API등 실행 
        AssembleInfo();
    }

    private void AssembleInfo() 
    {
        // API 호출 전 필요한 정보를 한곳에 모음

        // API 실행 
    }


    private void ProcessData()
    {
        float insertScore, insertStage;
        float preScore, preStage;

        // 현재 데이터 가져오기 
        MapType mapType = matchContext.synchedGameMapType;

        // 이전 점수, 스테이지 
        preScore = UserDataManager.Instance.ReturUserScore(mapType);
        preStage = UserDataManager.Instance.ReturnUserStage(mapType);

    }

    private void StageScoreRaiseEvent() 
    {
        Debug.Log("[ScoreStageSync] 점수, 스테이지 Raise이벤트");

        // 점수, 스테이지
        object[] contcnt = new object[]
        {
            ScoreManager.Instance.AchiveScore,
            ScoreManager.Instance.AchiveStage,
        };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOption = new SendOptions { Reliability = true };

        bool success = PhotonNetwork.RaiseEvent((byte)PunEventType.UserDataSync,
            contcnt,
            raiseEventOptions,
            sendOption);

        Debug.Log($"[ScoreStageSync] RaiseEvent 보냄? {success}");
    }
}

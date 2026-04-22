using ExitGames.Client.Photon;
using Newtonsoft.Json;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

[System.Serializable]
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
        StartCoroutine(AssembleInfo());
    }

    IEnumerator SettingMatchConx() 
    {
        // 사실 이 부분은 없어도됨, Assemble할 때 ScoreManger 바로 접근해도됨
        // 근데 Context만든김에 값이 ctx 안에 있었으면 좋겠음 
        yield return null;
        matchContext.synchedScore = ScoreManager.Instance.AchiveScore;
        matchContext.synchedStage = ScoreManager.Instance.AchiveStage;
    }

    IEnumerator AssembleInfo()
    {
        long nick1, nick2;

        var ingamePlayer = PunIngameManager.Instance.IngamePlayerList;
        nick1 = ingamePlayer[0].SteamID;
        nick2 = ingamePlayer[1].SteamID;

        yield return StartCoroutine(SettingMatchConx());

        // 게임 데이터 저장 API 실행 
        yield return GameServices.Instance.GameDataService.UpdateGameDataService
            (
                matchContext.synchedGameIDGuid,
                (int)matchContext.synchedGameMapType,
                nick1, nick2,
                matchContext.synchedScore,
                matchContext.synchedStage
            );

        // 게임 Data API 실행 후 저장된 Model을 동기화
        GameDataRaiseEvent();
    }

    private void GameDataRaiseEvent() 
    {
        Debug.Log("[BestScoreSync] 점수, 스테이지 Raise이벤트");

        // Game Data Model
        BestScoreUpdateResponse model = GameServices.Instance.GameDataModel.GetBestScoreInfo();
        string json = JsonConvert.SerializeObject(model);

        object[] contcnt = new object[]
        {
            json,
            ScoreManager.Instance.CurrTime,
            (int)matchContext.synchedGameMapType
        };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOption = new SendOptions { Reliability = true };

        bool success = PhotonNetwork.RaiseEvent((byte)PunEventType.BestScoreSync,
            contcnt,
            raiseEventOptions,
            sendOption);

        Debug.Log($"[BestScoreSync] RaiseEvent 보냄? {success}");
    }

    /*
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
    */
}

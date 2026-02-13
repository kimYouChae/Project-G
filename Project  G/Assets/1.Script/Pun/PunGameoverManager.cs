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

    public void GameOver(int viewId) 
    {
        // 점수-시간 변동 x 
        ScoreManager.Instance.IsReadyToCount = false;

        // 시간 정지
        TimeManager.Stop();

        // 1. 점수 / 시간 동기화
        StageScoreRaiseEvent();

        // 2. 사망 UI+애니메이션 실행 
        PhotonView view = PhotonView.Find(viewId);
        if (view != null)
        {
            InGameUI.Instance.HighlightPlayer(view.transform);
        }

        // 3. API등 실행 
        float highlighTime = InGameUI.Instance.HighlistTime;
        StartCoroutine(WaitUntilAnimation(highlighTime));
    }

    IEnumerator WaitUntilAnimation(float highlightTime)
    {
        // (실제시간) 애니메이션 끝날 때 까지 대기 
        yield return new WaitForSecondsRealtime(highlightTime * 1.5f);

        // 근데 여기서 굳이굳이 안해도될듯 ???? 

        float gameTime = ScoreManager.Instance.CurrTime;    // 진행 시간 



        // 저장된 점수 vs 방금 점수 비교하기 
        // (동기화된)점수가 더 크면 
        /*
        if (matchContext.synchedScore > preScore)
        {
            // 게임오버 텍스트 설정
            InGameUI.Instance.SetGameOverText(matchContext.synchedScore, true, gameTime);

            // 유저 manager에 정보 업데이트
            UserDataManager.Instance.UpdateUserData(mapType, matchContext.synchedScore, matchContext.synchedStage);

            // 동기화된 유저 ID 가져오기
            List<long> idList = new List<long>();
            Dictionary<int, InGamePlayer> ingameUsers = PunIngameManager.Instance.InGamePlayerDataDic;
            foreach (var user in ingameUsers)
            {
                InGamePlayer info = user.Value;
                idList.Add(info.SteamID);
            }

            long id1 = idList[0] != 0 ? idList[0] : 0;
            long id2 = idList[1] != 0 ? idList[1] : 0;

            // api 호출 
            GameServices.Instance.GameDataService.UpdateGameDataService
                (matchContext.synchedGameIDGuid, (int)matchContext.synchedGameMapType, id1, id2,
                matchContext.synchedScore, matchContext.synchedStage);
        }
        else 
        {
            // 업데이트 안해도됨 ?? 
            // 근데 매치 API는 따로 없어서 분리할필요가있을듯 ?

        }
        */
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

using Unity.Services.Analytics;
using UnityEngine;

public static class AnalyticsManager
{
    /// <summary>
    /// 게임 시작 이벤트
    /// </summary>
    public static void SendSessionStart()
    {
        Debug.Log($"[AnalyticsManager] {AnalyticsEvent.SessionStart.ToString()} 이벤트 호출");

        AnalyticsService.Instance.RecordEvent(AnalyticsEvent.SessionStart);
    }

    /// <summary>
    /// 방 생성 이벤트
    /// </summary>
    public static void SendRoomCreate() 
    {
        Debug.Log($"[AnalyticsManager] {AnalyticsEvent.RoomCreate.ToString()} 이벤트 호출");

        AnalyticsService.Instance.RecordEvent(AnalyticsEvent.RoomCreate);
    }

    /// <summary>
    /// 방 생성 실패 이벤트
    /// </summary>
    /// <param name="failedReason">실패 사유</param>
    public static void SendRoomCreateFailed(string failedReason) 
    {
        Debug.Log($"[AnalyticsManager] {AnalyticsEvent.RoomJoinFailed.ToString()} 이벤트 호출");

        var evt = new CustomEvent(AnalyticsEvent.RoomJoinFailed)
        {
            { AnalyticsParam.Reason, failedReason }
        };

        AnalyticsService.Instance.RecordEvent(evt);
    }

    /// <summary>
    /// 게임 시작 이벤트
    /// </summary>
    /// <param name="sesstionIdx">이번 세션에서 몇 번째 플레이인지 </param>
    public static void SendGameStart(int sesstionIdx) 
    {
        Debug.Log($"[AnalyticsManager] {AnalyticsEvent.GameStart.ToString()} 이벤트 호출");

        var evt = new CustomEvent(AnalyticsEvent.GameStart)
        {
            { AnalyticsParam.SessionRoundIndex, sesstionIdx }
        };

        AnalyticsService.Instance.RecordEvent(evt);
    }


    public static void SendGameEnd
        (bool isComplted, float score, int stage, string mapType, bool isbestScore, float playTime) 
    {
        Debug.Log($"[AnalyticsManager] {AnalyticsEvent.GameEnd.ToString()} 이벤트 호출");

        var evt = new CustomEvent(AnalyticsEvent.GameEnd)
        {
            { AnalyticsParam.IsCompleted, isComplted },
            { AnalyticsParam.Score , score },
            { AnalyticsParam.Stage , stage },
            { AnalyticsParam.MapType , mapType },
            { AnalyticsParam.IsBestScore , isbestScore } ,
            { AnalyticsParam.PlayTimeSec , playTime}
        };

        AnalyticsService.Instance.RecordEvent(evt);
    }
}

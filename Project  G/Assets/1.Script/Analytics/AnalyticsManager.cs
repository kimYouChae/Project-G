using Unity.Services.Analytics;

public static class AnalyticsManager
{
    /// <summary>
    /// 게임 시작 이벤트
    /// </summary>
    public static void SendSessionStart()
    {
        AnalyticsService.Instance.RecordEvent(AnalyticsEvent.SessionStart);
    }

    /// <summary>
    /// 방 생성 이벤트
    /// </summary>
    public static void SendRoomCreate() 
    {
        AnalyticsService.Instance.RecordEvent(AnalyticsEvent.RoomCreate);
    }

    /// <summary>
    /// 방 생성 실패 이벤트
    /// </summary>
    /// <param name="failedReason">실패 사유</param>
    public static void SendRoomCreateFailed(string failedReason) 
    {
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
        var evt = new CustomEvent(AnalyticsEvent.GameStart)
        {
            { AnalyticsParam.SessionRoundIndex, sesstionIdx }
        };

        AnalyticsService.Instance.RecordEvent(evt);
    }


    public static void SendGameEnd
        (bool isComplted, float score, int stage, string mapType, bool isbestScore, float playTime) 
    {
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

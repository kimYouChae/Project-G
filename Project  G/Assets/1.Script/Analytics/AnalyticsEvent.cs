using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Analytics 이벤트 이름 
public static class AnalyticsEvent
{
    public const string SessionStart = "session_start";
    public const string RoomCreate = "room_create";
    public const string RoomJoinFailed = "room_join_failed";
    public const string GameStart = "game_start";
    public const string GameEnd = "game_end";
}

// Analytics 파라미터 이름 
public static class AnalyticsParam
{
    // game Start 파라미터 
    public const string SessionRoundIndex = "session_round_index";

    // room Join failed 파라미터 
    public const string Reason = "reason";

    // game end 파라미터 
    public const string Score = "score";
    public const string Stage = "stage";
    public const string MapType = "map_type";
    public const string IsBestScore = "is_best_score";
    public const string IsCompleted = "is_completed";
    public const string PlayTimeSec = "play_time_sec";
}
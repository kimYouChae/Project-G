using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataManager : Singleton<UserDataManager>
{
    // 로컬인지 체크 
    public static bool IsOfflineMode;

    [SerializeField] 
    private UserData userdata = new UserData();
    [SerializeField]
    private Texture2D userProfile;
    [SerializeField]
    private int game_round_index;    // 현재 세션에서 몇번째 게임인지

    public long SteamID { get => userdata.SteamID; }
    public string NickName { get => userdata.NickName; }
    public CharacterType CharacterType { get => userdata.CharacterType; set { userdata.CharacterType = value; } }

    protected override void Singleton_Awake()
    {
        SetUpDontDestroy();

        // 게임씬 시작 시 -> 이벤트 실행 
        game_round_index = 0;
        PhotonSceneManager.Instance.RegisterAction(SceneType.Game_Forest, UpdateSessionRoundIndex);
    }

    private void UpdateSessionRoundIndex() 
    {
        game_round_index++;

        // 게임시작 이벤트 실행
        AnalyticsManager.SendGameStart(game_round_index);
    }

    public void UpdateUserProfileImage(Texture2D texure) 
    {
        this.userProfile = texure;
    }

    public void InsertUserInfo(long id, string name, string cnt) 
    {
        userdata.SteamID = id;
        userdata.NickName = name;
        userdata.Country = cnt;
    }

    public void UpdateUserData(MapType maptype, float score, int stage) 
    {
        // 점수,스테이지 업데이트 
        userdata.ScoreByMaptype[maptype] = score;
        userdata.StageByMaptype[maptype] = stage;
    }

    public void SetScoreByMapType(MapType type, float bestScore, int bestStage)
    {
        // 최고점수 저장 
        if (userdata.ScoreByMaptype.ContainsKey(type))
            userdata.ScoreByMaptype[type] = bestScore;
        else 
            Debug.Log($"[UserData] 타입별 최고점수 딕셔너리에 값을 넣지 못함 / {type} : {bestStage}");

        // 최고스테이지 저장
        if (userdata.StageByMaptype.ContainsKey(type)) 
            userdata.StageByMaptype[type] = bestStage;
        else
            Debug.Log($"[UserData] 타입별 최고 스테이지 딕셔너리에 값을 넣지 못함 {type} : {bestStage}");

    }

    public int ReturnUserStage(MapType type)
    {
        if (userdata.StageByMaptype.ContainsKey(type))
        {
            return userdata.StageByMaptype[type];
        }

        return -1;
    }

    public float ReturUserScore(MapType type)
    {
        if (userdata.ScoreByMaptype.ContainsKey(type))
        {
            return userdata.ScoreByMaptype[type];
        }

        return -1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Define
{
    [Header("테이블명")]
    public static readonly string USERTABLE = "USER_DATA";

    public static readonly Dictionary<MapType, string> MAPTYPEBY_LEADERBOARD_TABLE = new Dictionary<MapType, string>() 
    {
        { MapType.Forest , "Leaderboard_map_forest"},
        { MapType.GiganticTree , " "},
        { MapType.Market, " "},
        { MapType.Island , " "},
        { MapType.Hell , " "},
        { MapType.IceVillage , " "}
    };

    [Header("폴더명")]
    public static string DEFAULT_SPAWNER = "Spawner/";   // 스포너 상위 폴더 이름

    [Header("씬 명")]
    public static readonly Dictionary<SceneType, string> sceneNames = new Dictionary<SceneType, string>()
    {
        { SceneType.Lobby, "01.LobbyScene"},
        { SceneType.Game_Forest , "02.Forest"},
        { SceneType.Game_GiganticTree , "02.GiganticTree"},
        { SceneType.Game_Island , "02.Island"},
        { SceneType.Game_Market , "02.Market"},
        { SceneType.Game_Hell , "02.Hell"},
        { SceneType.Game_IceVillage , "02.IceVillage"},
    };

    [Header("로컬라이징")]
    public static readonly Dictionary<LanguageType, string> languageNames = new Dictionary<LanguageType, string>()
    {
        { LanguageType.Korean, "한국어" },
        { LanguageType.English, "English" },
        { LanguageType.Japanese, "日本語" },
        { LanguageType.Chinese, "中國語" }
    };

    public static float mapMinX = -13f;
    public static float mapMinY = -8f;
    public static float mapMaxX = 13;
    public static float mapMaxY = 8;

    public static readonly Dictionary<QuadrantType, Vector2> twoMemberPoint = new Dictionary<QuadrantType, Vector2>()
    {
        { QuadrantType.one , new Vector2(5.5f , 0) },
        { QuadrantType.two , new Vector2(-5.5f , 0 ) }
    };

    public static readonly Dictionary<QuadrantType, Vector2> twoMemberFieldMin = new Dictionary<QuadrantType, Vector2>()
    {
        { QuadrantType.one , new Vector2(2,-4)},
        { QuadrantType.two , new Vector2(-9,-4)}
    };

    public static readonly Dictionary<QuadrantType, Vector2> twoMemberFieldMax = new Dictionary<QuadrantType, Vector2>()
    {
        {QuadrantType.one , new Vector2(9,4)}, 
        { QuadrantType.two , new Vector2(-2,4)}
    };

    public static readonly Dictionary<QuadrantType, Vector2> fourMemberPoint = new Dictionary<QuadrantType, Vector2>()
    {
        { QuadrantType.one , new Vector2(10f,5f) },
        { QuadrantType.two , new Vector2(5f,10f) },
        { QuadrantType.three , new Vector2(10f,5f) },
        { QuadrantType.four , new Vector2(5f,10f) }
    };

    public static readonly Dictionary<DirType, Vector2> twoMemberSpawnerPoint = new Dictionary<DirType, Vector2>()
    {
        { DirType.Left, new Vector2(-0.57f,0)},
        {DirType.Top , new Vector2(0,0.55f) },
        {DirType.Right ,new Vector2(0.57f,0)},
        {DirType.Bottom, new Vector2(0, -0.55f ) }
    };


}

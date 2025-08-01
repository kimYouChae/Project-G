using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Define 
{
    public static readonly string USERTABLE = "USER_DATA";

    public static readonly string[] MapName = new string[6]
    {
        "숲", "거대한 나무", "시장", "섬", "지옥", "얼음마을"
    };

    
    public static readonly Dictionary<SceneType, string> SceneNames = new Dictionary<SceneType, string>()
    {
        { SceneType.Lobby, "01.LobbyScene"},
        { SceneType.Game_Forest , "02.Forest"},
        { SceneType.Game_GiganticTree , "02.GiganticTree"},
        { SceneType.Game_Island , "02.Island"},
        { SceneType.Game_Market , "02.Market"},
        { SceneType.Game_Hell , "02.Hell"},
        { SceneType.Game_IceVillage , "02.IceVillage"},
    };

    public static readonly Dictionary<QuadrantType, Vector2> twoMemberPoint = new Dictionary<QuadrantType, Vector2>()
    {
        { QuadrantType.one , new Vector2(5.5f , 0) },
        { QuadrantType.two , new Vector2(-5.5f , 0 ) }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData
{
    private long steamId;
    private string nickName;
    private string country;
    private CharacterType characterType;

    // 맵 타입별 최고 점수
    private Dictionary<MapType, float> scoreByMaptype 
        = new Dictionary<MapType, float>();

    // 맵 타입별 달성 스테이지 
    private Dictionary<MapType, int> stageByMaptype
        = new Dictionary<MapType, int>();

    public long SteamID { get => steamId; set { steamId = value; } }
    public string NickName { get => nickName; set { nickName = value; } }
    public string Country { get => country; set { country = value; } }
    public CharacterType CharacterType { get => characterType; set { characterType = value; } }

    public Dictionary<MapType, float> ScoreByMaptype { get => scoreByMaptype; set => scoreByMaptype = value; }
    public Dictionary<MapType, int> StageByMaptype { get => stageByMaptype; set => stageByMaptype = value; }

    public UserData() 
    {
        // 맵 타입별 최고 점수
        scoreByMaptype = new Dictionary<MapType, float>()
        {
            { MapType.Forest, 0 },
            { MapType.GiganticTree, 0 },
            { MapType.Market, 0 },
            { MapType.Island, 0 },
            { MapType.Hell, 0 },
            { MapType.IceVillage, 0 },
        };

        // 맵 타입별 달성 스테이지 
        stageByMaptype = new Dictionary<MapType, int>()
        {
            { MapType.Forest, 0 },
            { MapType.GiganticTree, 0 },
            { MapType.Market, 0 },
            { MapType.Island, 0 },
            { MapType.Hell, 0 },
            { MapType.IceVillage, 0 },
        };
    }
}

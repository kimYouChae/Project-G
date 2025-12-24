using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    public static UserData Instance { get; private set; }

    private string steamId;
    private string nickName;
    private string country;
    private CharacterType characterType;

    // 맵 타입별 최고 점수
    private Dictionary<MapType, float> scoreByMaptype = new Dictionary<MapType, float>()
    {
        { MapType.Forest, 0 },
        { MapType.GiganticTree, 0 },
        { MapType.Market, 0 },
        { MapType.Island, 0 },
        { MapType.Hell, 0 },
        { MapType.IceVillage, 0 },
    };

    // 맵 타입별 달성 스테이지 
    private Dictionary<MapType, int> stageByMaptype = new Dictionary<MapType, int>()
    {
        { MapType.Forest, 0 },
        { MapType.GiganticTree, 0 },
        { MapType.Market, 0 },
        { MapType.Island, 0 },
        { MapType.Hell, 0 },
        { MapType.IceVillage, 0 },
    };

    public string SteamID { get => steamId; private set { } }
    public string NickName { get => nickName; private set { } }
    public string Country { get => country; private set { } }
    public CharacterType CharacterType { get => characterType; set { characterType = value; } }

    public UserData() 
    {
        Instance = new UserData();
    }

    public UserData(string id, string name, string ctr)
    {
        this.steamId = id;
        this.nickName = name;
        this.country = ctr;
    }

    public int ReturnUserStage(MapType type) 
    { 
        if(stageByMaptype.ContainsKey(type)) 
        {
            return stageByMaptype[type];
        }

        return -1;
    }

    public float ReturUserScore(MapType type) 
    {
        if (scoreByMaptype.ContainsKey(type))
        {
            return scoreByMaptype[type];
        }

        return -1;
    }
}

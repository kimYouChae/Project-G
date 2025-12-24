using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    public static UserData instance { get; private set; }

    private string steamId;
    private string nickName;
    private string country;

    // 맵 타입별 최고 점수
    private Dictionary<MapType, float> mapTypeByScore = new Dictionary<MapType, float>()
    {
        { MapType.Forest, 0 },
        { MapType.GiganticTree, 0 },
        { MapType.Market, 0 },
        { MapType.Island, 0 },
        { MapType.Hell, 0 },
        { MapType.IceVillage, 0 },
    };

    // 맵 타입별 달성 스테이지 
    private Dictionary<MapType, int> mapTypeByStage = new Dictionary<MapType, int>()
    {
        { MapType.Forest, 0 },
        { MapType.GiganticTree, 0 },
        { MapType.Market, 0 },
        { MapType.Island, 0 },
        { MapType.Hell, 0 },
        { MapType.IceVillage, 0 },
    };

    public UserData() 
    {
        instance = new UserData();
    }

    public int ReturnUserStage(MapType type) 
    { 
        if(mapTypeByStage.ContainsKey(type)) 
        {
            return mapTypeByStage[type];
        }

        return -1;
    }

}

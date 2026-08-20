using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPatternManager : Singleton<MapPatternManager>
{
    [SerializeField]
    private Dictionary<MapType, IMapPattern> keyValuePairs;

    //
    private MarketMapPattern marketMapPattern;

    public MarketMapPattern MarketMapPattern { get => marketMapPattern; }

    protected override void Singleton_Awake()
    {
        keyValuePairs = new Dictionary<MapType, IMapPattern>()
        {
            { MapType.Market , GetComponent<MarketMapPattern>() }
            // { MapType.Island , GetComponent<>() }
            // { MapType.Hell , GetComponent<>() }
            // { MapType.IceVillage , GetComponent<>() }

        };

        marketMapPattern = GetComponent<MarketMapPattern>();
    }

    public IMapPattern GetMapPattern(MapType mapType) 
    {
        if(keyValuePairs.ContainsKey(mapType))
            return keyValuePairs[mapType];

        return null;    
    }

}

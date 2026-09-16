using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPatternManager : Singleton<MapPatternManager>
{
    [SerializeField]
    private IMapPattern currentMapPattern;

    public IMapPattern CurrentMapPattern { get => currentMapPattern; }

    protected override void Singleton_Awake()
    {
        TryGetComponent<IMapPattern>(out currentMapPattern);
    }

    public void StartMapPatternByType(MapType mapType) 
    {
        // 맵 패턴이 NULL이면 
        if(currentMapPattern == null) return;
        // 맵 타입이 다르면 
        if (mapType != currentMapPattern.IGetMapType()) return;

        // 같으면 실행
        currentMapPattern.IMapPatternEnter();
    }

}

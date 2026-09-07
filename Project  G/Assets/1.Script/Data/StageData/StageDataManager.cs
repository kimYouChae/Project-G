using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageData 
{
    [SerializeField]
    [JsonProperty("quadrantType")]
    private QuadrantType quadrantType;
    [SerializeField]
    [JsonProperty("stage")]
    private int stage;
    [SerializeField]
    [JsonProperty("spawnerType")]
    private SpawnerType spawnerType;
    [SerializeField]
    [JsonProperty("dirType")]
    private DirType dirType;

    #region 멤버가 있는 생성자
    public StageData(QuadrantType quadrantType, int stage, SpawnerType spawnerType, DirType dirType)
    {
        this.quadrantType = quadrantType;
        this.stage = stage;
        this.spawnerType = spawnerType;
        this.dirType = dirType;
    }
    #endregion

    public QuadrantType QuadrantType { get => quadrantType; }
    public int Stage { get => stage;  }
    public SpawnerType SpawnerType { get => spawnerType;}
    public DirType DirType { get => dirType;  }
}

public class StageDataManager : Singleton<StageDataManager>
{
    private Dictionary<MapType, List<StageData>> quOneMapTypeByData; // 맵 타입별 (플레이어)1사분면 정보
    private Dictionary<MapType, List<StageData>> quTwoMapTypeByData; // 맵 타입별 (플레이어)2사분면 정보

#if UNITY_EDITOR
    // 인스펙터에서 확인용
    [SerializeField] private List<StageData> forestStageData;
    [SerializeField] private List<StageData> giganticTreeData;
#endif

    protected override void Singleton_Awake()
    {
        quOneMapTypeByData = new Dictionary<MapType, List<StageData>>();
        quTwoMapTypeByData = new Dictionary<MapType, List<StageData>>();
    }

    public void AddToData(StageData data, MapType type) 
    {
        if (data.QuadrantType == QuadrantType.one)
        {
            if (!quOneMapTypeByData.ContainsKey(type))
                quOneMapTypeByData.Add(type, new List<StageData>());

            quOneMapTypeByData[type].Add(data);
        }
        else if(data.QuadrantType == QuadrantType.two) 
        {
            if (!quTwoMapTypeByData.ContainsKey(type))
                quTwoMapTypeByData.Add(type, new List<StageData>());

            quTwoMapTypeByData[type].Add(data);
        }

#if UNITY_EDITOR
        if (type == MapType.Forest)
            forestStageData.Add(data);
        else if(type == MapType.GiganticTree)
            giganticTreeData.Add(data);
#endif
    }

    public StageData StageData(MapType mapType, QuadrantType quType, int stage)
    {
        Dictionary<MapType, List<StageData>> dict =
            (quType == QuadrantType.one) ? quOneMapTypeByData
            : (quType == QuadrantType.two) ? quTwoMapTypeByData
            : null;

        if (dict == null)
        {
            Debug.Log($"[StageDataManager] 존재하지 않는 사분면 타입 {quType}");
            return null;
        }

        if (!dict.ContainsKey(mapType))
        {
            Debug.Log($"[StageDataManager] {mapType}에 해당하는 Stage 데이터가 없음");
            return null;
        }

        if (stage > dict[mapType].Count || stage <= 0) 
        {
            Debug.Log($"[StageDataManager] 현재 스테이지 {stage}에 대한 정보가 없습니다");
            return null;
        }

        // stage 정보는 항상 1부터 시작함 ( 1스테이지 -> 리스트[0]번째 데이터 리턴 )
        return dict[mapType][stage - 1];
    }

    public int StageDataMaxLength(MapType type) 
    {
        // 한쪽이라도 데이터 없으면 -> 데이터 없음으로 판단 
        if( !quOneMapTypeByData.ContainsKey(type)
            || !quTwoMapTypeByData.ContainsKey(type))
            return 0;

        // ##TODO : 사분면 별 스테이지 데이터의 갯수(행)이 다른 경우는 고려 안됨 
        // 그럴일이 없는데 만약 다르면 max로 리턴
        return Math.Max(quOneMapTypeByData[type].Count ,
            quTwoMapTypeByData[type].Count);
    }

    public bool HasMapData(MapType type) 
    {
        // 맵 타입에 따른 데이터 여부
        // 데이터가 있으면 true, 없으면 false
        return StageDataMaxLength(type) > 0;
    }

}

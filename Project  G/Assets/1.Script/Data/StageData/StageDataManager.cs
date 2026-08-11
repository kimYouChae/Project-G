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
    [SerializeField] private List<StageData> quOneList; // (플레이어)1사분면 정보
    [SerializeField] private List<StageData> quTwoList; // (플레이어)2사분면 정보
    // 현재 스테이지 최대 번호
    [SerializeField] private int stageDataMaxLength = -1;

    // mapType별 StageData
    private Dictionary<MapType, StageData> mapTypeByData;

    public int StageDataMaxLength { get => stageDataMaxLength; }

    protected override void Singleton_Awake()
    {
        mapTypeByData = new Dictionary<MapType, StageData>();

        quOneList = new List<StageData>();
        quTwoList = new List<StageData>();
    }

    public void AddToData(StageData data, MapType type) 
    {
        // 딕셔너리에 추가 
        if ( !mapTypeByData.ContainsKey(type)) 
        {
            mapTypeByData.Add(type, data);
        }

        if(data.QuadrantType == QuadrantType.one)
            quOneList.Add(data);
        else if(data.QuadrantType== QuadrantType.two)
            quTwoList.Add(data);

        stageDataMaxLength = Math.Max(quOneList.Count, stageDataMaxLength);
    }

    public StageData StageData(QuadrantType type, int stage) 
    {
        if (stage - 1 < 0)
        {
            Debug.Log("인덱스가 0 미만"); 
            return null;
        }

        if (type == QuadrantType.one)
            return quOneList[stage - 1];
        else if (type == QuadrantType.two)
            return quTwoList[stage-1];

        return null;
    }

}

using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StageData 
{
    [JsonProperty("auadrantType")]
    private QuadrantType quadrantType;
    [JsonProperty("stage")]
    private int stage;
    [JsonProperty("spawnerType")]
    private List<SpawnerType> spawnerType;
    [JsonProperty("dirType")]
    private List<DirType> dirType;

    public StageData(QuadrantType q, int s, List<SpawnerType> sType, List<DirType> dType)
    {
        this.quadrantType = q;
        this.stage = s;
        this.spawnerType = sType;
        this.dirType = dType;
    }

    public QuadrantType QuadrantType { get => quadrantType; }
    public int Stage { get => stage;  }
    public List<SpawnerType> SpawnerType { get => spawnerType;}
    public List<DirType> DirType { get => dirType;  }
}

public class StageDataManager : Singleton<StageDataManager>
{
    [SerializeField] private List<StageData> quOneList; // (플레이어)1사분면 정보
    [SerializeField] private List<StageData> quTwoList; // (플레이어)2사분면 정보
    // 현재 스테이지 최대 번호
    [SerializeField] private int stageDataMaxLength = -1;

    public int StageDataMaxLength { get => stageDataMaxLength; }

    protected override void Singleton_Awake()
    {
        quOneList = new List<StageData>();
        quTwoList = new List<StageData>();
    }

    public void AddToData(StageData d) 
    {
        if(d.QuadrantType == QuadrantType.one)
            quOneList.Add(d);
        else if(d.QuadrantType== QuadrantType.two)
            quTwoList.Add(d);

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

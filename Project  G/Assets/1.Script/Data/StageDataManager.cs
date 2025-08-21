using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageData 
{
    [SerializeField] QuadrantType quadrantType;
    [SerializeField] int stage;
    [SerializeField] List<SpawnerType> spawnerType;
    [SerializeField] List<DirType> dirType;

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
    [SerializeField] private List<StageData> data;

    protected override void Singleton_Awake()
    {
        data = new List<StageData>();
    }

    public void AddToData(StageData d) 
    {
        data.Add(d);
    }


}

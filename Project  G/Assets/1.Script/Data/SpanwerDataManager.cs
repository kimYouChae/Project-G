using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnerData 
{
    [SerializeField] SpawnerType type;
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;

    public float Speed { get => speed; }
    public float Acceleration { get => acceleration; }
    public SpawnerType Type { get => type;  }

    public SpawnerData(SpawnerType t,float s, float ac) 
    {
        this.type = t;
        this.speed = s;
        this.acceleration = ac;
    }
}

public class SpanwerDataManager : Singleton<SpanwerDataManager>
{
    private Dictionary<SpawnerType, SpawnerData> typeBySpawnerData;

    // 인스펙터 창에서 보기용
    [SerializeField] private List<SpawnerData> spawnerDataList;

    public SpawnerData spanwerData(SpawnerType type) 
    {
        if (typeBySpawnerData.ContainsKey(type)) 
        {
            return typeBySpawnerData[type];
        }
        return null;
    }

    protected override void Singleton_Awake()
    {
        SetUpDontDestroy();

        typeBySpawnerData = new Dictionary<SpawnerType, SpawnerData>();
        spawnerDataList = new List<SpawnerData>();
    }

    public void AddtoMapDictionary(SpawnerType type, SpawnerData data)
    {
        typeBySpawnerData.Add(type, data);

        // 인스펙터 창에서 보기용
        spawnerDataList.Add(data);
    }


}

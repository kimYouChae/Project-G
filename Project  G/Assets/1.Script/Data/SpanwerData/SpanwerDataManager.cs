using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnerData 
{
    [SerializeField]
    [JsonProperty("spawnerType")]
    private SpawnerType type;
    [SerializeField]
    [JsonProperty("speed")]
    private float speed;
    [SerializeField]
    [JsonProperty("accerleraion")]
    private float smoothTime;

    public float Speed { get => speed; }
    public float SmoothTime { get => smoothTime; }
    public SpawnerType SpawnerType { get => type;  }

    public SpawnerData(SpawnerType t,float s, float ac) 
    {
        this.type = t;
        this.speed = s;
        this.smoothTime = ac;
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

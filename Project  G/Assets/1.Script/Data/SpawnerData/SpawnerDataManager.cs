using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnerData 
{

    [SerializeField] [JsonProperty("spawnerType")] private SpawnerType type;
    [SerializeField] private float speed;
    [SerializeField] private float bulletSpeed;
    [SerializeField] [JsonProperty("acceleration")] private float smoothTime;
    [SerializeField] private float coolTimeMin;
    [SerializeField] private float coolTimeMax;
    [SerializeField] private float chargeTime;
    [SerializeField] private float chargeDuration;
    [SerializeField] private float bulletLifeTime;
    [SerializeField] private float bulletInterval;
    [SerializeField] private float bulletInitWait;

    #region 맴버가 있는 생성자
    public SpawnerData(SpawnerType type, float speed, float bulletSpeed, 
        float smoothTime, float coolTimeMin, float coolTimeMax, float chargeTime, float chargeDuration, float bulletLifeTime, float bulletInterval, float bulletInitWait)
    {
        this.type = type;
        this.speed = speed;
        this.bulletSpeed = bulletSpeed;
        this.smoothTime = smoothTime;
        this.coolTimeMin = coolTimeMin;
        this.coolTimeMax = coolTimeMax;
        this.chargeTime = chargeTime;
        this.chargeDuration = chargeDuration;
        this.bulletLifeTime = bulletLifeTime;
        this.bulletInterval = bulletInterval;
        this.bulletInitWait = bulletInitWait;
    }
    #endregion

    #region 프로퍼티

    public SpawnerType Type { get => type;  }
    public float Speed { get => speed; }
    public float BulletSpeed { get => bulletSpeed; }
    public float SmoothTime { get => smoothTime; }
    public float CoolTimeMin { get => coolTimeMin; }
    public float CoolTimeMax { get => coolTimeMax; }
    public float ChargeTime { get => chargeTime; }
    public float ChargeDuration { get => chargeDuration; }
    public float BulletLifeTime { get => bulletLifeTime; }
    public float BulletInterval { get => bulletInterval; }
    public float BulletInitWait { get => bulletInitWait; }
    #endregion
}

public class SpawnerDataManager : Singleton<SpawnerDataManager>
{
    private Dictionary<SpawnerType, SpawnerData> typeBySpawnerData;

    // 인스펙터 창에서 보기용
    [SerializeField] private List<SpawnerData> spawnerDataList;

    public string SerializationSpawnerData(SpawnerType type) 
    {
        if (typeBySpawnerData.ContainsKey(type))
        {
            return JsonConvert.SerializeObject(typeBySpawnerData[type]);
        }
        return string.Empty;
    }

    public SpawnerData DeserializationSpawnerData(string str) 
    {
        return JsonConvert.DeserializeObject<SpawnerData>(str);
    }

    protected override void Singleton_Awake()
    {
        SetUpDontDestroy();
    }

    public void AddtoMapDictionary(SpawnerType type, SpawnerData data)
    {
        if(typeBySpawnerData == null)
            typeBySpawnerData = new Dictionary<SpawnerType, SpawnerData>();

        if(spawnerDataList == null)
            spawnerDataList = new List<SpawnerData>();

        typeBySpawnerData.Add(type, data);

        // 인스펙터 창에서 보기용
        spawnerDataList.Add(data);
    }


}

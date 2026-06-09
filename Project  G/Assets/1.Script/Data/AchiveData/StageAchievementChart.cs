using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageAchieve
{
    [SerializeField] private string title;
    [SerializeField] private AchieveType achieveType;
    [SerializeField] private int achieveStage;
    [SerializeField] private MapType mapType;

    #region 맴버가 있는 생성자

    public StageAchieve(string title, AchieveType achieveType, int achieveStage, MapType mapType)
    {
        this.title = title;
        this.achieveType = achieveType;
        this.achieveStage = achieveStage;
        this.mapType = mapType;
    }
    #endregion

    public string Title { get => title; }
    public AchieveType AchieveType { get => achieveType; }
    public int AchieveStage { get => achieveStage;  }
    public MapType MapType { get => mapType; }
}

public class StageAchievementChart : ICharHandler
{
    public void IParseAndStore(string jsonStr)
    {
        List<StageAchieve> datalist;
        datalist = JsonConvert.DeserializeObject<List<StageAchieve>>(jsonStr);

        if (datalist == null || datalist.Count == 0)
        {
            Debug.LogWarning($"Data 리스트가 비었거나 null : {nameof(StageAchievementChart)}");
            return;
        }

        for (int i = 0; i < datalist.Count; i++)
        {
            StageAchieve data = datalist[i];
            AchievDataManager.Instance.AddtoAchieveContainer(data);
        }
    }
}

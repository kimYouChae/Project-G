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

    public string Title { get => title; set => title = value; }
    public AchieveType AchieveType { get => achieveType; set => achieveType = value; }
    public int AchieveStage { get => achieveStage; set => achieveStage = value; }
    public MapType MapType { get => mapType; set => mapType = value; }
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

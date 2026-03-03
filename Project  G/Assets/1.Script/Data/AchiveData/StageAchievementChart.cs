using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageAchive
{
    [SerializeField] private string title;
    [SerializeField] private AchiveType achiveType;
    [SerializeField] private int achiveStage;
    [SerializeField] private MapType mapType;

    public string Title { get => title; set => title = value; }
    public AchiveType AchiveType { get => achiveType; set => achiveType = value; }
    public int AchiveStage { get => achiveStage; set => achiveStage = value; }
    public MapType MapType { get => mapType; set => mapType = value; }
}

public class StageAchievementChart : ICharHandler
{
    public void IParseAndStore(string jsonStr)
    {
        ApiResponse<List<StageAchive>> obj = JsonConvert.DeserializeObject<ApiResponse<List<StageAchive>>>(jsonStr);

        if (obj == null)
        {
            Debug.LogError($"ApiResponse 파싱 실패 : {nameof(StageAchievementChart)}");
            return;
        }

        List<StageAchive> datalist = obj.data;

        if (datalist == null || datalist.Count == 0)
        {
            Debug.LogWarning($"Data 리스트가 비었거나 null : {nameof(StageAchievementChart)}");
            return;
        }

        for (int i = 0; i < datalist.Count; i++)
        {
            StageAchive data = datalist[i];
            AchievDataManager.Instance.AddtoAchiveContainer(data);
        }
    }
}

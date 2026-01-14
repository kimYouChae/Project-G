using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageAchievementChart : ICharHandler
{
    public void IParseAndStore(string jsonStr)
    {
        ApiResponse<List<StageAchievement>> obj = JsonConvert.DeserializeObject<ApiResponse<List<StageAchievement>>>(jsonStr);

        if (obj == null)
        {
            Debug.LogError($"ApiResponse 파싱 실패 : {nameof(StageAchievementChart)}");
            return;
        }

        List<StageAchievement> datalist = obj.data;

        if (datalist == null || datalist.Count == 0)
        {
            Debug.LogWarning($"Data 리스트가 비었거나 null : {nameof(StageAchievementChart)}");
            return;
        }

        for (int i = 0; i < datalist.Count; i++)
        {
            StageAchievement data = datalist[i];
            AchievementsManager.Instance.AddtoAchiveContainer(data);
        }
    }
}

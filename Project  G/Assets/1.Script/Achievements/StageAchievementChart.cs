using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageAchievementChart : ICharHandler
{
    public void IParseAndStore(string jsonStr)
    {
        /*
        foreach (LitJson.JsonData row in jsonData) 
        {
            string title = row["title"].ToString();
            AchiveType achType = Extension.StringToEnum<AchiveType>(row["achiveType"].ToString());
            int cnt = int.Parse(row["achiveStage"].ToString());
            MapType mapType = Extension.StringToEnum<MapType>(row["mapType"].ToString());

            StageAchievement stageAchievement = new StageAchievement(title, achType, cnt, mapType);
            AchievementsManager.Instance.AddtoAchiveContainer(stageAchievement);

        }
        */
    }
}

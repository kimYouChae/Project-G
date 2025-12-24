using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerChart : ICharHandler
{
    public void IParseAndStore(string jsonStr)
    {
        /*
        foreach (LitJson.JsonData row in jsonData)
        {
            // row는 각 원소(오브젝트)
            SpawnerType type = Extension.StringToEnum<SpawnerType>(row["spanwerType"].ToString());
            float speed = float.Parse(row["speed"].ToString());
            float accerate = float.Parse(row["acceleration"].ToString());

            SpawnerData data = new SpawnerData(type, speed,accerate);
            SpanwerDataManager.Instance.AddtoMapDictionary(type ,data);
        }
        */
    }
}

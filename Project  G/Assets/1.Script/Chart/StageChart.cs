using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageChart : ICharHandler
{
    public void IParseAndStore(LitJson.JsonData jsonData)
    {
        foreach (LitJson.JsonData row in jsonData)
        {
            // row는 각 원소(오브젝트)
            QuadrantType quType = Extension.StringToEnum<QuadrantType>(row["quadrant"].ToString());
            int sta = int.Parse(row["stage"].ToString());

            List<SpawnerType> sTypes = new List<SpawnerType>();
            string[] spawner = (row["spawner"].ToString()).Split('-', StringSplitOptions.RemoveEmptyEntries);
            for(int i = 0; i < spawner.Length; i++) 
            {
                sTypes.Add(Extension.StringToEnum<SpawnerType>(spawner[i]));
            }

            List<DirType> dTypes = new List<DirType>();
            string[] dir = (row["dir"].ToString()).Split('-', StringSplitOptions.RemoveEmptyEntries);
            for(int i = 0; i < dir.Length; i++) 
            {
                dTypes.Add(Extension.StringToEnum<DirType>(dir[i]));
            }

            StageData sd = new StageData(quType, sta, sTypes, dTypes);
            StageDataManager.Instance.AddToData(sd);
        }
    }
}

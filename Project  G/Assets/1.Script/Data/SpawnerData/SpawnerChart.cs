using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerChart : ICharHandler
{
    public void IParseAndStore(string jsonStr)
    {
        List<SpawnerData> datalist;
        datalist = JsonConvert.DeserializeObject<List<SpawnerData>>(jsonStr);

        if (datalist == null || datalist.Count == 0)
        {
            Debug.LogWarning($"Data 리스트가 비었거나 null : {nameof(SpawnerChart)}");
            return;
        }

        for (int i = 0; i < datalist.Count; i++)
        {
            SpawnerData data = datalist[i];
            SpawnerDataManager.Instance.AddtoMapDictionary(data.Type, data);
        }

    }
}

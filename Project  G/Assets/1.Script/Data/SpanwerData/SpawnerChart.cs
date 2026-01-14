using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerChart : ICharHandler
{
    public void IParseAndStore(string jsonStr)
    {
        ApiResponse<List<SpawnerData>> obj = JsonConvert.DeserializeObject<ApiResponse<List<SpawnerData>>>(jsonStr);

        if (obj == null)
        {
            Debug.LogError($"ApiResponse 파싱 실패 : {nameof(SpawnerChart)}");
            return;
        }

        List<SpawnerData> datalist = obj.data;

        if (datalist == null || datalist.Count == 0)
        {
            Debug.LogWarning($"Data 리스트가 비었거나 null : {nameof(SpawnerChart)}");
            return;
        }

        for (int i = 0; i < datalist.Count; i++)
        {
            SpawnerData data = datalist[i];
            SpanwerDataManager.Instance.AddtoMapDictionary(data.SpawnerType, data);
        }

    }
}

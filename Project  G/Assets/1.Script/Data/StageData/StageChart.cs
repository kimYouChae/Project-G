using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageChart : ICharHandler
{
    public void IParseAndStore(string jsonStr)
    {
        ApiResponse<List<StageData>> obj = JsonConvert.DeserializeObject<ApiResponse<List<StageData>>>(jsonStr);

        if (obj == null)
        {
            Debug.LogError($"ApiResponse 파싱 실패 : {nameof(StageChart)}");
            return;
        }

        List<StageData> datalist = obj.data;

        if (datalist == null || datalist.Count == 0)
        {
            Debug.LogWarning($"Data 리스트가 비었거나 null : {nameof(StageChart)}");
            return;
        }

        for (int i = 0; i < datalist.Count; i++)
        {
            StageData data = datalist[i];
            StageDataManager.Instance.AddToData(data);
        }
    }
}

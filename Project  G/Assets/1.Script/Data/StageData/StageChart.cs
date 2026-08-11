using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Map 관련 파서는 "ChartHandlerFactory"클래스 안에 속할일이 없음
/// -> MapType기준으로 돌아가기 때문에
/// 
/// 그래서 interface 구현을 삭제함 
/// </summary>
public static class StageChart
{
    public static void ParseAndStoreMapData(string jsonStr, MapType type)
    {
        List<StageData> datalist;
        datalist = JsonConvert.DeserializeObject<List<StageData>>(jsonStr);

        if (datalist == null || datalist.Count == 0)
        {
            Debug.LogWarning($"Data 리스트가 비었거나 null : {nameof(StageChart)}");
            return;
        }

        for (int i = 0; i < datalist.Count; i++)
        {
            StageData data = datalist[i];
            StageDataManager.Instance.AddToData(data, type);
        }
    }
}

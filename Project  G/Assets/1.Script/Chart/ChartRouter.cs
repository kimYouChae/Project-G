using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChartRouter
{
    // key : 차트 ID - value : 차트에 해당하는 클래스 
    private Dictionary<string, ICharHandler> keyValuePairs;

    public ChartRouter() 
    {
        keyValuePairs = new Dictionary<string, ICharHandler>();
    }

    public void RegisterChartHanlder(string key, ICharHandler value) 
    {
        if (!keyValuePairs.ContainsKey(key)) 
        {
            keyValuePairs.Add(key, value);
        }
    }

    public void ChartHandle(string key, LitJson.JsonData data) 
    {
        if( keyValuePairs.TryGetValue(key, out ICharHandler value)) 
        {
            value.ParseAndStore(data);
        }
    }
}

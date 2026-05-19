using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;


public class MapChart : ICharHandler
{
    public void IParseAndStore(string jsonStr)
    {
        List<MapData> datalist;
        datalist = JsonConvert.DeserializeObject<List<MapData>>(jsonStr);

        if (datalist == null || datalist.Count == 0)
        {
            Debug.LogWarning($"Data 리스트가 비었거나 null : {nameof(MapChart)}");
            return;
        }

        for (int i = 0; i < datalist.Count; i++)
        {
            MapData data = datalist[i];
            MapDataManager.Instance.AddtoMapDictionary(data.Maptype, data);
        }

    }
}

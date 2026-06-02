using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfigChart : ICharHandler
{
    public void IParseAndStore(string jsonStr)
    {
        List<GameConfigData> datalist;
        datalist = JsonConvert.DeserializeObject<List<GameConfigData>>(jsonStr);

        if (datalist == null || datalist.Count == 0)
        {
            Debug.LogWarning($"Data 리스트가 비었거나 null : {nameof(GameConfigChart)}");
            return;
        }

        for (int i = 0; i < datalist.Count; i++)
        {
            GameConfig.SetGameConfigData(datalist[i]);
        }
    }
}

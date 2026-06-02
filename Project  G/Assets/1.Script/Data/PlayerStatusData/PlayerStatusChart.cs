using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusChart : ICharHandler
{
    public void IParseAndStore(string jsonStr)
    {
        List<PlayerStatusData> datalist;
        datalist = JsonConvert.DeserializeObject<List<PlayerStatusData>>(jsonStr);

        if (datalist == null || datalist.Count == 0)
        {
            Debug.LogWarning($"Data 리스트가 비었거나 null : {nameof(PlayerStatusChart)}");
            return;
        }

        for (int i = 0; i < datalist.Count; i++)
        {
            PlayerStatus.SetPlayerStatusData(datalist[i]);
        }
    }
}

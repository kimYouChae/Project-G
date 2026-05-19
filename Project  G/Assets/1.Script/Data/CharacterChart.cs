using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterChart : ICharHandler
{
    public void IParseAndStore(string jsonStr)
    {
        List<CharacterData> datalist;
        datalist = JsonConvert.DeserializeObject<List<CharacterData>>(jsonStr);

        if (datalist == null || datalist.Count == 0)
        {
            Debug.LogWarning($"Data 리스트가 비었거나 null : {nameof(CharacterChart)}");
            return;
        }

        for (int i = 0; i < datalist.Count; i++)
        {
            CharacterData data = datalist[i];
            CharacterManager.Instance.AddtoCharacterContainer(data);
        }
    }
}

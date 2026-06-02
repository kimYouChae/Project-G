using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameConfigData
{
    [SerializeField]
    [JsonProperty("key")]
    private GameConfigType gameConfigType;
    [SerializeField]
    [JsonProperty("value")]
    private float value;
    [SerializeField]
    private string description;

    public GameConfigType GameConfigType { get => gameConfigType;  }
    public float Value { get => value; }
    public string Description { get => description; }
}

public static class GameConfig
{
    private static Dictionary<GameConfigType, GameConfigData> keyValuePairs;

    public static void SetGameConfigData(GameConfigData data) 
    {
        // 딕셔너리에 타입별 config 데이터 저장 
        if(keyValuePairs == null)
            keyValuePairs= new Dictionary<GameConfigType, GameConfigData>();

        keyValuePairs.Add(data.GameConfigType, data);
    }

    public static float GetGameConfigValueByType(GameConfigType type) 
    {
        if( keyValuePairs.ContainsKey(type) )
            return keyValuePairs[type].Value;

        return -1;
    }
}

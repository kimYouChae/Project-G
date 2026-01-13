using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChartHandlerFactory
{
    // 싱글톤 
    private static ChartHandlerFactory instance;
    public static ChartHandlerFactory Instance
    {
        get
        {
            if (instance == null)
                instance = new ChartHandlerFactory();

            return instance;
        }
    }

    Dictionary<DataType, ICharHandler> keyValuePairs;

    private ChartHandlerFactory() 
    {
        // 차트가 추가되면 이 부분에 차트 이름 - 해당 클래스 생성 필요! 
        keyValuePairs = new Dictionary<DataType, ICharHandler>() 
        {
            { DataType.Map , new MapChart()},
            { DataType.Spawner , new SpawnerChart()},
            { DataType.Stage_Forest_SpawnerInfo , new StageChart()},
            { DataType.Localization_basic, new LocalizationChart()},
            { DataType.Localization_Ingame, new LocalizationChart() },
            { DataType.Localization_Player, new LocalizationChart() },
            { DataType.Character, new CharacterChart()},
            { DataType.Achievement, new StageAchievementChart()}
        };
    }

    public ICharHandler DataTypeByChart(DataType type)
    {
        if(keyValuePairs.ContainsKey(type))
            return keyValuePairs[type];

        return null;
    }
}

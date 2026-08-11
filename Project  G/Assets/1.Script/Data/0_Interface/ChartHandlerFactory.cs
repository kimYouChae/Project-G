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
        // (+) 맵별 스포너 차트는 WebChartService에서 직접 처리
        keyValuePairs = new Dictionary<DataType, ICharHandler>()
        {
            { DataType.Map , new MapChart()},
            { DataType.Spawner , new SpawnerChart()},
            { DataType.Localization_basic, new LocalizationChart()},
            { DataType.Localization_Ingame, new LocalizationChart() },
            { DataType.Localization_Player, new LocalizationChart() },
            { DataType.Localization_Temp , new LocalizationChart()},
            { DataType.Character, new CharacterChart()},
            { DataType.Achievement, new StageAchievementChart()},
            { DataType.GameConfig, new GameConfigChart()},
            { DataType.PlayerStatus, new PlayerStatusChart()}
        };
    }

    public ICharHandler DataTypeByChart(DataType type)
    {
        if(keyValuePairs.ContainsKey(type))
            return keyValuePairs[type];

        return null;
    }
}

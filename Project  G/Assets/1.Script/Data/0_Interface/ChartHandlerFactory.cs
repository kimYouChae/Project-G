using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChartHandlerFactory
{
    Dictionary<string, ICharHandler> keyValuePairs;

    public ChartHandlerFactory() 
    {
        // 차트가 추가되면 이 부분에 차트 이름 - 해당 클래스 생성 필요! 
        keyValuePairs = new Dictionary<string, ICharHandler>() 
        {
            { "Map" , new MapChart()},
            { "SpanwerData" , new SpawnerChart()},
            { "Stage_Forest" , new StageChart()},
            { "Localization_Table", new LocalizationChart()},
            { "CharacterData_Korean", new CharacterChart()},
            { "Achievement_Stage", new StageAchievementChart()}
        };
    }

    public ICharHandler ChartNameByChart(string name)
    {
        if(keyValuePairs.ContainsKey(name))
            return keyValuePairs[name];

        return null;
    }

}

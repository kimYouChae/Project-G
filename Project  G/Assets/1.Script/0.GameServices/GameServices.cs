using System;
using System.Collections;
using System.Collections.Generic;


public sealed class GameServices 
{
    // 싱글톤
    private static GameServices instance;
    public static GameServices Instance 
    {
        get 
        {
            if(instance == null)
                instance = new GameServices();
            return instance;
        }
    }
    
    public IAuthService AuthService { get; private set; }
    public IRankingService RankingService { get; private set; }
    public IGameDataService GameDataService { get; private set; }
    public IChartService ChartDataService { get; private set; }

    private static readonly string baseUrl = "http://" + "localhost/Project_G/api/";

    private GameServices() 
    {
        AuthService = new WebAuthService(baseUrl);
        RankingService = new WebRankingService(baseUrl);   
        GameDataService = new WebGameDataService(baseUrl);
        ChartDataService = new WebChartService(baseUrl);
    }

    public void ChartLogic() 
    {
        DataType[] array = (DataType[])Enum.GetValues(typeof(DataType));
        for (int i = 0; i < array.Length; i++)
        {
            ChartDataService.ChartService(array[0]);
        }

    }
}

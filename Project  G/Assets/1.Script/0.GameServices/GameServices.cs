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

    private IAuthService authService;
    private IRankingService rankingService;
    private IGameDataService gameDataService;
    private IChartService chartDataService;
    private IRankingModel rankingModel;

    public IAuthService AuthService { get => authService; }
    public IRankingService RankingService { get => rankingService; }
    public IRankingModel RankingModel { get => rankingModel; }
    public IGameDataService GameDataService { get => gameDataService; }
    public IChartService ChartDataService { get => chartDataService; }

    private static readonly string baseUrl = "http://" + "localhost/Project_G/api/";

    private GameServices() 
    {
        authService = new WebAuthService(baseUrl);
        rankingModel = new RankingModel();
        rankingService = new WebRankingService(baseUrl, rankingModel);
        gameDataService = new WebGameDataService(baseUrl);
        chartDataService = new WebChartService(baseUrl);
    }

    public IEnumerator ChartLogic() 
    {
        DataType[] array = (DataType[])Enum.GetValues(typeof(DataType));
        for (int i = 0; i < array.Length; i++)
        {
            yield return ChartDataService.ChartService(array[i]);
        }

    }
}

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

    // 서비스 인터페이스 
    private IAuthService authService;
    private IRankingService rankingService;
    private IGameDataService gameDataService;
    private IChartService chartDataService;
    
    // 모델 인터페이스
    private IRankingModel rankingModel;
    private IGameDataModel gameDataModel;

    public IAuthService AuthService { get => authService; }
    public IRankingService RankingService { get => rankingService; }
    public IRankingModel RankingModel { get => rankingModel; }
    public IGameDataService GameDataService { get => gameDataService; }
    public IGameDataModel GameDataModel { get => gameDataModel; }
    public IChartService ChartDataService { get => chartDataService; }
    

    private static readonly string baseUrl = "http://" + "localhost/Project_G/api/";

    private GameServices() 
    {
        authService = new WebAuthService(baseUrl);
        rankingModel = new RankingModel();
        rankingService = new WebRankingService(baseUrl, rankingModel);
        gameDataModel = new GamdDataModel();
        gameDataService = new WebGameDataService(baseUrl, gameDataModel);
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

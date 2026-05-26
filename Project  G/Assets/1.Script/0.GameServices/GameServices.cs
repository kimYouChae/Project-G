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
            if (instance == null)
                instance = new GameServices();
            return instance;
        }
    }

    // 서비스 인터페이스 
    private IAuthService authService;
    private IRankingService rankingService;
    private IGameDataService gameDataService;
    private IChartService chartDataService;
    private IUserProgressService userProgressService;

    // 모델 인터페이스
    private IRankingModel rankingModel;
    private IGameDataModel gameDataModel;
    private IAchiveProgressModel achiveModel;

    // Server Config
    private ServerConfig serverConfig;

    public IAuthService AuthService { get => authService; }
    public IRankingService RankingService { get => rankingService; }
    public IRankingModel RankingModel { get => rankingModel; }
    public IGameDataService GameDataService { get => gameDataService; }
    public IGameDataModel GameDataModel { get => gameDataModel; }
    public IChartService ChartDataService { get => chartDataService; }
    public IUserProgressService UserProgressService { get => userProgressService; }
    public IAchiveProgressModel AchiveProgressModel { get => achiveModel; }

    private GameServices() 
    {

    }

    public void Init(ServerConfig config) 
    {
        this.serverConfig = config;

        // 모델
        rankingModel = new RankingModel();
        gameDataModel = new GameDataModel();
        achiveModel = new AchiveProgressModel();

        // 서비스 
        authService = new WebAuthService(config.LoginUrl);
        rankingService = new WebRankingService(config.UserRankUrl, config.RankerUrl, rankingModel);
        gameDataService = new WebGameDataService( config.GameDataUrl, gameDataModel);
        chartDataService = new WebChartService(config.ChartUrl, config.ChartVersionUrl);
        userProgressService = new WebUserProgressService(config.AchiveProgressUrl, achiveModel);
    }
}

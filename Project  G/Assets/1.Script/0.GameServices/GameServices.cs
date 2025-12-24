using System.Collections;
using System.Collections.Generic;

public enum DataType
{
    None,
    Achievement,
    Character,
    Localization_basic,
    Localization_Ingame,
    Localization_Player,
    Map,
    Spawner,
    Stage_Forest_SpawnerInfo
}

public sealed class GameServices 
{
    // 싱글톤
    public static GameServices Instance { get; private set; }
    
    public IAuthService AuthService { get; private set; }
    public IRankingService RankingService { get; private set; }
    public IGameDataService GameDataService { get; private set; }
    public IChartService ChartDataService { get; private set; }

    private static readonly string baseUrl = "http://" + "localhost/Project_G/api/";

    public GameServices() 
    {
        Instance = new GameServices();

        AuthService = new WebAuthService(baseUrl);
        RankingService = new WebRankingService(baseUrl);   
        GameDataService = new WebGameDataService(baseUrl);
        ChartDataService = new WebChartService(baseUrl);
    }
}

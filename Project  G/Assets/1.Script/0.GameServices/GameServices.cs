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
    public static GameServices Instance { get; private set; }
    
    public IAuthService authService { get; private set; }
    public IRankingService rankingService { get; private set; }
    public IGameDataService gameDataService { get; private set; }
    public IChartService chartDataService { get; private set; }

    private static readonly string baseUrl = "http://" + "localhost/Project_G/api/";

    public GameServices() 
    {
        Instance = new GameServices();

        authService = new WebAuthService(baseUrl);
        rankingService = new WebRankingService(baseUrl);   
        gameDataService = new WebGameDataService(baseUrl);
        chartDataService = new WebChartService(baseUrl);
    }
}

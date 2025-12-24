using System.Collections;
using System.Collections.Generic;

public sealed class GameServices 
{
    public static GameServices Instance { get; private set; }
    
    public IAuthService authService { get; private set; }
    public IPlayerDataService playerDataService { get; private set; }
    public IRankingService rankingService { get; private set; }
    public IGameDataService gameDataService { get; private set; }
    public IChartService chartDataService { get; private set; }

    private static readonly string baseUrl = "http://" + "localhost/Project_G/api/";

    public GameServices() 
    {
        Instance = new GameServices();

        //authService = new WebAuthService();
        // playerDataService = new WebPlayerDataService();
        // rankingService = new WebRankingService();
        // gameDataService = new WebGameDataService();
    }
}

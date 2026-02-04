using System;

public class SteamAPI 
{
    // 싱글톤 
    private static SteamAPI instance;
    public static SteamAPI Instance
    {
        get 
        {
            if (instance == null)
                instance = new SteamAPI();

            return instance;
        }
    }

    // 외부에서 생성못하게
    private SteamAPI() { }

    public long GetSteamID() 
    {
        return 101;
    }

    public string GetSteamNick() 
    {
        return "Alpha";
    }

    public string GetCountry() 
    {
        return "KR";
    }
}

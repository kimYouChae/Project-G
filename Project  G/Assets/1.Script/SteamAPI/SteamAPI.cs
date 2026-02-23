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
#if UNITY_EDITOR
        return 4560000012;
#elif UNITY_STANDALONE
        return 7890000012;
#endif
    }

    public string GetSteamNick() 
    {
#if UNITY_EDITOR
        return "에디터환경의유저";
#elif UNITY_STANDALONE
        return "빌드환경의유저";
#endif
    }

    public string GetCountry() 
    {
        return "KR";
    }
}

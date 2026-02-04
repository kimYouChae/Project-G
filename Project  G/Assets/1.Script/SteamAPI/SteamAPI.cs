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

    public string GetSteamID() 
    {
        // 일단 임시로 아무 문자열 return
        var rnd = new Random();
        //string steamUid = "7656119" + rnd.Next(1, 50).ToString();
        string steamUid = "101";

        return steamUid;
    }

    public string GetSteamNick() 
    {
        return "kebby";
    }

    public string GetCountry() 
    {
        return "KR";
    }
}

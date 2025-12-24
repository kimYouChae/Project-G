using System;
using System.Collections;
using System.Collections.Generic;

public class SteamAPI 
{
    // 싱글톤 
    public static SteamAPI instance;

    public SteamAPI() 
    {
        instance = new SteamAPI();  
    }

    public string GetSteamID() 
    {
        // 일단 임시로 아무 문자열 return
        string steamUid = "7656119" + Guid.NewGuid().ToString();

        return steamUid;
    }

    public string GetSteamNick() 
    {
        return Guid.NewGuid().ToString();
    }

    public string GetCountry() 
    {
        return "KR";
    }
}

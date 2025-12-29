using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;

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

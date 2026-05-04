using System;
using UnityEngine;

public class SteamUserData 
{
    // 싱글톤 
    private static SteamUserData instance;
    public static SteamUserData Instance
    {
        get 
        {
            if (instance == null)
                instance = new SteamUserData();

            return instance;
        }
    }


    // 외부에서 생성못하게
    private SteamUserData() { }

    [Header("Field")]
    private ulong steamID;
    private string nickName;
    private string country;
    public ulong SteamID { get => steamID; set => steamID = value; }
    public string NickName { get => nickName; set => nickName = value; }
    public string Country { get => country; set => country = value; }

    public long GetSteamID() 
    {
        return (long) steamID;
#if DEV_BUILD_TEST
        return 22463504765611990;
#endif
    }

    public string GetSteamNick() 
    {
        return nickName;
#if DEV_BUILD_TEST
        return "김감자고구마";
#endif
    }

    public string GetCountry() 
    {
        return country;
#if DEV_BUILD_TEST
        return "SKY";
#endif
    }
}

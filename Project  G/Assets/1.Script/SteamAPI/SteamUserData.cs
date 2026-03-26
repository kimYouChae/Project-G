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
#if UNITY_EDITOR
        return (long) steamID;
#elif UNITY_STANDALONE
        return 7890000012;
#endif
    }

    public string GetSteamNick() 
    {
#if UNITY_EDITOR
        return nickName;
#elif UNITY_STANDALONE
        return "빌드환경의유저";
#endif
    }

    public string GetCountry() 
    {
#if UNITY_EDITOR
        return country;
#elif UNITY_STANDALONE
        return "SKY";
#endif
    }
}

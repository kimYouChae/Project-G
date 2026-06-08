using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class SteamAchievement : MonoBehaviour
{
    private void Start()
    {
        Debug.Log($"[SteamDebug] SteamManager.Initialized = {SteamManager.Initialized}");

        if (!SteamManager.Initialized)
        {
            Debug.LogError("[SteamDebug] Steam API 초기화 안됨");
            return;
        }
    }


}

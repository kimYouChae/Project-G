using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class UGSInitializer : MonoBehaviour
{
    public static bool isInitialized;

    private async void Awake()
    {
        if (isInitialized) return;

        try
        {
            await UnityServices.InitializeAsync();

            AnalyticsService.Instance.StartDataCollection(); 

            isInitialized = true;
            Debug.Log("UGS initialized");
        }
        catch (Exception e)
        {
            Debug.LogError($"UGS init failed: {e}");
        }
    }
}

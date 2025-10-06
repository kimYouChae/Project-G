using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeManager
{
    public static void Stop() 
    {
        Debug.Log("TimeManager:시간을0으로");
        Time.timeScale = 0;
    }

    public static void Play() 
    {
        Debug.Log("TimeManager:시간을1로");
        Time.timeScale = 1;
    }
}

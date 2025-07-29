using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeManager
{
    public static void Stop() 
    {
        Time.timeScale = 0;
    }
}

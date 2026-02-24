using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeManager
{
    public static void Stop() 
    {
        Debug.Log("TimeManager:시간을0으로");
        Time.timeScale = 0;

        // 원래 -1인데 이벤트 받을 수 있게 timeScale보다 큰 값으로 변경 
        PhotonNetwork.MinimalTimeScaleToDispatchInFixedUpdate = 1f;
    }

    public static void Play() 
    {
        Debug.Log("TimeManager:시간을1로");
        Time.timeScale = 1;

        // 원래대로 
        PhotonNetwork.MinimalTimeScaleToDispatchInFixedUpdate = -1f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PunConnected
{
    // Photon ConnectUsingSettings()를 이미 호출했는지 여부
    // 중복 접속 시도를 막기 위한 플래그
    public static bool hasStartedPhotonConnect = false;

    // Photon ConnectUsingSettings()를 이미 호출했는지 여부
    // 중복 접속 시도를 막기 위한 플래그
    public static bool hasHandledInitialPhotonConnect = false;
}

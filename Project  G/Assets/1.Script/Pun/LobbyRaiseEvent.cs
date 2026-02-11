using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PunEventType
{
    UserDataSync = 1,
    GameIdSync = 2,
    MapDataSync = 3,
    ScoreStageSync = 4
}

public class LobbyRaiseEvent : MonoBehaviour, IOnEventCallback
{
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        Debug.Log($"[Photon] 수신된 eventCode = {eventCode}");

        // 유저 데이터 싱크 이벤트
        if (eventCode == (int)PunEventType.MapDataSync)
        {
            Debug.Log("[MapDataSync] 유저 데이터 싱크 이벤트 OnEvent실행");
            object[] data = (object[])photonEvent.CustomData;

            MapType maptype = Extension.StringToEnum<MapType>((string)data[0]);
            Difficulty difficulty = Extension.StringToEnum<Difficulty>((string)data[1]);
            string content = (string)data[2];
            int rate = (int)data[3];

            MapData mapdata = new MapData(maptype, difficulty, content, rate);

            MapDataManager.Instance.MapDataSetting(mapdata);

        }

    }
}

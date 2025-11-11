using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class InGameRiseEvent : MonoBehaviour, IOnEventCallback
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
        // Debug.Log($"[Photon] 수신된 eventCode = {eventCode}");

        // 유저 데이터 싱크 이벤트
        if (eventCode == (int)PunEventType.UserDataSync) 
        {
            Debug.Log("[UserDataSync] 유저 데이터 싱크 이벤트 OnEvent실행");
            object[] data = (object[])photonEvent.CustomData;

            int actorNum = (int)data[0];
            string nick = (string)data[1];
            float score = (float)data[2];
            string indate = (string)data[3];

            InGamePlayer player = new InGamePlayer(actorNum, nick, score, indate);

            PunIngameManager.Instance.AddInGamePlayer(actorNum, player);

            player.PrintPlayer();
        }

        // 게임 ID 싱크 이벤트
        if(eventCode == (int)PunEventType.GameIdSync) 
        {
            Debug.Log("[GameIdSync] 게임 ID 싱크 이벤트 OnEvent실행");
            object[] data = (object[])photonEvent.CustomData;

            string id = (string)data[0];

            PunIngameManager.Instance.GameIdGuid = id;
        }
    }

 
}

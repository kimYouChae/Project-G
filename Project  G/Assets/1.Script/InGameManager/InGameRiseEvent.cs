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
            long steamId = (long)data[1];
            string nick = (string)data[2];
            float bestScore = (float)data[3];

            InGamePlayer player = new InGamePlayer(actorNum, steamId, nick, bestScore);
            player.PrintPlayer();

            PunIngameManager.Instance.AddInGamePlayer(actorNum, player);
        }

        // 게임 ID 싱크 이벤트
        if(eventCode == (int)PunEventType.GameIdSync) 
        {
            Debug.Log("[GameIdSync] 게임 ID 싱크 이벤트 OnEvent실행");
            object[] data = (object[])photonEvent.CustomData;

            string gameId = (string)data[0];
            MapType mapType = (MapType)data[1];

            PunGameoverManager.Instance.SynchedGameIDGuid = gameId;
            PunGameoverManager.Instance.SynchedGameMapType = mapType;
        }

        // (게임종료시) 점수, 스테이지 싱크 이벤트
        if (eventCode == (int)PunEventType.ScoreStageSync) 
        {
            Debug.Log("[ScoreStageSync] 점수,스테이지 싱크 이벤트 OnEvent실행");
            object[] data = (object[])photonEvent.CustomData;

            float score = (float)data[0];
            int stage = (int)data[1];

            PunGameoverManager.Instance.SynchedScore = score;
            PunGameoverManager.Instance.SynchedStage = stage;

        }
    }

 
}

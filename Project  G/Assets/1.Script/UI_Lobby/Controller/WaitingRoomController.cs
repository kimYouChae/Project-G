using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingRoomController
{
    private WaitingRoomView waitingRoomView;

    public WaitingRoomController(WaitingRoomView roomView)
    {
        this.waitingRoomView = roomView;

        waitingRoomView.RegisterGameStart(GameStart);
    }

    private void GameStart() 
    {
        // Mapdata 세팅
        MapDataManager.Instance.SettingNowMapData(PhotonRoomInfo.MapTypeName);

        // 마스터클라이언트만 가능 
        if (PhotonNetwork.IsMasterClient)
        {
            // 게임씬으로 전환
            PhotonSceneManager.Instance.ChangeGameScene();
        }
        // 마스터가 아니면
        else
        {
            // PopUp 띄우기 
            Debug.Log("마스터가 아닙니다!");
        }
    }
}

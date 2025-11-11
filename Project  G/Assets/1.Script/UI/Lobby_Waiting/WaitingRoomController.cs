using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingRoomController : ILobbyPanelInitionlize
{
    private WaitingRoomView waitingRoomView;

    public WaitingRoomController(WaitingRoomView roomView)
    {
        this.waitingRoomView = roomView;

        waitingRoomView.RegisterGameStart(GameStart);
        waitingRoomView.RegisterBackButton(BackAction);
    }

    private void BackAction()
    {
        LobbyUIManager.Instance.ChangePanel(LobbyPanelType.WaitingRoom, LobbyPanelType.RoomList);

        // 현재 있는 photon 방에서 나가기 
        PunLobbyManager.Instance.LeaveRoom();
    }

    public void IInitPanel()
    {
        
    }

    private void GameStart() 
    {

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
            TextPopUp textPopUp = UIManager.Instance.GetPopUP<TextPopUp>();
            textPopUp.UpdateText("(로컬라이징전) 마스터가아닙니다" );
        }
    }
}

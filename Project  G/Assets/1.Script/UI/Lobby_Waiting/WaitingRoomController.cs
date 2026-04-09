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
        waitingRoomView.RegisterGetFriend(GetFriend);
    }

    private void GetFriend() 
    {
        // 포톤 호스트만 친구 초대 가능
        if (!PhotonNetwork.IsMasterClient)
        {
            TextPopUp textPopUp = UIManager.Instance.GetPopUP<TextPopUp>();
            textPopUp.UpdateText("(로컬라이징전) 방장만 스팀 친구를 초대할 수 있습니다");

            return;
        }

        // 방의 인원이 MaxCount와 같으면 
        int count = PhotonNetwork.CurrentRoom.PlayerCount;
        if (count >= PhotonRoomInfo.MaxUser) 
        {
            TextPopUp textPopUp = UIManager.Instance.GetPopUP<TextPopUp>();
            textPopUp.UpdateText($"(로컬라이징전) 한 방에 최대 {PhotonRoomInfo.MaxUser}명 입니다.");

            return;
        }

        // 친구 팝업 띄우기 
        FriendPopUP popup = UIManager.Instance.GetPopUP<FriendPopUP>();
        popup.OpenFriendPopUP();
    }

    private void BackAction()
    {
        // SFX 실행
        SFXManager.Instance.PlaySFX(SFXType.UIBack);

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
            // SFX 실행
            SFXManager.Instance.PlaySFX(SFXType.UIClick);

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

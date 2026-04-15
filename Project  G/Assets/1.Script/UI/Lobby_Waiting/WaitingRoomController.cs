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
            string local = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.Popup_OnlyHostCanInvite);
            textPopUp.UpdateText(local);

            return;
        }

        // 방의 인원이 MaxCount와 같으면 
        int count = PhotonNetwork.CurrentRoom.PlayerCount;
        if (count >= PhotonRoomInfo.MaxUser) 
        {
            TextPopUp textPopUp = UIManager.Instance.GetPopUP<TextPopUp>();
            string local = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.Popup_RoomFull);
            textPopUp.UpdateText($"{local}{PhotonRoomInfo.MaxUser}");

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
            string local = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.Popup_NotMasterClient);
            textPopUp.UpdateText(local);
        }
    }
}

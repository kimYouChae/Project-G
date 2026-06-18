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
        // 포톤 네트워크의 현재 방의 인원과
        // 방이 만들어질 때 호스트가 지정한 max 인원을 비교
        // ( max 인원은 서버에 저장됨 , 호스트가 나가서 다른 클라가 호스트가 되어도 접근가능 .) 
        int count = PhotonNetwork.CurrentRoom.PlayerCount;
        if (count >= PhotonNetwork.CurrentRoom.MaxPlayers) 
        {
            TextPopUp textPopUp = UIManager.Instance.GetPopUP<TextPopUp>();
            string local = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.Popup_RoomFull);
            textPopUp.UpdateText($"{local}{PhotonNetwork.CurrentRoom.MaxPlayers}");

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
            // 방에 MAX 인원만큼 안 들어왔으면 -> PopUp 띄우기
            if (PhotonNetwork.CurrentRoom.MaxPlayers != PhotonNetwork.CurrentRoom.PlayerCount)
            {
                TextPopUp textPopUp = UIManager.Instance.GetPopUP<TextPopUp>();
                string local = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.Room_NotEnoughPlayers);
                textPopUp.UpdateText($"{local}{PhotonNetwork.CurrentRoom.MaxPlayers}");

                return;
            }

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

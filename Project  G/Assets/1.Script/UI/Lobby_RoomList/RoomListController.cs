using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class RoomListModel 
{
    public int currSelectRoomIndex;

    public RoomListModel() 
    {
        currSelectRoomIndex = -1;
    }

    public void SetRoomIndex(int roomIndex) 
    {
        currSelectRoomIndex = roomIndex;
    }

    public bool isValue() 
    {
        // 범위 안에 있는지 
        return currSelectRoomIndex >= 0 && currSelectRoomIndex < PunLobbyManager.Instance.RoomLength;
    }
}

public class RoomListController : ILobbyPanelInitionlize
{
    private RoomListView roomListView;
    private RoomListModel roomListModel;

    public RoomListController(RoomListView roomListView, RoomListModel roomListMode)
    {
        this.roomListView = roomListView;
        this.roomListModel = roomListMode;

        roomListView.RegisterRefreshRoom(RefrechRoomList);
        roomListView.RegisterJoinRoom(JoinRoom);
        roomListView.RegisterSelectRoomIndex(UpdateRoomSelectIndex);
        roomListView.RegisterBackButton(BackAction);
    }

    private void BackAction() 
    {
        // SFX 실행
        SFXManager.Instance.PlaySFX(SFXType.UIBack);

        LobbyUIManager.Instance.ChangePanel(LobbyPanelType.RoomList, LobbyPanelType.Lobby);
    }

    private void UpdateRoomSelectIndex(int idex) 
    {
        // SFX 실행
        SFXManager.Instance.PlaySFX(SFXType.UIClick);

        // 현재 선택되어 있는 방의 select Image를 Off
        if(roomListModel.currSelectRoomIndex != -1)
            roomListView.OnOffRoomSelect(roomListModel.currSelectRoomIndex , false);

        // curr room Index 설정 
        roomListModel.SetRoomIndex(idex);

        // 선택된 방의 select Image를 ON
        roomListView.OnOffRoomSelect(roomListModel.currSelectRoomIndex, true);
    }

    private void RefrechRoomList() 
    {
        // SFX 실행
        SFXManager.Instance.PlaySFX(SFXType.UIClick);

        // 포톤 - 룸 정보 업데이트 
        PunLobbyManager.Instance.RefreshRoomList();

        // 선택된 방 index 초기화
        roomListModel.SetRoomIndex(-1);

        // 룸 정보로 오브젝트 생성
        // roomListView.UpdateRoomList();
    }

    private void JoinRoom(string password) 
    {
        // SFX 실행
        SFXManager.Instance.PlaySFX(SFXType.UIClick);

        if (!roomListModel.isValue()) 
        {
            // POPUP : 알수없는오류
            TextPopUp textPopup = UIManager.Instance.GetPopUP<TextPopUp>();
            textPopup.UpdateText(LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.Popup_ROOM_JOIN_FAILED_RETRY));
            return;
        }

        EnterPassWord(password);
    }

    private void EnterPassWord(string inputPassword)
    {
        // 선택한 방
        RoomInfo info = PunLobbyManager.Instance.RoomInfoByIndex(roomListModel.currSelectRoomIndex);
        if (info == null)
        {
            // POPUP : 알수없는오류
            TextPopUp textPopUp = UIManager.Instance.GetPopUP<TextPopUp>();
            textPopUp.UpdateText(LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.Popup_ROOM_JOIN_FAILED_RETRY));
            return;
        }

        ExitGames.Client.Photon.Hashtable hashtable = info.CustomProperties;
        object passwordValue;
        object roomCodeValue;

        // 비번이 없으면 return
        if (!hashtable.TryGetValue("Password", out passwordValue))
        {
            // POPUP : 알수없는오류
            TextPopUp textPopup = UIManager.Instance.GetPopUP<TextPopUp>();
            textPopup.UpdateText(LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.Popup_ROOM_JOIN_FAILED_RETRY));
            return;
        }
        if (inputPassword.Equals(string.Empty))
        {
            // POPUP : 입력한 password가 빈칸입니다
            TextPopUp textPopUp = UIManager.Instance.GetPopUP<TextPopUp>();
            textPopUp.UpdateText(LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.Popup_PASSWORD_NOT_ENTERED));
            return;
        }
        if (!hashtable.TryGetValue("RoomCode", out roomCodeValue)) 
        {
            // POPUP : 방 코드가 존재하지 않습니다. 
            TextPopUp textPopUp = UIManager.Instance.GetPopUP<TextPopUp>();
            textPopUp.UpdateText(LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.Popup_ROOM_JOIN_FAILED_RETRY));
            return;           
        }

        // 방의 비밀번호 
        int roomPassword = (int)passwordValue;

        // 방 비번 = 비번 입력이 같으면 
        if (int.Parse(inputPassword) == roomPassword)
        {
            Debug.Log("올바른 비밀번호를 입력 했습니다! 방에 입장 합니다");

            // 방 참가 시도 
            PunLobbyManager.Instance.JoinRoom((string)roomCodeValue);

            // panel 변경 
            LobbyUIManager.Instance.ChangePanel(LobbyPanelType.RoomList, LobbyPanelType.WaitingRoom);
        }
        else
        {
            // POPUP : 비밀번호가 다릅니다
            TextPopUp textPopUp = UIManager.Instance.GetPopUP<TextPopUp>();
            textPopUp.UpdateText(LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.Popup_PASSWORD_INCORRECT));
        }

    }

    public void IInitPanel()
    {
        
    }
}

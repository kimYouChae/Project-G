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
        return currSelectRoomIndex >= 0 && currSelectRoomIndex <= PunLobbyManager.Instance.RoomLength;
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

        roomListModel.SetRoomIndex(idex);
    }

    private void RefrechRoomList() 
    {
        // SFX 실행
        SFXManager.Instance.PlaySFX(SFXType.UIClick);

        // 포톤 - 룸 정보 업데이트 
        PunLobbyManager.Instance.RefreshRoomList();

        // 룸 정보로 오브젝트 생성
        roomListView.UpdateRoomList();
    }

    private void JoinRoom(string password) 
    {
        // SFX 실행
        SFXManager.Instance.PlaySFX(SFXType.UIClick);

        if (!roomListModel.isValue()) 
        {
            TextPopUp textPopup = UIManager.Instance.GetPopUP<TextPopUp>();
            textPopup.UpdateText("(로컬라이징전) 유효하지 않은 방번호");
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

            return;
        }

        ExitGames.Client.Photon.Hashtable hashtable = info.CustomProperties;
        object passwordValue;
        object roomCodeValue;

        // 비번이 없으면 return
        if (!hashtable.TryGetValue("Password", out passwordValue))
        {
            // POPUP : 알수없는오류
            
            return;
        }
        if (inputPassword.Equals(string.Empty))
        {
            // POPUP : 입력한 password가 빈칸입니다

            return;
        }
        if (!hashtable.TryGetValue("RoomCode", out roomCodeValue)) 
        {
            // POPUP : 방 코드가 존재하지 않습니다. 

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

            Debug.Log("비밀번호가 다릅니다! ");
        }

    }

    public void IInitPanel()
    {
        
    }
}

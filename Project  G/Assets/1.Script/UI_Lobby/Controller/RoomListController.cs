using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
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

public class RoomListController 
{
    private RoomListView roomListView;
    private RoomListModel roomListModel;

    public RoomListController (RoomListView roomListView, RoomListModel roomListMode)
    {
        this.roomListView = roomListView;
        this.roomListModel = roomListMode;

        roomListView.RegisterRefreshRoom(RefrechRoomList);
        roomListView.RegisterJoinRoom(JoinRoom);
        roomListView.RegisterSelectRoomIndex(UpdateRoomSelectIndex);
    }

    private void UpdateRoomSelectIndex(int idex) 
    {
        roomListModel.SetRoomIndex(idex);
    }

    private void RefrechRoomList() 
    {
        // 포톤 - 룸 정보 업데이트 
        PunLobbyManager.Instance.RefreshRoomList();

        // 룸 정보로 오브젝트 생성
        roomListView.UpdateRoomList();
    }

    private void JoinRoom(string password) 
    {
        if (!roomListModel.isValue()) 
        {
            // ## POPUP : view의 UI 띄우기 


            Debug.Log("유효하지 않는 방 번호");
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
        object value;

        // 비번이 없으면 return
        if (!hashtable.TryGetValue("Password", out value))
        {
            // POPUP : 알수없는오류
            Debug.Log("해당 Room에 비밀번호가 존재하지 않습니다!");
            return;
        }
        if (inputPassword.Equals(string.Empty))
        {
            // POPUP : 입력한 password가 빈칸입니다

            return;
        }

        // 방의 비밀번호 
        int roomPassword = (int)value;

        // 방 비번 = 비번 입력이 같으면 
        if (int.Parse(inputPassword) == roomPassword)
        {
            Debug.Log("올바른 비밀번호를 입력 했습니다! 방에 입장 합니다");

            // 방 참가 시도 
            PunLobbyManager.Instance.JoinRoom(info.Name);

            // panel 변경 
            LobbyUIManager.GetInstance().ChangePanel(LobbyPanelType.RoomList, LobbyPanelType.WaitingRoom);
        }
        else
        {
            // POPUP : 비밀번호가 다릅니다

            Debug.Log("비밀번호가 다릅니다! ");
        }

    }
}

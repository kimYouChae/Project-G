using Photon.Pun;
using Photon.Realtime;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


// room을 만들 때 설정하는 데이터를 담아놓는 용도
public static class PhotonRoomInfo
{
    // 방 고유 번호
    public static string RoomCode 
    {
        get => PlayerPrefs.GetString("RoomCode");
        set => PlayerPrefs.SetString("RoomCode", value);
    }

    // 방 이름
    public static string RoomName
    {
        get => PlayerPrefs.GetString("RoomName");
        set => PlayerPrefs.SetString("RoomName", value);
    }

    // 비밀번호 
    public static int Password
    {
        get => PlayerPrefs.GetInt("Password");
        set => PlayerPrefs.SetInt("Password", value);
    }

    // 최대 유저
    public static int MaxUser
    {
        get => PlayerPrefs.GetInt("MaxUser");
        set => PlayerPrefs.SetInt("MaxUser", value);
    }

    // 맵 타입
    public static string MapTypeName
    {
        get => PlayerPrefs.GetString("MapType");
        set => PlayerPrefs.SetString("MapType", value);
    }
}

public class PunLobbyManager : Singleton<PunLobbyManager>
{
    // 생성된 방 정보
    private List<RoomInfo> roomInfoList;
    
    public List<RoomInfo> RoomInfoList { get { return roomInfoList; } }
    public int RoomLength { get { return roomInfoList.Count; } }

    protected override void Singleton_Awake()
    {

    }

    // 세션 세팅
    public void SettingSession(List<RoomInfo> list) 
    {
        this.roomInfoList = list;

        // UI 업데이트 
        LobbyUIManager.Instance.UpdateRoomListView();

        // 출력 
        string str = string.Empty;
        for (int i = 0; i < roomInfoList.Count; i++) 
        {
            var roomInfo = roomInfoList[i];

            str += roomInfo.ToStringFull();
        }
        Debug.Log(str);
    }

    public void SettingPhotonLocalPlayer() 
    {
        // 포톤 로컬 닉네임 
        PhotonNetwork.NickName = SteamUserData.Instance.NickName;

        // 포톤 커스텀 프로퍼티 설정 
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
        props["SteamId"] = SteamUserData.Instance.SteamID;

        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    // 방 커스텀 생성
    public void CreateCusomRoom() 
    {
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable() 
        {
            // 방 코드
            { "RoomCode", PhotonRoomInfo.RoomCode}, 
            // 방 이름
            { "RoomName" , PhotonRoomInfo.RoomName },
            // 비밀번호
            { "Password" , PhotonRoomInfo.Password },
            // 게임이 시작 했는지
            { "IsStartGame" , false },
            // 맵 타입
            { "MapType" , PhotonRoomInfo.MapTypeName }
        };

        RoomOptions roomOption = new RoomOptions()
        {
            MaxPlayers = PhotonRoomInfo.MaxUser,

            // 방 내부에서 접근 가능한 데이터
            CustomRoomProperties = hashtable,

            // 로비에서 방 리스트 볼 때 노출할 목록
            CustomRoomPropertiesForLobby = new string[]
            {
                "RoomName", "Password" , "MapType"
            }
        };

        // 방 생성 
        // PhotonNetwork의 CurrentRoom에 방 정보가 저장됨 
        bool isSuccess = PhotonNetwork.CreateRoom
        (
            PhotonRoomInfo.RoomName,
            roomOption
        );

        if (isSuccess)
        {
            Debug.Log("방 생성에 성공 했습니다");
        }
        else
        { 
            Debug.Log("방 생성에 실패 했습니다 ");
                
            // PopUP : 방생성에 실패
        }

    }
    
    public void PrintRoomInfo(Room info) 
    {
        if (info == null)
        {
            Debug.Log("방이 NULL입니다");
            return;
        }

        StringBuilder sb = new StringBuilder();
        sb.Append("방 이름 : " + info.Name + " \n");
        sb.Append("방 인원 : " + info.MaxPlayers + " \n");
        if (info.CustomProperties.TryGetValue("Password", out object value)) 
        {
            sb.Append("방 비번 :" + value + "\n");
        }
        if (info.CustomProperties.TryGetValue("MapType", out object type)) 
        {
            sb.Append("방 타입 :" + type + "\n");
        }

        Debug.Log(sb);
    }


    // 룸 목록 업데이트
    public void RefreshRoomList() 
    {
        // 로비를 재접속 하는것 만으로 OnRoomListUpdate가 실행 x
        // 로비를 벗어났다가 N초후에 다시 재접속 

        PhotonNetwork.LeaveLobby();
        StartCoroutine(ReJoinLobbyDelay());
    }

    IEnumerator ReJoinLobbyDelay() 
    {
        yield return new WaitForSeconds(0.02f);

        PhotonNetwork.JoinLobby();
    }

    public void JoinRoom(string roomId) 
    {
        // 이미 방에 있으면 떠나기
        if (PhotonNetwork.InRoom)
        {
            LeaveRoom();
        }

        // LeaveRoom 끝난 후 JoinRoom 해야 함
        PhotonNetwork.JoinRoom(roomId);
    }

    public void LeaveRoom()
    {
        // 방에서 나가기
        PhotonNetwork.LeaveRoom();
    }

    public void JoinLobby()
    {
        // 로비에 입장하기
        PhotonNetwork.JoinLobby();
    }

    // 인덱스에 해당하는 RoomInfo를 return
    public RoomInfo RoomInfoByIndex(int index) 
    { 
        if(index < 0 || index >= roomInfoList.Count)
        {
            return null;
        }

        return roomInfoList[index];
    }

}

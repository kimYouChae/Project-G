using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

public class PunLobbyManager : Singleton<PunLobbyManager>
{
    // 생성된 방 정보
    private List<RoomInfo> roomInfoList;
    // room에 참가한 플레이어 정보
    private Player[] playerList;
    
    public List<RoomInfo> RoomInfoList { get { return roomInfoList; } }
    public Player[] PlayerList { get { return playerList; } }


    protected override void Singleton_Awake()
    {

    }

    // 세션 세팅
    public void SettingSession(List<RoomInfo> list) 
    {
        this.roomInfoList = list;

        // UI 업데이트 
        LobbyUIManager.GetInstance().UpdateRoomList();

        // 출력 
        string str = string.Empty;
        for (int i = 0; i < roomInfoList.Count; i++) 
        {
            var roomInfo = roomInfoList[i];

            str += roomInfo.ToStringFull();
        }
        Debug.Log(str);
    }

    // 플레이어 세팅 (입장)
    public void UpdateRoomUser()
    {
        // LobbyUI : 유저 업데이트 필요 
        LobbyUIManager.GetInstance().UpdateWaitingRoomInfo(PhotonNetwork.PlayerList);
    }

    // 닉네임 세팅
    public void SettingNickName(string nickname)
    {
        // #Important
        if (string.IsNullOrEmpty(nickname))
        {
            Debug.LogError("Player Name is null or empty");
            return;
        }
        PhotonNetwork.NickName = nickname;
    }

    // 방 커스텀 생성
    public void CreateCusomRoom() 
    {
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable() 
        {
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
            Debug.Log("방 생성에 성공 했습니다");
        else
            Debug.Log("방 생성에 실패 했습니다 ");

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

    public void JoinRoom(string title) 
    {
        // 방에 접속하기 
        PhotonNetwork.JoinRoom(title);
    }

}

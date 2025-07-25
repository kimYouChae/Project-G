using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PunLobbyManager : MonoBehaviour
{
    private static PunLobbyManager instance;   // 인스턴스

    // 생성된 방 정보
    private List<RoomInfo> roomInfoList;
    // room에 참가한 플레이어 정보
    private Player[] playerList;
    
    public List<RoomInfo> RoomInfoList { get { return roomInfoList; } }
    public Player[] PlayerList { get { return playerList; } }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;

        roomInfoList = new List<RoomInfo>();
    }

    public static PunLobbyManager GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("BackEndServerManager 인스턴스가 존재하지 않습니다.");
            return null;
        }

        return instance;
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
            { "RoomName" , FusionRoomInfo.RoomName },
            // 비밀번호
            { "Password" , FusionRoomInfo.Password },
            // 게임이 시작 했는지
            { "IsStartGame" , false }
        };

        RoomOptions roomOption = new RoomOptions()
        {
            MaxPlayers = FusionRoomInfo.MaxUser,

            // 방 내부에서 접근 가능한 데이터
            CustomRoomProperties = hashtable,

            // 로비에서 방 리스트 볼 때 노출할 목록
            CustomRoomPropertiesForLobby = new string[]
            {
                "RoomName", "Password"
            }
        };

        // 방 생성 
        // PhotonNetwork의 CurrentRoom에 방 정보가 저장됨 
        bool isSuccess = PhotonNetwork.CreateRoom
        (
            FusionRoomInfo.RoomName,
            roomOption
        );

        if (isSuccess)
            Debug.Log("방 생성에 성공 했습니다");
        else
            Debug.Log("방 생성에 실패 했습니다 ");


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

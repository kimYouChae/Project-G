using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunLauncher : MonoBehaviourPunCallbacks
{
    // 게임 버젼
    string gameVersion = "1";

    private void Awake()
    {
        // LoadLevel() 사용가능 ? 
        // 씬 동기화 | 마스터 클라이언트가 씬을 바꾸면 다른 클라이언트들도 자동으로 같은 씬으로 이동하게 해 줌
        // PhotonNetwork.LoadLevel("GameScene");로 씬 전환 해야함
        PhotonNetwork.AutomaticallySyncScene = true;

        if (!PhotonNetwork.IsConnected)
        {
            // *필수* 포톤 서버에 연결!
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    /// <summary>
    /// MonoBehaviourPunCallbacks의 콜백
    /// PhotonNetwork.ConnectUsingSettings()이 성공했을 때 / 성공못했을때 콜백
    /// </summary>
    public override void OnConnectedToMaster()
    {
        Debug.Log("Pun : OnConnectedToMaster 콜백실행 | 연결이 성공적입니다");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("Pun : OnDisconnected 콜백실행 | 연결안됨 :  {0}", cause);
    }

    /// <summary>
    /// PhotonNetwork.JoinLobby()이 성공했을 때 
    /// </summary>
    public override void OnJoinedLobby()
    {
        Debug.Log($"Pun : OnJoinedLobby 콜백실행 | { PhotonNetwork.NickName } 로비에 입장합니다");
    }

    /// <summary>
    /// Lobby에 재접속될때 , 다른 호스트가 CreatRoom을 할 때
    /// </summary>
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log($"Pun : roomList 콜백실행 | 방 목록이 업데이트 되었습니다");

        // 룸 리스트 넘기기 
        PunLobbyManager.GetInstance().SettingSession(roomList);
    }

    /// <summary>
    /// 룸에 Join할 때 
    /// 방 생성시에는 X 
    /// 다른 클라이언트가 방에 입장 했을 때 - > 이미 방에 있던 사람들이 받음 
    /// </summary>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"Pun : OnPlayerEnteredRoom 콜백실행 | {newPlayer.NickName} 새로 들어왔습니다");

        PunLobbyManager.GetInstance().UpdateRoomUser();
    }

    /// <summary>
    /// 룸에 Exit할 때 
    /// </summary>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"Pun : OnPlayerLeftRoom 콜백실행 | {otherPlayer.NickName} 새로 들어왔습니다");

        PunLobbyManager.GetInstance().UpdateRoomUser();
    }

    /// <summary>
    /// PhotonNetwork.JoinRoom()이 성공 / 실패 했을 때
    /// CreateRoom했을 때도 콜백이 실행됨 -> 로컬 유저 저장 가능 
    /// ! 내가 방에 성공적으로 입장 했을 때 
    /// </summary>

    public override void OnJoinedRoom()
    {
        Debug.Log($"Pun : OnJoinedRoom 콜백실행 | Room에 접속 했습니다");

        // 내가 방에 성공적으로 입장 헀을 때
        PunLobbyManager.GetInstance().UpdateRoomUser();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Pun : OnJoinRoomFailed 콜백실행 | Room 접속에 실패 했습니다 {message} ");
    }

}


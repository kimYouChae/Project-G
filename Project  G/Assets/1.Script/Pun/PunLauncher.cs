using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PunLauncher : MonoBehaviourPunCallbacks
{
    // 게임 버젼
    private string gameVersion = "1";

    private bool isReturningFromGame = false;       // 게임씬 -> 로비씬 돌아왔는지

    private void Awake()
    {
        SceneManager.sceneLoaded += SceneLoaded;

        // LoadLevel() 사용가능 ? 
        // 씬 동기화 | 마스터 클라이언트가 씬을 바꾸면 다른 클라이언트들도 자동으로 같은 씬으로 이동하게 해 줌
        // PhotonNetwork.LoadLevel("GameScene");로 씬 전환 해야함
        PhotonNetwork.AutomaticallySyncScene = true;

        if (!PunConnected.hasStartedPhotonConnect && !PhotonNetwork.IsConnected)
        {
            // *필수* 포톤 서버에 연결!
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();

            PunConnected.hasStartedPhotonConnect = true;

            Debug.Log("[PunLauncher] Photon 서버 최초 연결 시도");

            // 타이틀 화면 켜기
            LobbyUIManager.Instance.ChangePanel(LobbyPanelType.None, LobbyPanelType.Title);

            // 게임 BGM 실행 ( Title
            BGMManager.Instance.PlayBGM(BGMType.Title);
        }
        else 
        {
            Debug.Log("[PunLauncher] 이미 Photon 서버에 연결되어 있음");
            isReturningFromGame = true;

            // 게임 BGM 실행 ( Lobby
            BGMManager.Instance.PlayBGM(BGMType.Lobby);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // awake 실행하고 -> 씬 로드 완료 되기 때문에 
        // awake에서 isReturningFromGame가 true, 이후 if문 들어감 

        // 1. 로비씬
        // 2. 접속해 있는 상태일 때 
        if (scene.name == Define.sceneNames[SceneType.Lobby]
            && isReturningFromGame)
        {
            // waiting Room 화면 켜기 
            LobbyUIManager.Instance.ChangePanel(LobbyPanelType.None, LobbyPanelType.WaitingRoom);
            // 유저 업데이트 
            LobbyUIManager.Instance.UpdateWaitinRoomView(PhotonNetwork.PlayerList);
        }
    }

    /// <summary>
    /// MonoBehaviourPunCallbacks의 콜백
    /// PhotonNetwork.ConnectUsingSettings()이 성공했을 때 / 성공못했을때 콜백
    /// </summary>
    public override void OnConnectedToMaster()
    {
        // 최초 접속 , 재접속 , 룸 이동, 로비 이동 등의 과정에서 실행됨. 

        Debug.Log("Pun : OnConnectedToMaster 콜백실행 | 연결이 성공적입니다");

        // 처음 콜백 실행되었을 때만 실행 
        if ( !PunConnected.hasHandledInitialPhotonConnect) 
        {
            PunConnected.hasHandledInitialPhotonConnect = true;
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("Pun : OnDisconnected 콜백실행 | 연결안됨 :  {0}", cause);

        LobbyUIManager.Instance.ChangePanel( LobbyPanelType.None , LobbyPanelType.UnTitled );
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
        PunLobbyManager.Instance.SettingSession(roomList);
    }

    /// <summary>
    /// 룸에 Join할 때 
    /// 방 생성시에는 X 
    /// 다른 클라이언트가 방에 입장 했을 때 - > 이미 방에 있던 사람들이 받음 
    /// </summary>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"Pun : OnPlayerEnteredRoom 콜백실행 | {newPlayer.NickName} 방에 들어왔습니다");

        PunLobbyManager.Instance.UpdateRoomUser();

        // MapData 동기화
        // 게임씬 -> 로비로 다시 돌아왔을 땐 해당 콜백이 실행이 안되니 또 실행안될듯 
        if (PhotonNetwork.IsMasterClient)
        {
            // 다른 클라이언트는 PhotonRoomInfo의 플레이어프리팹에 아무것도 안들어가잇음
            MapDataManager.Instance.SettingNowMapData(PhotonRoomInfo.MapTypeName);
        }
    }

    /// <summary>
    /// 로컬플레이어 제외, 다른 플레이어가 방에서 나갔을 때
    /// </summary>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"Pun : OnPlayerLeftRoom 콜백실행 | {otherPlayer.NickName} 방을 나갔습니다");

        PunLobbyManager.Instance.UpdateRoomUser();
    }

    /// <summary>
    /// 로컬플레이어가 방에서 나갔을 때 
    /// </summary>
    public override void OnLeftRoom()
    {
        Debug.Log($"Pun : OnLeftRoom 콜백실행 | {PhotonNetwork.NickName} 방을 나갔습니다");
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
        PunLobbyManager.Instance.UpdateRoomUser();

        // 방에 들어왔을 때 방 정보 출력
        // PunLobbyManager.Instance.PrintRoomInfo(PhotonNetwork.CurrentRoom);

        // MapData 동기화
        // 게임씬 -> 로비로 다시 돌아왔을 땐 해당 콜백이 실행이 안되니 또 실행안될듯 
        if (PhotonNetwork.IsMasterClient) 
        {
            // 다른 클라이언트는 PhotonRoomInfo의 플레이어프리팹에 아무것도 안들어가잇음
            MapDataManager.Instance.SettingNowMapData(PhotonRoomInfo.MapTypeName);
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Pun : OnJoinRoomFailed 콜백실행 | Room 접속에 실패 했습니다 {message} ");

        TextPopUp textPopUp = UIManager.Instance.GetPopUP<TextPopUp>();
        textPopUp.UpdateText(LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.Popup_CreateRoomFailed));
    }

}


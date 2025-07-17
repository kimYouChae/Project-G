using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FusionManager : MonoBehaviour 
{
    private static FusionManager instance;

    // 네트워크 주체
    [SerializeField] private NetworkRunner runner;
    // 콜백
    [SerializeField] private FusionCallBack callback;
    // 세션 리스트
    [SerializeField] private List<SessionInfo> sessionInfoList;

    [Header("임시 컴포넌트")]
    public LobbyUI lobbyUi;

    // 프로퍼티
    public List<SessionInfo> sessionInfoLists { get => sessionInfoList; }

    public static FusionManager GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("FusionManager 인스턴스가 존재하지 않습니다.");
            return null;
        }
        return instance;
    }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }

        // 네트워크와 통신하기 위해서 
        if (!gameObject.TryGetComponent<NetworkRunner>(out runner))
            runner = gameObject.AddComponent<NetworkRunner>();
         if(!gameObject.TryGetComponent<FusionCallBack>(out callback))
            callback = gameObject.AddComponent<FusionCallBack>();

        // 콜백 등록 
        // runner.AddCallbacks(callback);

        sessionInfoList = new List<SessionInfo>();

        StartAsync();

    }

    private async Task StartAsync()
    {
        // 로비에 접속 필요 
        var joinResult = await runner.JoinSessionLobby(SessionLobby.Shared);
    }

    // 방 생성 
    public async void CreateFusionRoom(GameMode mode) 
    {
        Debug.Log("=====*&^%방생성*&^%=====");

        // 콜백 등록 
        runner.AddCallbacks(callback);

        Debug.Log($"방생성 정보 :{FusionRoomInfo.RoomName} / {FusionRoomInfo.Password} ");

        try 
        {
            StartGameArgs gameArgs = new StartGameArgs()
            {
                GameMode = mode,
                SessionName = FusionRoomInfo.RoomName,
                CustomLobbyName = FusionRoomInfo.RoomName,
                PlayerCount = FusionRoomInfo.MaxUser,

                // 방 커스텀 정보 (딕셔너리 구조 / 추후 필터링 가능)
                SessionProperties = new Dictionary<string, SessionProperty>
                {
                   { "Password" , (SessionProperty)FusionRoomInfo.Password }
                },

            };

            // 방 생성
            StartGameResult temp = await runner.StartGame(gameArgs);

            if (temp.Ok)
                Debug.Log("===ㅊㅊㅊㅊ방생성 완 !ㅊㅊㅊㅊㅊㅊ");
            else
                Debug.Log("===저런 방생성 실패===");

        }
        catch (Exception ex) 
        {
            Debug.Log("방 생성중!! 예외발생 + " + ex);
        }
    }

    // 방 참가
    public async Task JoinFusionRoom(string roomName, GameMode mode = GameMode.Client) 
    {
        try 
        {
            StartGameArgs gamdArgs = new StartGameArgs()
            {
                GameMode = mode,
                SessionName = roomName
            };

            // 참가
            StartGameResult temp = await runner.StartGame(gamdArgs);

            if (temp.Ok)
                Debug.Log("===방참가 완 !ㅊㅊㅊㅊㅊㅊ");
            else
                Debug.Log("===방참가 실패===");
        }
        catch (Exception ex)
        {
            Debug.Log("방 참가중!! 예외발생 + " + ex);
        }
    }

    public void SettingSessionInfo(List<SessionInfo> sessionlist) 
    {
        this.sessionInfoList = sessionlist;

        // 출력 
        DebugCurrSession(sessionlist);
    }

    // 현재 session 정보 출력 
    private void DebugCurrSession(List<SessionInfo> sessionList)
    {
        if (sessionList == null)
        {
            Debug.Log("생성된 rooms가 없습니다");
            return;
        }

        Debug.Log("*&^^방출력^^&*");

        int index = 1;
        foreach (SessionInfo info in sessionList) 
        {
            Debug.Log($" {index}번째 \n 방 이름 :  {info.Name} \n 방 인원 : {info.PlayerCount} \n");
            
            var properties = info.Properties;
            foreach(var dic in properties) 
            {
                Debug.Log($"{dic.Key} : {dic.Value} \n");
            }

            index++;
        }

    }
    
}

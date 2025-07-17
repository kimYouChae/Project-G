using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum MapType 
{
    None,
    Basic
}
public enum ModeType 
{
    Node,
    Fight
}

// room을 만들 때 설정하는 데이터를 담아놓는 용도
public static class FusionRoomInfo 
{
    // 게임모드
    public static int GameMode
    {
        get => PlayerPrefs.GetInt("GameMode");
        set => PlayerPrefs.SetInt("GameMoe", value);
    }

    // 맵 
    public static int TrackId 
    {
        get => PlayerPrefs.GetInt("TrackId");
        set => PlayerPrefs.SetInt("TrackId", value);
    }

    // 최대 유저
    public static int MaxUser 
    {
        get => PlayerPrefs.GetInt("MaxUser");
        set => PlayerPrefs.SetInt("MaxUser", value);
    }

    // 비밀번호 
    public static int Password 
    {
        get => PlayerPrefs.GetInt("Password");
        set => PlayerPrefs.SetInt("Password", value);
    }

    // 방 이름
    public static string RoomName
    {
        get => PlayerPrefs.GetString("RoomName");
        set => PlayerPrefs.SetString("RoomName", value);
    }
}

public class LobbyUI : MonoBehaviour
{
    [Space]
    [SerializeField] private Button hostRoomButton; // 호스트 버튼 (방 생성)
    [SerializeField] private Button joinRoomButton;   // 방 참가 버튼 
    [SerializeField] private Button createRoomButton;  // 방 생성 버튼 

    [Space]
    [SerializeField] private GameObject createRoomUi; // 방생성 UI
    [SerializeField] private GameObject roomListUi;   // 룸 리스트 UI
    [SerializeField] private GameObject inRoomUi;      // 생성한 방 UI

    [Header("방 생성")]
    [SerializeField] private TMP_InputField roomNameInputField;
    [Header("방 정보")]
    [SerializeField] private MapType mapType = MapType.Basic;   // 맵 타입
    [SerializeField] private string roomTitle;  // 방 이름
    [SerializeField] private int clientCount = 2;   // 참여 인원수
    [SerializeField] private ModeType modeType = ModeType.Fight;     // 모드 타입
    [SerializeField] private int roomPassword;      // 방 비번 (랜덤)

    [Header("생성한 방")]


    private const int maxPasswordDigit = 8;

    private void Awake()
    {
        // 방생성 버튼 -> 방 생성 UI 켜기
        hostRoomButton.onClick.AddListener(() => OnOffCreateRoomUI(true));
        // 방 Join 버튼 -> 룸 목록 UI 켜기
        joinRoomButton.onClick.AddListener( () => OnOffRoolListUi(true));
        // 방 생성 버튼 -> room 정보 바탕으로 방 생성
        // 1. 방 정보 세팅
        // 2. 방 생성
        // 3. 후 방 UI 
        createRoomButton.onClick.AddListener ( () =>
            {
                RoomInfoSetting();
                FusionRoomCreate();
                OnOffRoomUI(true);
            }
        );

        // 방 비번 랜덤으로 설정 
        roomPassword = GetRandomNPassword();
    }

    // 방생성 UI
    private void OnOffCreateRoomUI(bool flag)
    {
        createRoomUi.SetActive(flag);
    }
    
    // 방 목록UI
    private void OnOffRoolListUi(bool flag) 
    {
        roomListUi.SetActive(flag);        
    }

    // 방 UI
    private void OnOffRoomUI(bool flag) 
    {
        inRoomUi.SetActive(flag);
    }

    #region Cliend - Room 생성하기 

    // 방 생성하기 
    private void RoomInfoSetting() 
    {
        mapType = MapType.Basic;

        FusionRoomInfo.GameMode = (int)mapType;
        FusionRoomInfo.TrackId = (int)modeType;
        FusionRoomInfo.MaxUser = clientCount;
        FusionRoomInfo.Password = roomPassword;
        FusionRoomInfo.RoomName = roomNameInputField.text;

        roomTitle = roomNameInputField.text;
    }

    // 비밀번호 랜덤 생성
    public static int GetRandomNPassword()
    {
        int min = (int)Mathf.Pow(10, maxPasswordDigit - 1);
        int max = (int)Mathf.Pow(10, maxPasswordDigit) - 1;
        return Random.Range(min, max + 1); // max는 포함되지 않으므로 +1
    }
    #endregion

    #region Server - Room 생성하기
    private void FusionRoomCreate() 
    {
        Debug.Log($"방 생성 시도 - RoomName: {FusionRoomInfo.RoomName}");

        if (string.IsNullOrEmpty(FusionRoomInfo.RoomName))
        {
            Debug.LogError("방 이름이 비어 있습니다!");
            return;
        }

        //FusionManager의 메서드 실행 
        FusionManager.GetInstance().CreateFusionRoom(GameMode.Host);
    }

    #endregion

    public void UpdatdCurrentRoomInfo(List<SessionInfo> sessionList) 
    {
        // 꺼져있으면 업데이트 x 
        if (inRoomUi.activeSelf == false)
            return;

        // UI에 생성 로직 작성필요
        // 매번 업데이트하면 비효율적일지도, 생각해봐야함 

    }
}

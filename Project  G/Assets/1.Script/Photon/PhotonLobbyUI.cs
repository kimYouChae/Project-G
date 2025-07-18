using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum MapType { Basic }
public enum ModeType { Fight }

public class PhotonLobbyUI : MonoBehaviour
{
    [Space]
    [SerializeField] private Button hostRoomButton; // 호스트 버튼 (방 생성)
    [SerializeField] private Button joinRoomButton;   // 방 참가 버튼 

    [Space]
    [SerializeField] private GameObject createRoomUi; // 방생성 UI
    [SerializeField] private GameObject roomListUi;   // 룸 리스트 UI
    [SerializeField] private GameObject inRoomUi;      // 생성한 방 UI

    [Header("방 생성")]
    [SerializeField] private Button createRoomButton;  // 방 생성 버튼 
    [SerializeField] private TMP_InputField roomNameInputField;
    [Header("방 정보")]
    [SerializeField] private MapType mapType = MapType.Basic;   // 맵 타입
    [SerializeField] private string roomTitle;  // 방 이름
    [SerializeField] private int clientCount = 2;   // 참여 인원수
    [SerializeField] private ModeType modeType = ModeType.Fight;     // 모드 타입
    [SerializeField] private int roomPassword;      // 방 비번 (랜덤)

    [Header("생성한 방 목록(임시)")]
    [SerializeField] private Button updateRoomListButton;     // 룸 목록 업데이트
    [SerializeField] private Button enterRoom;    // 방에 입장 
    [SerializeField] private TextMeshProUGUI[] roomInfoTexts;   // 방 텍스트 리스트 

    [Header("비밀번호")]
    [SerializeField] private GameObject passWordPopUp;  // 비번 팝업
    [SerializeField] private TMP_InputField passWordText;
    [SerializeField] private Button enterPassWord;

    private const int maxPasswordDigit = 8;

    [Header("대기방")]
    [SerializeField] private TextMeshProUGUI waitingRoomTitle;
    [SerializeField] private TextMeshProUGUI[] waitingPlayerName;
    [SerializeField] private Button startGameButton;

    private void Awake()
    {
        // 방생성 버튼 -> 방 생성 UI 켜기
        hostRoomButton.onClick.AddListener(() => OnOffCreateRoomUI(true));
        // 참가하기 버튼 
        joinRoomButton.onClick.AddListener( () => 
        {
            OnOffRoolListUi(true);      // ->룸 목록 UI 켜기
            UpdatdCurrentRoomInfo();    // 생성된 방 리스트 업데이트
        });
        // IN ROOM 생성 버튼 
        createRoomButton.onClick.AddListener ( () =>
            {
                RoomInfoSetting();  // 1. 방 정보 세팅
                FusionRoomCreate(); // 2. 방 생성
                OnOffRoomUI(true); // 3. 후 방 UI 
            }
        );

        // 방 목록 업데이트 버튼
        updateRoomListButton.onClick.AddListener(UpdatdCurrentRoomInfo);
        // 방에 입장 버튼
        enterRoom.onClick.AddListener( () => 
        {
            passWordPopUp.SetActive(true); 
            OnOffRoolListUi(false);
            OnOffRoomUI(true); 
        });

        // 비번입력후 버튼
        enterPassWord.onClick.AddListener(EnterPassWord);

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

        //FusionRoomInfo.GameMode = (int)mapType;
        //FusionRoomInfo.TrackId = (int)modeType;
        FusionRoomInfo.MaxUser = clientCount;
        FusionRoomInfo.Password = roomPassword;
        FusionRoomInfo.RoomName = roomNameInputField.text;

        roomTitle = roomNameInputField.text;
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

    // 생성된 방 리스트 업데이트 
    public void UpdatdCurrentRoomInfo() 
    {
        // 방리스트 Ui 꺼져있으면 업데이트 x 
        if (roomListUi.activeSelf == false)
            return;

        // 현재 만들어진 세션 
        List<SessionInfo> sessionIninfo = FusionManager.GetInstance().SessionInfoLists;

        Debug.Log("방 세션 갯수: " + sessionIninfo.Count);

        string format = "방 이름 :  {0}  인원 : {1}";
        for (int i = 0; i < sessionIninfo.Count; i++) 
        {
            SessionInfo info = sessionIninfo[i];

            Debug.Log("방 이름 : " + info.Name);
            roomInfoTexts[i].text = string.Format(format , info.Name , info.PlayerCount );
        }
    }

    private void EnterPassWord() 
    {
        // 비번 입력받기
        string inputPassword = passWordText.text;

        // ##TODO : 임시 - 첫번째 비번 기준 , 추후 수정필요 
        SessionInfo info = FusionManager.GetInstance().SessionInfoLists[0];
        if (info == null)
            return;

        SessionProperty value;
        // 비번이 없으면 return
        if (!info.Properties.TryGetValue("Password", out value))
        {
            Debug.Log("해당 Room에 비밀번호가 존재하지 않습니다!");
            return;
        }
        if (inputPassword.Equals(string.Empty))
            return;

        int roomPassword = (int)value;
        
        // 같으면 
        if(int.Parse(inputPassword) == roomPassword) 
        {
            Debug.Log("올바른 비밀번호를 입력 했습니다! 방에 입장 합니다");

            Debug.Log($"!@#$ 방이름 {info.Name}!@#$");
            // 방 참가 시도 
            FusionManager.GetInstance().JoinFusionRoom(info.Name);

        }
        else 
        {
            Debug.Log("비밀번호가 다릅니다! ");
        }

        passWordPopUp.SetActive(false);
    }

    // In Room(대기방) 정보 업데이트
    public void UpdateWaitingRoomInfo() 
    {
        // 꺼져있을땐 return
        if (inRoomUi.activeSelf == false)
            return;

        SessionInfo info = FusionManager.GetInstance().SessionInfoLists[0];

        // 방제 업데이트
        waitingRoomTitle.text = info.Name;
        // 플레이어 정보 입력 
        List < PlayerRef > te = FusionManager.GetInstance().JoinPlayersRefInfo;
        for (int i = 0; i < te.Count; i++) 
        {
            waitingPlayerName[i].text = te[i].PlayerId.ToString();
        }
    }
    public int GetRandomNPassword()
    {
        int min = (int)Mathf.Pow(10, maxPasswordDigit - 1);
        int max = (int)Mathf.Pow(10, maxPasswordDigit) - 1;
        return Random.Range(min, max + 1); // max는 포함되지 않으므로 +1
    }

}

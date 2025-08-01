using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// room을 만들 때 설정하는 데이터를 담아놓는 용도
public static class PhotonRoomInfo
{
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


public partial class LobbyUIManager : MonoBehaviour
{
    [Space]
    [Header("===CreateUi===")]
    [SerializeField] TMP_InputField roomTitleField;
    [SerializeField] Button passwordCopyButton;
    [SerializeField] Button createRoomButton;

    [SerializeField] int roomMaxUser = 2;
    [SerializeField] int roomPassword;
    [SerializeField] bool isCreatePassword = false;

    [Header("Map Select")]
    [SerializeField] Image mapImage;
    [SerializeField] TextMeshProUGUI mapTitle;
    [SerializeField] Button rightButton;
    [SerializeField] Button leftButton;
    [SerializeField] int currMapIndex = 0;

    private const int maxPasswordDigit = 8;

    private void InitCreateRoomInfo() 
    {
        // 비번 복사 버튼
        if (passwordCopyButton != null)
            passwordCopyButton.onClick.AddListener(CopyPassword);

        // 방 생성 버튼
        if (createRoomButton != null)
            createRoomButton.onClick.AddListener(() => 
            {
                RoominfoSetting();  // 1. 방 정보 세팅
                FusionCreateRoom(); // 2. 방 생성 (비번생성 후 생성 필요)
                ChangePanel(LobbyPanelType.CreateRoom, LobbyPanelType.WaitingRoom);  // 3. 대기룸으로
            });

        // 맵 타입 버튼
        if (rightButton != null)
            rightButton.onClick.AddListener(() => { ChangeMapIndex(++currMapIndex); });
        if (leftButton != null)
            leftButton.onClick.AddListener(() => { ChangeMapIndex(--currMapIndex); });

    }

    // 비밀번호 복사
    private void CopyPassword()
    {
        roomPassword = GetRandomNPassword();

        // 복사하기
        GUIUtility.systemCopyBuffer = roomPassword.ToString();

        isCreatePassword = true;
    }

    private void RoominfoSetting() 
    {
        PhotonRoomInfo.RoomName = roomTitleField.text;
        PhotonRoomInfo.MaxUser = roomMaxUser;
        PhotonRoomInfo.Password = roomPassword;;
        PhotonRoomInfo.MapTypeName = ((MapType)currMapIndex).ToString();
    }

    // 비밀번호 랜덤 생성
    private int GetRandomNPassword()
    {
        int min = (int)Mathf.Pow(10, maxPasswordDigit - 1);
        int max = (int)Mathf.Pow(10, maxPasswordDigit) - 1;
        return Random.Range(min, max + 1); // max는 포함되지 않으므로 +1
    }

    private void FusionCreateRoom() 
    {
        Debug.Log($"방 생성 시도 - RoomName: {PhotonRoomInfo.RoomName}");

        if (!isCreatePassword) 
        {
            Debug.Log("비밀 번호를 복사 해야합니다!");

            // ##TODO : 팝업 띄우기 

            return;
        }

        if (string.IsNullOrEmpty(PhotonRoomInfo.RoomName))
        {
            Debug.LogError("방 이름이 비어 있습니다!");

            // ##TODO : 팝업 띄우기 

            return;
        }

        // PunLobbyManager 매서드 실행
        PunLobbyManager.Instance.CreateCusomRoom();
    }

    private void ChangeMapIndex(int idx) 
    {
        if(idx < 0)
            idx = mapSprite.Length - 1;
        if (idx >= mapSprite.Length)
            idx = 0;

        currMapIndex = idx;

        mapImage.sprite = mapSprite[currMapIndex];
        mapTitle.text = Define.MapName[currMapIndex];
    }
}

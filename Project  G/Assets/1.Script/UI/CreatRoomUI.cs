using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// room을 만들 때 설정하는 데이터를 담아놓는 용도
public static class FusionRoomInfo
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
        FusionRoomInfo.RoomName = roomTitleField.text;
        FusionRoomInfo.MaxUser = roomMaxUser;
        FusionRoomInfo.Password = roomPassword;
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
        Debug.Log($"방 생성 시도 - RoomName: {FusionRoomInfo.RoomName}");

        if (!isCreatePassword) 
        {
            Debug.Log("비밀 번호를 생성해야합니다!");
            return;
        }

        if (string.IsNullOrEmpty(FusionRoomInfo.RoomName))
        {
            Debug.LogError("방 이름이 비어 있습니다!");
            return;
        }

        //FusionManager의 메서드 실행 
        FusionManager.GetInstance().CreateFusionRoom(GameMode.Host);
    }
}

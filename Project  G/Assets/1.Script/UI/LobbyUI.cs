using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class LobbyUIManager : MonoBehaviour
{
    [Space]
    [Header("===LobbyUi===")]
    [SerializeField] Button hostButton;
    [SerializeField] Button clientButton;
    [SerializeField] Button settinButton;
    [SerializeField] Button exitButton;
    [SerializeField] Button scoreButton;
    [SerializeField] Button scoreCloseButton;

    [SerializeField] GameObject scorePanel;
    [SerializeField] TextMeshProUGUI[] scoreText;

    [SerializeField] Image characterImage;
    [SerializeField] TextMeshProUGUI userNickNameText;

    private void InitLobbyUI()
    {
        // 방생성(호스트)
        if (hostButton != null)
            hostButton.onClick.AddListener(() => ChangePanel(LobbyPanelType.Lobby, LobbyPanelType.CreateRoom));

        // 방 참가(클라이언트)
        if (clientButton != null)
            clientButton.onClick.AddListener(() => ChangePanel(LobbyPanelType.Lobby, LobbyPanelType.RoomList));

        // 게임 나가기
        if (exitButton != null)
            exitButton.onClick.AddListener(() => Application.Quit());

        // 점수보기
        scoreButton.onClick.AddListener(() => 
        {
            scorePanel.SetActive(true);
            SetScoreText();
        } );

        // 점수판 끄기
        scoreCloseButton.onClick.AddListener(() => scorePanel.SetActive(false) ); ;
    }

    private void SetScoreText() 
    {
        Array type = System.Enum.GetValues(typeof(MapType));
        for(int i = 0; i < scoreText.Length; i++) 
        {
            MapType mapType = (MapType)type.GetValue(i);
            scoreText[i].text =
                UserDataManager.GetInstance().UserData.MapTypeToScore[mapType].ToString();
        }    
    }

    // 캐릭터 이미지 설정
    public void SettingProfile() 
    {
        int userIndex = (int)UserDataManager.GetInstance().UserData.UserAppearType;

        Array type = System.Enum.GetValues(typeof(CharacterType));
        if (userIndex < 0 || userIndex >= type.Length) return;

        Debug.Log(userIndex);

        characterImage.sprite = characterSprite[userIndex];

        userNickNameText.text = UserDataManager.GetInstance().UserData.NickName;

    }
}

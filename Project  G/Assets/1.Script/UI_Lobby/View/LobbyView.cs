using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyView : MonoBehaviour, ILocalizable
{
    [Header("===LobbyUi===")]
    [SerializeField] Button hostButton;
    [SerializeField] Button clientButton;
    [SerializeField] Button exitButton;
    [SerializeField] Button settinButton;
    [SerializeField] Button scoreButton;

    [SerializeField] GameObject scorePanel;
    [SerializeField] TextMeshProUGUI[] scoreTextList;

    [Header("===Localize Text===")]
    [SerializeField] TextMeshProUGUI createRoomText;
    [SerializeField] TextMeshProUGUI joinRoomTex;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI rankingText;
    [SerializeField] TextMeshProUGUI settingText;
    [SerializeField] TextMeshProUGUI exitText;

    private Action CreatHostRoomAction;
    private Action ClientJoinRoomAction;
    private Action ExitGameAction;
    private Action ScorePopUpAction;
    private Action SettingAction;

    private void Awake()
    {
        hostButton.onClick.AddListener(() => CreatHostRoomAction?.Invoke());
        clientButton.onClick.AddListener(() => ClientJoinRoomAction?.Invoke());
        exitButton.onClick.AddListener(() => ExitGameAction?.Invoke());
        scoreButton.onClick.AddListener(() => ScorePopUpAction?.Invoke());
        settinButton.onClick.AddListener(() => SettingAction?.Invoke());

    }

    public void RegisterCreateHostRoom(Action action) { CreatHostRoomAction += action;}
    public void RegisterClientJoinRoom(Action action) { ClientJoinRoomAction += action;}
    public void RegisterExitGame(Action action) {  ExitGameAction += action; }
    public void RegisterScorePopUp(Action action) { ScorePopUpAction += action; }
    public void RegisterSettingPopUp(Action action) { SettingAction += action;  }

    public void UpdateScoreText(int idx, float score) 
    {
        scoreTextList[idx].text = score.ToString();
    }

    private void OnEnable()
    {
        LocalizationManager.Instance.RegisterChangeLanguage(IUpdateLocalization);
    }

    public void IUpdateLocalization(LanguageType type)
    {
        createRoomText.text = LocalizationManager.Instance.ReturnLocalizationString(type, LocalizationKey.Lobby_CreateRoom);
        joinRoomTex.text = LocalizationManager.Instance.ReturnLocalizationString(type,LocalizationKey.Lobby_EnterRoom);
        scoreText.text = LocalizationManager.Instance.ReturnLocalizationString(type, LocalizationKey.Lobby_Score);
        rankingText.text = LocalizationManager.Instance.ReturnLocalizationString(type, LocalizationKey.Lobby_Ranking);
        settingText.text = LocalizationManager.Instance.ReturnLocalizationString(type, LocalizationKey.Lobby_Setting);
        exitText.text = LocalizationManager.Instance.ReturnLocalizationString(type, LocalizationKey.Lobby_Exit);
    }
}

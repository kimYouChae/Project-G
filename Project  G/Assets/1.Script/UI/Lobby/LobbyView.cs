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
    [SerializeField] Button characterSelectButton;
    [SerializeField] Button settinButton;
    [SerializeField] Button scoreButton;
    [SerializeField] Button achivementButton;
    [SerializeField] Button exitButton;

    [Header("===Localize Text===")]
    [SerializeField] TextMeshProUGUI createRoomText;
    [SerializeField] TextMeshProUGUI joinRoomTex;
    [SerializeField] TextMeshProUGUI characterText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI achivementText;
    [SerializeField] TextMeshProUGUI rankingText;
    [SerializeField] TextMeshProUGUI settingText;
    [SerializeField] TextMeshProUGUI exitText;

    private Action CreatHostRoomAction;
    private Action ClientJoinRoomAction;
    private Action CharacterSelectAction;
    private Action ScorePopUpAction;
    private Action SettingAction;
    private Action AchiveAction;
    private Action ExitGameAction;

    private void Awake()
    {
        hostButton.onClick.AddListener(() => CreatHostRoomAction?.Invoke());
        clientButton.onClick.AddListener(() => ClientJoinRoomAction?.Invoke());
        exitButton.onClick.AddListener(() => ExitGameAction?.Invoke());
        scoreButton.onClick.AddListener(() => ScorePopUpAction?.Invoke());
        settinButton.onClick.AddListener(() => SettingAction?.Invoke());
        achivementButton.onClick.AddListener(() => AchiveAction?.Invoke());
        characterSelectButton.onClick.AddListener(() => CharacterSelectAction?.Invoke());

    }

    public void RegisterCreateHostRoom(Action action) { CreatHostRoomAction += action;}
    public void RegisterClientJoinRoom(Action action) { ClientJoinRoomAction += action;}
    public void RegisterExitGame(Action action) {  ExitGameAction += action; }
    public void RegisterScorePopUp(Action action) { ScorePopUpAction += action; }
    public void RegisterSettingPopUp(Action action) { SettingAction += action;  }
    public void RegisterAchivePopup(Action action) {  AchiveAction += action;}
    public void RegisterCharacterSelectButton(Action action) { CharacterSelectAction += action;}

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
        characterText.text = LocalizationManager.Instance.ReturnLocalizationString(type, LocalizationKey.Character);
        achivementText.text = LocalizationManager.Instance.ReturnLocalizationString(type, LocalizationKey.Achievement);
    }
}

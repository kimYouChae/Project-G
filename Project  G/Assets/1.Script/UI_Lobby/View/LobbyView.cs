using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyView : MonoBehaviour
{
    [Header("===LobbyUi===")]
    [SerializeField] Button hostButton;
    [SerializeField] Button clientButton;
    [SerializeField] Button exitButton;
    [SerializeField] Button settinButton;
    [SerializeField] Button scoreButton;
    [SerializeField] Button scoreCloseButton;

    [SerializeField] GameObject scorePanel;
    [SerializeField] TextMeshProUGUI[] scoreText;

    private Action CreatHostRoomAction;
    private Action ClientJoinRoomAction;
    private Action ExitGameAction;
    private Action ScorePopUpAction;

    private void Awake()
    {
        hostButton.onClick.AddListener(() => CreatHostRoomAction?.Invoke());
        clientButton.onClick.AddListener(() => ClientJoinRoomAction?.Invoke());
        exitButton.onClick.AddListener(() => ExitGameAction?.Invoke());
        scoreButton.onClick.AddListener(() => ScorePopUpAction?.Invoke());

    }

    public void RegisterCreateHostRoom(Action action) { CreatHostRoomAction += action;}
    public void RegisterClientJoinRoom(Action action) { ClientJoinRoomAction += action;}
    public void RegisterExitGame(Action action) {  ExitGameAction += action; }
    public void RegisterScorePopUp(Action action) { ScorePopUpAction += action; }

    public void UpdateScoreText(int idx, float score) 
    {
        scoreText[idx].text = score.ToString();
    }
}

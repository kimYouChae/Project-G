using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyController : ILobbyPanelInitionlize
{
    private LobbyView lobbyView;

    public LobbyController(LobbyView lobbyView)
    {
        this.lobbyView = lobbyView;

        lobbyView.RegisterCreateHostRoom(CreateHostRoom);
        lobbyView.RegisterClientJoinRoom(JoinClientRoom);
        lobbyView.RegisterExitGame(ExitGame);
        lobbyView.RegisterScorePopUp(ScorePopUp);
        lobbyView.RegisterSettingPopUp(SettingPopUp);
    }

    private void CreateHostRoom() 
    {
        Debug.Log("CreateHostRoom");
        LobbyUIManager.Instance.ChangePanel(LobbyPanelType.Lobby, LobbyPanelType.CreateRoom); 
    }

    private void JoinClientRoom() 
    {
        LobbyUIManager.Instance.ChangePanel(LobbyPanelType.Lobby, LobbyPanelType.RoomList);
    }

    private void ExitGame() 
    {
        Application.Quit();
    }

    private void ScorePopUp() 
    {
        UserScorePopUP scorePopUp = UIManager.Instance.GetPopUP<UserScorePopUP>();
        scorePopUp.InitUserScorePopup();
    }

    private void SettingPopUp() 
    {
        SettingPopUP settingpopup = UIManager.Instance.GetPopUP<SettingPopUP>();

    }

    public void IInitPanel()
    {
        
    }
}

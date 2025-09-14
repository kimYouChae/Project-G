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
        // Score 팝업 띄우기 
        LobbyUIManager.Instance.OnOffPopUPPanel(true);
        UserScorePopUP scorePopUp = UIManager.Instance.GetPopUP<UserScorePopUP>();
        scorePopUp.InitUserScorePopup();
    }

    public void IInitPanel()
    {
        
    }
}

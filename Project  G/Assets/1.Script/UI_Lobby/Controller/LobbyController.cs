using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyController 
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
        LobbyUIManager.GetInstance().ChangePanel(LobbyPanelType.Lobby, LobbyPanelType.CreateRoom); 
    }

    private void JoinClientRoom() 
    {
        LobbyUIManager.GetInstance().ChangePanel(LobbyPanelType.Lobby, LobbyPanelType.RoomList);
    }

    private void ExitGame() 
    {
        Application.Quit();
    }

    private void ScorePopUp() 
    {
        // Score 팝업 띄우기 
        Debug.Log("Score PopUp 띄울 예정입니다");

        SetScoreText();
    }

    private void SetScoreText()
    {
        Array type = System.Enum.GetValues(typeof(MapType));

        for (int i = 0; i < type.Length; i++)
        {
            MapType mapType = (MapType)type.GetValue(i);

            lobbyView.UpdateScoreText(i, UserDataManager.Instance.UserData.MapTypeToScore[mapType]);
        }
    }
}

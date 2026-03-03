using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyController : ILobbyPanelInitionlize
{
    private LobbyView lobbyView;

    // 로비씬에서 도전과제 UI를 한 번이라도 열었는지 체크하는 플래그
    private bool hasOpenedAchievementUI = false;

    public LobbyController(LobbyView lobbyView)
    {
        this.lobbyView = lobbyView;

        lobbyView.RegisterCreateHostRoom(CreateHostRoom);
        lobbyView.RegisterClientJoinRoom(JoinClientRoom);
        lobbyView.RegisterExitGame(ExitGame);
        lobbyView.RegisterScorePopUp(ScorePopUp);
        lobbyView.RegisterSettingPopUp(SettingPopUp);
        lobbyView.RegisterAchivePopup(AchivePopup);
        lobbyView.RegisterCharacterSelectButton(CharacterPopUp);
        lobbyView.RegisterRankButton(Rank);
    }

    private void Rank() 
    {
        LeaderBoardPopUp leaderPopup = UIManager.Instance.GetPopUP<LeaderBoardPopUp>();
        leaderPopup.InitLeaderBoardPopUp();
    }

    private void CreateHostRoom() 
    {
        Debug.Log("CreateHostRoom");
        LobbyUIManager.Instance.ChangePanel(LobbyPanelType.Lobby, LobbyPanelType.CreateRoom); 
    }

    private void JoinClientRoom() 
    {
        LobbyUIManager.Instance.ChangePanel(LobbyPanelType.Lobby, LobbyPanelType.RoomList);

        // 로비에 입장
        PunLobbyManager.Instance.JoinLobby();
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

    private void AchivePopup() 
    {
        // 도전과제 팝업
        if (!hasOpenedAchievementUI)
            hasOpenedAchievementUI = true;

        AchivePopUP achive = UIManager.Instance.GetPopUP<AchivePopUP>();
        achive.InitAchivePopup(hasOpenedAchievementUI);
    }

    private void CharacterPopUp() 
    {
        // 캐릭터 선택 팝업
        if (!hasOpenedAchievementUI)
            hasOpenedAchievementUI = true;

        CharacterSelectPopUP chara = UIManager.Instance.GetPopUP<CharacterSelectPopUP>();
        chara.InitCharacterView(hasOpenedAchievementUI);
    }

    public void IInitPanel()
    {
        
    }
}

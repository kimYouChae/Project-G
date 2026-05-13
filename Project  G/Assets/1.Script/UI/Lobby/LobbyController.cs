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
        lobbyView.RegisterBuyCoffeeButton(BuyCoffee);
    }

    private void BuyCoffee() 
    {
        Application.OpenURL("https://ko-fi.com/youchea");
    }

    private void Rank() 
    {
        LeaderBoardPopUp leaderPopup = UIManager.Instance.GetPopUP<LeaderBoardPopUp>();
        leaderPopup.OpenLeaderBoardPopUp();
    }

    private void CreateHostRoom() 
    {
        Debug.Log("CreateHostRoom");

        // sfx 실행 
        SFXManager.Instance.PlaySFX(SFXType.UIClick);

        LobbyUIManager.Instance.ChangePanel(LobbyPanelType.Lobby, LobbyPanelType.CreateRoom); 
    }

    private void JoinClientRoom() 
    {
        LobbyUIManager.Instance.ChangePanel(LobbyPanelType.Lobby, LobbyPanelType.RoomList);

        // sfx 실행 
        SFXManager.Instance.PlaySFX(SFXType.UIClick);

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
        scorePopUp.OpenUserScorePopup();
    }

    private void SettingPopUp() 
    {
        SettingPopUP settingpopup = UIManager.Instance.GetPopUP<SettingPopUP>();
        settingpopup.OpenSetting();
    }

    private void AchivePopup() 
    {
        AchivePopUP achive = UIManager.Instance.GetPopUP<AchivePopUP>();
        achive.OpenAchivePopup(hasOpenedAchievementUI);

        // 도전과제 팝업
        if (!hasOpenedAchievementUI)
            hasOpenedAchievementUI = true;
    }

    private void CharacterPopUp() 
    {
        CharacterSelectPopUP chara = UIManager.Instance.GetPopUP<CharacterSelectPopUP>();
        chara.OpenCharacterView(hasOpenedAchievementUI);

        // 캐릭터 선택 팝업
        if (!hasOpenedAchievementUI)
            hasOpenedAchievementUI = true;
    }

    public void IInitPanel()
    {
        
    }
}

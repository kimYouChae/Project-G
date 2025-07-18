using System.Collections;
using System.Collections.Generic;
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

    private void InitLobbyUI() 
    {
        // 방생성(호스트)
        if(hostButton != null)
            hostButton.onClick.AddListener( ()=> ChangePanel(LobbyPanelType.Lobby, LobbyPanelType.CreateRoom));

        // 방 참가(클라이언트)
        if(clientButton != null) 
            clientButton.onClick.AddListener(() => ChangePanel(LobbyPanelType.Lobby, LobbyPanelType.RoomList));

        // 게임 나가기
        if(exitButton != null)
            exitButton.onClick.AddListener(() => Application.Quit());
    }


}

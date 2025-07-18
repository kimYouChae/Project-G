using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LobbyPanelType
{
    Title, NickName , Lobby, RoomList, CreateRoom , WaitingRoom
}

public partial class LobbyUIManager : MonoBehaviour
{
    [Header("---LobbyUIManager---")]
    [SerializeField] GameObject[] panelList;
    [SerializeField] LobbyPanelType prePanel;
    [SerializeField] LobbyPanelType currPanel;

    private void Awake()
    {
        // 다른 partial 클래스 초기화
        InitTitleUI();
        InitNickNameUI();
        InitLobbyUI();
        InitCreateRoomInfo();
    }


    // 패널 변경 
    public void ChangePanel(LobbyPanelType curr, LobbyPanelType next) 
    {
        prePanel = curr;
        currPanel = next;

        if (panelList[(int)prePanel].activeSelf)
            panelList[(int)prePanel].SetActive(false);

        if (!panelList[(int)currPanel].activeSelf)
            panelList[(int)currPanel].SetActive(true);
    }

}

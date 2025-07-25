
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum LobbyPanelType
{
    Title, NickName , Lobby, RoomList, CreateRoom , WaitingRoom
}

public partial class LobbyUIManager : MonoBehaviour
{
    // 싱글톤 
    private static LobbyUIManager instance;

    [Header("---LobbyUIManager---")]
    [SerializeField] GameObject[] panelList;
    [SerializeField] LobbyPanelType prePanel;
    [SerializeField] LobbyPanelType currPanel;

    public static LobbyUIManager GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("FusionManager 인스턴스가 존재하지 않습니다.");
            return null;
        }
        return instance;
    }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }

        // 다른 partial 클래스 초기화
        InitTitleUI();
        InitNickNameUI();
        InitLobbyUI();
        InitCreateRoomInfo();
        InitRoomListUi();
        InitWaitinRoomUI();
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

    // 리스트 비우기
    public void DestoryListObject(List<GameObject> list) 
    {
        for (int i = 0; i < list.Count; i++)
        {
            Destroy(list[i]);
        }
    }
}

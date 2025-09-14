
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class LobbyUIManager : MonoBehaviour
{
    // 싱글톤 
    private static LobbyUIManager instance;

    [Header("---LobbyUIManager---")]
    [SerializeField] private GameObject[] panelList;
    [SerializeField] private LobbyPanelType prePanel;
    [SerializeField] private LobbyPanelType currPanel;

    [SerializeField] private GameObject popupPanel;

    [Header("===Controller===")]
    private NickNameController nickNameController;
    private RoomListController roomListController;
    private CreateRoomController createRoomController;
    private WaitingRoomController waitingRoomController;
    private LobbyController lobbyController;

    [Header("===View===")]
    private NickNameView nickNameView;
    private RoomListView roomListView;
    private CreateRoomView createRoomView;
    private WaitingRoomView waitingRoomView;
    private LobbyView lobbyView;

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
    }

    private void Start()
    {
        // NickName MVC 
        nickNameView = GetComponent<NickNameView>();
        NickNameModel nickNameModel = new NickNameModel();
        nickNameController = new NickNameController(nickNameView, nickNameModel);

        // RoomList MVC
        roomListView = GetComponent<RoomListView>();
        RoomListModel roomListModel = new RoomListModel();
        roomListController = new RoomListController(roomListView, roomListModel);

        // Creat Room MVC
        createRoomView = GetComponent<CreateRoomView>();
        CreateRoomModel createRoomModel = new CreateRoomModel();
        createRoomController = new CreateRoomController(createRoomView, createRoomModel);

        // Waiting Room MVC
        waitingRoomView = GetComponent<WaitingRoomView>();
        waitingRoomController = new WaitingRoomController(waitingRoomView);

        // Lobby MVC
        lobbyView = GetComponent<LobbyView>();
        lobbyController = new LobbyController(lobbyView);
    }

    #region 외부에서 view를 수정
    public void UpdateWaitinRoomView(Player[] playerref) 
    {
        waitingRoomView.UpdateWaitingRoomInfo(playerref);
    }

    public void UpdateRoomListView() 
    {
        roomListView.UpdateRoomList();
    }

    #endregion

    // 패널 변경 
    public void ChangePanel(LobbyPanelType curr, LobbyPanelType next) 
    {
        prePanel = curr;
        currPanel = next;

        if (curr == LobbyPanelType.None)
        {
            // 다 끄기
            OffAllPanel();
        }
        else 
        {
            if (panelList[(int)prePanel].activeSelf)
                panelList[(int)prePanel].SetActive(false);
        }

        if (!panelList[(int)currPanel].activeSelf)
        {
            // 패널 켜기 
            panelList[(int)currPanel].SetActive(true);

            // 해당하는 controller의 Init실행하기
            var init = TypeByController(currPanel);
            if (init != null)
                init.IInitPanel();
        }  
    }

    // 리스트 비우기
    public void DestoryListObject(List<GameObject> list) 
    {
        for (int i = 0; i < list.Count; i++)
        {
            Destroy(list[i]);
        }
    }

    private void OffAllPanel() 
    {
        for(int i = 0; i < panelList.Length; i++) 
        {
            panelList[i].SetActive(false);
        }
    }

    public void OnOffPopUPPanel(bool flag) 
    {
        popupPanel.SetActive(flag);
    }

    private ILobbyPanelInitionlize TypeByController(LobbyPanelType type)
    { 
        switch(type) 
        {
            case LobbyPanelType.NickName: return nickNameController;
            case LobbyPanelType.Lobby: return lobbyController;
            case LobbyPanelType.RoomList: return roomListController;
            case LobbyPanelType.CreateRoom : return createRoomController;
            case LobbyPanelType.WaitingRoom : return waitingRoomController;
        }

        return null;
    }
}

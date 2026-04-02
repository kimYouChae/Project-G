
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;


public class LobbyUIManager : Singleton<LobbyUIManager>
{
    [Header("---LobbyUIManager---")]
    [SerializeField] private GameObject[] panelList;
    [SerializeField] private LobbyPanelType prePanel;
    [SerializeField] private LobbyPanelType currPanel;

    [SerializeField] private GameObject darkPanel;

    [Header("===Controller===")]
    private RoomListController roomListController;
    private CreateRoomController createRoomController;
    private WaitingRoomController waitingRoomController;
    private LobbyController lobbyController;

    [Header("===View===")]
    private RoomListView roomListView;
    private CreateRoomView createRoomView;
    private WaitingRoomView waitingRoomView;
    private LobbyView lobbyView;

    protected override void Singleton_Awake()
    {
        InitMVCController();
    }

    private void InitMVCController()
    {
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

        // POPUP 관련 MVC
        // Setting MVC
        SettingPopUP settingpopup = UIManager.Instance.GetPopUP<SettingPopUP>();
        settingpopup.gameObject.SetActive(false);

        AudioMixer mixer = ResourceManager.Instance.GetAudioMixer;
        SoundModel soundModel = new SoundModel(mixer);
        SettingController settingCT = new SettingController(soundModel, settingpopup);
    }

    #region 외부에서 view를 수정
    public void UpdateWaitinRoomView(Player[] playerref)
    {
        if (waitingRoomView == null)
        {
            Debug.Log("포톤의 플레어어 배열이 NULL 입니다");
            return;
        }

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

        // 이전 panel
        if (prePanel != LobbyPanelType.None && panelList[(int)prePanel].activeSelf) 
        {
            panelList[(int)prePanel].SetActive(false);
        }

        // 현재 panel
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

    private ILobbyPanelInitionlize TypeByController(LobbyPanelType type)
    { 
        switch(type) 
        {
            case LobbyPanelType.Lobby: return lobbyController;
            case LobbyPanelType.RoomList: return roomListController;
            case LobbyPanelType.CreateRoom : return createRoomController;
            case LobbyPanelType.WaitingRoom : return waitingRoomController;
        }

        return null;
    }

    public void OnOffDarkPanel(bool flag) 
    {
        darkPanel.SetActive(true);
    }
}

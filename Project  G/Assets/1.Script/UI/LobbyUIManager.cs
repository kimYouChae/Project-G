
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public partial class LobbyUIManager : MonoBehaviour
{
    // 싱글톤 
    private static LobbyUIManager instance;

    [Header("---LobbyUIManager---")]
    [SerializeField] private GameObject[] panelList;
    [SerializeField] private LobbyPanelType prePanel;
    [SerializeField] private LobbyPanelType currPanel;
    [SerializeField] private Sprite[] characterSprite;

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
        // 다른 partial 클래스 초기화
        InitTitleUI();
        InitNickNameUI();
        InitCharacterSelectUI();
        InitLobbyUI();
        InitCreateRoomInfo();
        InitRoomListUi();
        InitWaitinRoomUI();
        InitUnTitledUI();
    }


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

    private void OffAllPanel() 
    {
        for(int i = 0; i < panelList.Length; i++) 
        {
            panelList[i].SetActive(false);
        }
    }
}

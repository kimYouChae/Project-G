using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class LobbyUIManager : MonoBehaviour
{
    /// <summary>
    /// 1. 방 새로고침
    /// 2. 방 생성
    /// 3. 방 참가
    /// 4. 비밀번호 입력 , 검사 
    /// </summary>

    [Space]
    [Header("===RoomListUi===")]
    [SerializeField] Button refreshRoomListButton;
    [SerializeField] int currSelectRoomIndex = -1;
    [SerializeField] Button joinRoomButton;
    [SerializeField] TMP_InputField passWordText;

    [SerializeField] GameObject roomInfoPrefab;
    [SerializeField] List<GameObject> roomObjList;

    [SerializeField] GameObject content;    // 스크롤뷰의 콘텐츠

    private void InitRoomListUi() 
    {
        // 방 새로고침 버튼 
        refreshRoomListButton.onClick.AddListener( ()=> 
        {
            RefreshRoomList();
            UpdateRoomList();
        });

        // 방 참여 버튼
        joinRoomButton.onClick.AddListener( ()=> 
        {
            JoinRoom();
        });
    }

    private void RefreshRoomList() 
    {
        
    }

    private void UpdateRoomList() 
    {
        // 방은 몇개 없으니까 그냥 생성 + 파괴 해도될듯 ? 
        DestoryListObject(roomObjList);

        roomObjList.Clear();

        /*
        // 다시 룸(세선) 정보로 생성 
        for (int i = 0; i < FusionLobbyManager.GetInstance().SessionInfoLists.Count; i++) 
        {
            GameObject temp = Instantiate(roomInfoPrefab);
            temp.transform.SetParent(content.transform);

            roomObjList.Add(temp);

            // 제목 설정 
            TextMeshProUGUI roomTitle = temp.GetComponentInChildren<TextMeshProUGUI>();
            if(roomTitle != null ) 
            {
                roomTitle.text = FusionLobbyManager.GetInstance().SessionInfoLists[i].Name;
            }

            // 인덱스 설정
            RoomInfoObject infoObj = temp.GetComponentInChildren<RoomInfoObject>();
            if(infoObj != null) 
            {
                infoObj.RoomObjectIndex = i;
            }
        }
        */
    }

    private void JoinRoom() 
    {
        /*
        if (currSelectRoomIndex < 0 && currSelectRoomIndex >= FusionLobbyManager.GetInstance().SessionInfoLists.Count)
            return;

        EnterPassWord();
        */
    }


    // 선택 RoomIndex 세팅
    public void SettingRoomIndex(int index) 
    {
        this.currSelectRoomIndex = index;
    }

    private void EnterPassWord() 
    {
        // 비번 입력받기
        string inputPassword = passWordText.text;

        /*
        // 선택한 방
        SessionInfo info = FusionLobbyManager.GetInstance().SessionInfoLists[currSelectRoomIndex];
        if (info == null)
            return;

        SessionProperty value;
        // 비번이 없으면 return
        if (!info.Properties.TryGetValue("Password", out value))
        {
            Debug.Log("해당 Room에 비밀번호가 존재하지 않습니다!");
            return;
        }
        if (inputPassword.Equals(string.Empty))
            return;

        int roomPassword = (int)value;

        // 같으면 
        if (int.Parse(inputPassword) == roomPassword)
        {
            Debug.Log("올바른 비밀번호를 입력 했습니다! 방에 입장 합니다");

            // 방 참가 시도 
            FusionLobbyManager.GetInstance().JoinFusionRoom(info.Name);

            // panel 변경 
            ChangePanel(LobbyPanelType.RoomList, LobbyPanelType.WaitingRoom);
        }
        else
        {
            Debug.Log("비밀번호가 다릅니다! ");
        }
        */
    }

}

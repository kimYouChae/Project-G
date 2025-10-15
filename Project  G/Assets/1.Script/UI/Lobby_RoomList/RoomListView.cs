using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomListView : MonoBehaviour, ILocalizable
{
    [Header("===RoomListUi===")]
    [SerializeField] Button refreshRoomListButton;
    [SerializeField] Button joinRoomButton;
    [SerializeField] TMP_InputField passWordText;

    [SerializeField] GameObject roomInfoPrefab;     // ##TODO 나중에 Resource에서 가져오는걸로 바꿔보기 
    [SerializeField] List<GameObject> roomObjList;

    [SerializeField] GameObject content;    // 스크롤뷰의 콘텐츠
    [SerializeField] Button backButton;     // 뒤로가기 버튼 

    [Header("===Localize Text===")]
    [SerializeField] TextMeshProUGUI refreshText;
    [SerializeField] TextMeshProUGUI inputPasswordText;
    [SerializeField] TextMeshProUGUI enterRoomText;

    private Action RefreshRoomListAction;
    private Action<string> JoinRoomAction;
    private Action<int> SelectRoomIndex;
    private Action backButtonAction;

    private void Awake()
    {
        refreshRoomListButton.onClick.AddListener(() => RefreshRoomListAction?.Invoke());
        backButton.onClick.AddListener(() => backButtonAction?.Invoke());

        // 입력받은 passWord를 연결된 모든 메서드에게 넘겨주기
        joinRoomButton.onClick.AddListener(() => JoinRoomAction?.Invoke(passWordText.text));
    }

    public void RegisterRefreshRoom(Action action) { RefreshRoomListAction += action; }
    public void RegisterJoinRoom(Action<string> action) { JoinRoomAction += action; }
    public void RegisterSelectRoomIndex(Action<int> action) { SelectRoomIndex += action; }
    public void RegisterBackButton(Action action) { backButtonAction += action; }

    public void NotifySelectRoomIndex(int index)
    {
        // RoomInfoObject에서 선택한 index를 매개변수로, 연결된 메서드 실행 
        SelectRoomIndex?.Invoke(index);
    }

    public void UpdateRoomList()
    {
        // 방은 몇개 없으니까 그냥 생성 + 파괴 해도될듯 ? 
        LobbyUIManager.Instance.DestoryListObject(roomObjList);

        roomObjList.Clear();

        // 다시 룸(세선) 정보로 생성 
        for (int i = 0; i < PunLobbyManager.Instance.RoomInfoList.Count; i++)
        {
            RoomInfo roomInfo = PunLobbyManager.Instance.RoomInfoList[i];

            // 오브젝트 생성
            GameObject temp = Instantiate(roomInfoPrefab);
            temp.transform.SetParent(content.transform);

            roomObjList.Add(temp);

            // 제목 설정 
            TextMeshProUGUI roomTitle = temp.GetComponentInChildren<TextMeshProUGUI>();
            if (roomTitle != null)
            {
                roomTitle.text = roomInfo.Name;
            }

            // 인덱스 설정
            RoomInfoObject infoObj = temp.GetComponentInChildren<RoomInfoObject>();
            if (infoObj != null)
            {
                infoObj.RoomObjectIndex = i;
                infoObj.roomListView = this;
            }
        }

    }

    private void OnEnable()
    {
        LocalizationManager.Instance.RegisterChangeLanguage(IUpdateLocalization);
    }

    public void IUpdateLocalization(LanguageType type)
    {
        refreshText.text = LocalizationManager.Instance.ReturnLocalizationString(type, LocalizationKey.RoomList_Refresh);
        inputPasswordText.text = LocalizationManager.Instance.ReturnLocalizationString(type, LocalizationKey.RoomList_InputPassword);
        enterRoomText.text = LocalizationManager.Instance.ReturnLocalizationString(type, LocalizationKey.RoomList_EnterRoom);
    }
}

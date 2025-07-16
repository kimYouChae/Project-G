using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MapType 
{
    None,
    Basic
}
public enum ModeType 
{
    Node,
    Fight
}

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private Button hostRoomButton; // 호스트 버튼 (방 생성)
    [SerializeField] private Button joinRoomButton;   // 방 참가 버튼 
    [SerializeField] private Button createRoomButton;  // 방 생성 버튼 

    [SerializeField] private GameObject createRoomUi; // 방생성 UI
    [SerializeField] private GameObject roomListUi;   // 룸 리스트 UI

    [Header("방 생성")]
    [SerializeField] private MapType mapType = MapType.Basic;   // 맵 타입
    [SerializeField] private string roomTitle;  // 방 이름
    [SerializeField] private int clientCount = 2;   // 참여 인원수
    [SerializeField] private ModeType modeType = ModeType.Fight;     // 모드 타입
    [SerializeField] private int roomPassword;      // 방 비번 (랜덤)

    private void Awake()
    {
        // 방생성 버튼 -> 방 생성 UI 켜기
        hostRoomButton.onClick.AddListener(() => OnCreateRoomUI(true));
        // 방 Join 버튼 -> 룸 목록 UI 켜기
        joinRoomButton.onClick.AddListener( () => OnRoolListUi(false));
        // 방 생성 버튼 -> room 정보 바탕으로 방 생성
        createRoomButton.onClick.AddListener (RoomInfoSetting);
    }

    private void OnCreateRoomUI(bool flag)
    {
        createRoomUi.SetActive(flag);
    }

    private void OnRoolListUi(bool flag) 
    {
        roomListUi.SetActive(flag);        
    }

    // 방 생성하기 
    private void RoomInfoSetting() 
    {
                   
    }
}

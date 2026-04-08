using Photon.Pun;
using Photon.Realtime;
using Steamworks;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaitingRoomView : MonoBehaviour, ILocalizable
{
    [Header("===WaitingUi===")]
    [SerializeField] TextMeshProUGUI roomTitle;
    [SerializeField] GameObject playeRefObject;
    [SerializeField] GameObject scrollViewContent;

    [SerializeField] List<GameObject> playerRefObj;

    [SerializeField] Button openFriendButton;   // 친구 팝업 버튼
    [SerializeField] Button gameStartButton;    // 게임시작 버튼
    [SerializeField] Button backButton;         // 뒤로가기 버튼 

    [Header("===Localize Text===")]
    [SerializeField] TextMeshProUGUI gameStartText;

    private Action GetFriendAction;
    private Action GameStartAction;
    private Action backButtonAction;

    private void Awake()
    {
        gameStartButton.onClick.AddListener(()=>GameStartAction?.Invoke());
        backButton.onClick.AddListener(() => backButtonAction?.Invoke());
        openFriendButton.onClick.AddListener(() => GetFriendAction?.Invoke());
    }

    public void RegisterGetFriend(Action action) { GetFriendAction += action; }
    public void RegisterGameStart(Action action) { GameStartAction += action; }
    public void RegisterBackButton(Action action) { backButtonAction += action; }

    public void UpdateWaitingRoomInfo(Player[] playerref)
    {
        // 현재 방 대한 정보를 가져옴
        Room info = PhotonNetwork.CurrentRoom;

        // 방제 업데이트
        object roomName;
        info.CustomProperties.TryGetValue("RoomName", out roomName);
        roomTitle.text = (string)roomName;

        // 리스트 초기화
        LobbyUIManager.Instance.DestoryListObject(playerRefObj);

        for (int i = 0; i < playerref.Length; i++)
        {
            var temp = Instantiate(playeRefObject).GetComponent<PlayerRefObject>();
            temp.transform.SetParent(scrollViewContent.transform, false);

            // 자식중 컴포넌트 찾기 
            Image image = temp.profileImage;
            TextMeshProUGUI text = temp.nicknameText;

            // 닉네임 설정
            text.text = playerref[i].NickName;
            // 프로필 세팅 
            object steamIdObj;
            if (playerref[i].CustomProperties.TryGetValue("SteamId", out steamIdObj))
            {
                string steamId = (string)steamIdObj;
                CSteamID cSteam = new CSteamID( ulong.Parse(steamId));
                Texture2D texture =SteamScript.Instance.ReturnProfileTexureByCSteamID(cSteam);
                image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
            else
            {
                Debug.LogWarning("SteamId 없음");
            }
            playerRefObj.Add(temp.gameObject);
        }
    }

    private void OnEnable()
    {
        LocalizationManager.Instance.RegisterChangeLanguage(IUpdateLocalization);
    }

    public void IUpdateLocalization(LanguageType type)
    {
        gameStartText.text = LocalizationManager.Instance.ReturnLocalizationString(type, LocalizationKey.Waiting_StartGame);
    }
}

using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
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

    [SerializeField] Button gameStartButton;
    [SerializeField] Button backButton;     // 뒤로가기 버튼 

    [Header("===Localize Text===")]
    [SerializeField] TextMeshProUGUI gameStartText;

    private Action GameStartAction;
    private Action backButtonAction;

    private void Awake()
    {
        gameStartButton.onClick.AddListener(()=>GameStartAction?.Invoke());
        backButton.onClick.AddListener(() => backButtonAction?.Invoke());
    }

    public void RegisterGameStart(Action action) { GameStartAction += action; }
    public void RegisterBackButton(Action action) { backButtonAction += action; }

    public void UpdateWaitingRoomInfo(Player[] playerref)
    {
        // 현재 방 대한 정보를 가져옴
        Room info = PhotonNetwork.CurrentRoom;

        // 방제 업데이트
        roomTitle.text = info.Name;

        // 리스트 초기화
        LobbyUIManager.Instance.DestoryListObject(playerRefObj);

        for (int i = 0; i < playerref.Length; i++)
        {
            GameObject temp = Instantiate(playeRefObject);
            TextMeshProUGUI text = temp.GetComponentInChildren<TextMeshProUGUI>();
            text.text = playerref[i].NickName;

            playerRefObj.Add(temp);

            temp.transform.SetParent(scrollViewContent.transform);
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

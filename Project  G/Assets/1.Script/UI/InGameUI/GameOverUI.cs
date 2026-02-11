using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [Space]
    [Header("===Header===")]
    [SerializeField] private TextMeshProUGUI timeHeadling;  // "생존시간" 텍스트
    [SerializeField] private TextMeshProUGUI scoreHeading;  // "점수" 텍스트

    [Header("===numberText")]
    [SerializeField] private TextMeshProUGUI gameOverScoreText; // 점수(숫자) 텍스트
    [SerializeField] private TextMeshProUGUI gameOverTimeText;  // 시간(숫자) 텍스트

    [Header("===other===")]
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private Button backToRoom;
    [SerializeField] private TextMeshProUGUI isnotMasterText;

    private void Awake()
    {
        backToRoom.onClick.AddListener(BacktoLobbyRoom);
    }

    public void GameOverText(float score, bool isUpdated, float time) 
    {
        gameOverScoreText.text = (Math.Round(score, 2)).ToString();
        gameOverTimeText.text = (Math.Round(time, 2)).ToString();

        if (isUpdated) 
        {
            // 최고점수일때만 
            // !최고점수입니다! 켜기 
            bestScoreText.text = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.InGame_Renewal);
        }

        // 마스터 클라이언트만 버튼 ON
        if (PhotonNetwork.IsMasterClient)
        {
            backToRoom.gameObject.SetActive(true);
            isnotMasterText.text = "";
        }
        else 
        {
            backToRoom.gameObject.SetActive(false);
            isnotMasterText.text = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.InGame_Waiting); ;
        }
    }

    public void BacktoLobbyRoom() 
    {
        PhotonSceneManager.Instance.ChangeScene(SceneType.Lobby);
    }

}

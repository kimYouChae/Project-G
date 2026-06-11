using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [Space]
    [Header("===Context===")]
    [SerializeField] private GameObject gameOverTextContent;
    [SerializeField] private TextMeshProUGUI apiLoadingText;

    [Header("===Header===")]
    [SerializeField] private TextMeshProUGUI scoreHeading;  // "점수" 텍스트
    [SerializeField] private TextMeshProUGUI timeHeadling;  // "생존시간" 텍스트

    [Header("===numberText")]
    [SerializeField] private TextMeshProUGUI achiveScoreText;       // 점수(숫자) 텍스트
    [SerializeField] private TextMeshProUGUI achiveTimeText;       // 시간(숫자) 텍스트
   
    [Header("===other===")]
    [SerializeField] private TextMeshProUGUI bestScoreHeading;      // 최고 점수 달성 여부 텍스트
    [SerializeField] private Button backToLobby;         // 로비로 돌아가는 버튼
    [SerializeField] private TextMeshProUGUI backToLobbyText;   // "로비로 돌아갑니다" 텍스트
    [SerializeField] private TextMeshProUGUI isnotMasterText;       // "마스터가 아닙니다" 텍스트 

    private void Awake()
    {
        backToLobby.onClick.AddListener(BacktoLobbyRoom);

        // 시간,점수 로컬라이징
        scoreHeading.text = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.InGame_Score);
        timeHeadling.text = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.InGame_Time);
    }

    public void OnOffGameContextTexts(bool flag) 
    {
        // flase 이면 
        if (!flag)
        {
            // context 끄기
            gameOverTextContent.SetActive(false);

            // api 로딩 텍스트
            // "데이터 로딩 하는 중"
            apiLoadingText.gameObject.SetActive(true);
            apiLoadingText.text 
                = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.Saving_In_Progress);

            return;
        }

        gameOverTextContent.SetActive(true);
        apiLoadingText.gameObject.SetActive(false);
    }

    public void GameOverTextOffline(float score, float time) 
    {
        achiveScoreText.text = score.ToString();
        achiveTimeText.text = time.ToString();

        bestScoreHeading.text 
            = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.GameOver_Offline);

        ReturnToLobbyButton();
    }

    public void GameOverText(float score, float time, bool isUpdated) 
    {
        achiveScoreText.text = score.ToString();
        achiveTimeText.text = time.ToString();

        if (isUpdated)
        {
            // 최고점수일때만 
            // " 최고 점수 달성 ! "
            bestScoreHeading.text = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.InGame_Renewal);
        }
        else 
        {
            // 최고 점수가 아니면 
            // " 최고 점수 달성에 실패"
            bestScoreHeading.text = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.GameOver_BestScoreUpdateFailed);
        }

        ReturnToLobbyButton();
    }

    private void ReturnToLobbyButton() 
    {
        // 마스터 클라이언트만 버튼 ON
        if (PhotonNetwork.IsMasterClient)
        {
            backToLobby.gameObject.SetActive(true);
            isnotMasterText.text = "";
            backToLobbyText.text = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.GameOver_ReturnToLobby); ;
        }
        else
        {
            backToLobby.gameObject.SetActive(false);
            isnotMasterText.text = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.InGame_Waiting); ;
        }
    }

    public void BacktoLobbyRoom() 
    {
        PhotonSceneManager.Instance.ChangeScene(SceneType.Lobby);
    }

}

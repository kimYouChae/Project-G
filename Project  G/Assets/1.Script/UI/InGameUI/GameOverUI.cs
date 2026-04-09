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
    [SerializeField] private TextMeshProUGUI achiveScoreText;       // 점수(숫자) 텍스트
    [SerializeField] private TextMeshProUGUI achiveStageText;       // 시간(숫자) 텍스트
   
    [Header("===other===")]
    [SerializeField] private TextMeshProUGUI bestScoreHeading;      // 최고 점수 달성 여부 텍스트
    [SerializeField] private Button backToRoom;
    [SerializeField] private TextMeshProUGUI isnotMasterText;       // "마스터가 아닙니다" 텍스트 

    private void Awake()
    {
        backToRoom.onClick.AddListener(BacktoLobbyRoom);
    }

    public void GameOverText(float score, float time, bool isUpdated) 
    {
        achiveScoreText.text = score.ToString();
        achiveStageText.text = time.ToString();

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
            bestScoreHeading.text = "(로컬라이징 전) 최고점수 갱신에 실패";
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

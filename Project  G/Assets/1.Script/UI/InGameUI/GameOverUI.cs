using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class InGameUI : MonoBehaviour
{
    [Space]
    [Header("===GameOverUI===")]
    [SerializeField] private TextMeshProUGUI gameOverScoreText;
    [SerializeField] private TextMeshProUGUI gameOverTimeText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private Button backToRoom;
    [SerializeField] private TextMeshProUGUI isnotMasterText;

    const string BestScoreText = "! 최고 기록 갱신 !";
    const string isNotMatsterText = "호스트가 버튼을 누를 때 까지 기다려주세요!";

    private void InitGameOverUI() 
    {
        backToRoom.onClick.AddListener(BacktoLobbyRoom);
    }

    public void GameOverText(float score, float time, bool bestScore = false) 
    {
        gameOverScoreText.text = (Math.Round(score, 2)).ToString();
        gameOverTimeText.text = (Math.Round(time, 2)).ToString();

        if (bestScore) 
        {
            bestScoreText.text = BestScoreText;
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
            isnotMasterText.text = isNotMatsterText;
        }
    }

    public void BacktoLobbyRoom() 
    {
        PhotonSceneManager.Instance.ChangeScene(SceneType.Lobby);
    }

}

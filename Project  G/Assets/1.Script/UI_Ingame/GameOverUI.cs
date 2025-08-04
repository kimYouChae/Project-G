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

    const string BestScoreText = "! 최고 기록 갱신 !";

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

    }

    public void BacktoLobbyRoom() 
    {
        PhotonSceneManager.Instance.ChangeScene(SceneType.Lobby);
    }

}

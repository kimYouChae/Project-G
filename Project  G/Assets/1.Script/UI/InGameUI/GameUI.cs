using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using System.Text;
using System;

public class GameUI : MonoBehaviour
{
    [Space]
    [Header("===GameUI===")]
    [Header("플레이어정보")]
    // 사분면을 지켜서 순서대로 담아둬야함 !
    [SerializeField] TextMeshProUGUI[] playerInfoText;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI scoreText; // 점수(숫자)텍스트
    [SerializeField] TextMeshProUGUI timeText;  // 시간(숫자)텍스트
    [SerializeField] TextMeshProUGUI levelText; // 레벨(숫자)텍스트

    [SerializeField] TextMeshProUGUI scoreHeadling; // "점수"텍스트
    [SerializeField] TextMeshProUGUI timeHeadling;  // "시간"텍스트
    [SerializeField] TextMeshProUGUI levelHeadling; // "레벨"텍스트

    private void Start()
    {
        scoreHeadling.text = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.InGame_Score);
        timeHeadling.text = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.InGame_Time);
        levelHeadling.text = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.InGame_Level);
    }

    public void UpdatePlayerInfoText(InGamePlayer player) 
    {
        int actorNum = player.ActorNum;
        string nick = player.NickName;
        float score = player.Score;

        // 로컬 플레이어의 사분면 위치에 해당하는 TMP
        TextMeshProUGUI tmp = playerInfoText[actorNum];

        StringBuilder sb = new StringBuilder();
        sb.Append($"{LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.InGame_Nick)} {nick} \n");

        // 현재 타입에 해당하는 맵 타입
        // 현재 방 정보의 커스텀 정보에 접근 (hashTable에서 matType검사)
        MapType type = PunIngameManager.Instance.GetMapType();
        sb.Append($"{LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.InGame_BestScore)} {score} \n");

        tmp.text = sb.ToString();

        Debug.Log(tmp);
    }

    public void UpdateTimeText(float time) 
    {
        timeText.text = Math.Round(time, 2).ToString();
    }

    public void UpdateScoreText(float score) 
    {
        scoreText.text = Math.Round(score, 2).ToString();
    }

    public void UpdateLevelText(int level) 
    {
        levelText.text = level.ToString();
    }
    
}

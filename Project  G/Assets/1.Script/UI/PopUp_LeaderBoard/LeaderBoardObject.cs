using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardObject : MonoBehaviour
{
    [SerializeField] Image rankIcon;            // 1,2,3등 아이콘 , 이외는 x
    [SerializeField] TextMeshProUGUI rankText;  // 랭크
    [SerializeField] TextMeshProUGUI playerNamesText;   // 유저 1 닉네임 | 유저 2 닉네임 
    [SerializeField] TextMeshProUGUI scoreText; // 점수 | 스테이지 

    public void UpdateLeaderBoard(Sprite image, string rText, string names, string scoreAndStage) 
    {
        if(image != null)
            rankIcon.sprite = image;    
        if(rText != string.Empty) 
            rankText.text = rText;
        playerNamesText.text = names;
        scoreText.text = scoreAndStage;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardObject : MonoBehaviour
{
    [SerializeField] Image rankIcon;
    [SerializeField] TextMeshProUGUI rankText;
    [SerializeField] TextMeshProUGUI playerNamesText;
    [SerializeField] TextMeshProUGUI scoreText;

    public void UpdateLeaderBoard(Sprite image, string rText, string names, string score) 
    {
        rankIcon.sprite = image;
        rankText.text = rText;
        playerNamesText.text = names;
        scoreText.text = score;
    }
}

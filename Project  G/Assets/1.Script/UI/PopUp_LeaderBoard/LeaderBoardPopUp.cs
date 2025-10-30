using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderBoardPopUp : UIPopUP
{
    [SerializeField] Sprite[] rankingIcon;   // 1~3위 아이콘 배경 + 나머지 아이콘 
    [SerializeField] GameObject leaderObject;   // 리더보드 오브젝트 
    [SerializeField] List<LeaderBoardObject> lbjList;

    [Header("===Localization===")]
    [SerializeField] TextMeshProUGUI leaderBoardText;



}

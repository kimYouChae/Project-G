using BackEnd.Leaderboard;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardPopUp : UIPopUP
{
    [Header("===Container===")]
    [SerializeField] Sprite[] rankingIcon;   // 1~3위 아이콘 배경 + 나머지 아이콘 
    [SerializeField] GameObject leaderObject;   // 리더보드 오브젝트 
    [SerializeField] List<LeaderBoardObject> lbjList;

    [Header("===Conponent===")]
    [SerializeField] Transform content;     // 오브젝트 상위 부모 

    [Header("===Button===")]
    [SerializeField] Button rightButton;
    [SerializeField] Button leftButton;

    [Header("===Data===")]
    [SerializeField] int rankStartIndex;        // 조회할 랭크 시작점 

    [Header("===Localization===")]
    [SerializeField] TextMeshProUGUI leaderBoardText;

    const int MAX_BOARD_PLAYER = 5;

    private void InstantiateLBObject()
    {
        // 처음 1회 생성
        for (int i = 0; i < MAX_BOARD_PLAYER; i++) 
        {
            GameObject temp = Instantiate(leaderObject);
            temp.transform.SetParent(content, false);

            LeaderBoardObject lobj = temp.GetComponent<LeaderBoardObject>();
            lbjList.Add(lobj);
        }
    }

    // On 될 때 마다 업데이트 
    public void InitLeaderBoardPopUp() 
    {
        // 타이틀 로컬라이징
        // leaderBoardText.text = LocalizationManager.Instance.ReturnLocalizationString();

        // 리스트에 없으면 -> 1회 오픈, 새로 생성
        if (lbjList.Count <= 0)
        {
            InstantiateLBObject();
        }

        rankStartIndex = 0;
        UpdateLeadrBoard(rankStartIndex);
    }

    private void UpdateLeadrBoard(int rankStartIdx) 
    {
        List<UserLeaderboardItem> userItems
            = BackEndLeaderBoardManager.Instance.GetRanking(MapType.Forest, MAX_BOARD_PLAYER, rankStartIdx);

        for(int i = 0; i < lbjList.Count; i++) 
        {
            LeaderBoardObject leaderboard = lbjList[i];
            UserLeaderboardItem userItem = userItems[i];

            // 랭킹에 따른 아이콘 설정
            int rank = int.Parse(userItem.rank);
            Sprite rankIcon;
            if (rank == 1) rankIcon = rankingIcon[0];
            else if (rank == 2) rankIcon = rankingIcon[1];
            else if (rank == 3) rankIcon = rankingIcon[2];
            else rankIcon = rankingIcon[3];

            // ##TODO : extraData에 맞는 유저 닉네임을 return해야함.

            // 업데이트 
            leaderboard.UpdateLeaderBoard(rankIcon, rank , userItem.extraData, userItem.score);
        }
    }

}

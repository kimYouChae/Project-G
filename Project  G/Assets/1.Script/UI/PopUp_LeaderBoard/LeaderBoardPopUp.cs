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

    [Header("===Localization===")]
    [SerializeField] TextMeshProUGUI leaderBoardText;

    private void InstantiateLBObject(int cnt)
    {
        // 처음 1회 생성
        for (int i = 0; i < cnt; i++) 
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

        // 리더보드의 총 유저 등록 수 
        long maxPlayer = BackEndLeaderBoardManager.Instance.GetTotalCountCount(MapType.Forest);
        Debug.Log("리더보드의 총 유저 수 : " + (int)maxPlayer);
        
        // 리스트에 없으면 -> 1회 오픈, 새로 생성
        if (lbjList.Count <= 0)
        {
            InstantiateLBObject((int)maxPlayer);
        }

        UpdateLeadrBoard((int)maxPlayer);
    }

    private void UpdateLeadrBoard(int rankCnt) 
    {
        List<UserLeaderboardItem> userItems
            = BackEndLeaderBoardManager.Instance.GetRanking(MapType.Forest, rankCnt, 0);

        for (int i = 0; i < lbjList.Count; i++)
        {
            LeaderBoardObject leaderboard = lbjList[i];
            UserLeaderboardItem userItem = userItems[i];

            // 랭킹에 따른 아이콘 설정
            int rank = int.Parse(userItem.rank);
            Sprite rankIcon;
            string rankText = string.Empty;
            if (rank == 1) rankIcon = rankingIcon[0];
            else if (rank == 2) rankIcon = rankingIcon[1];
            else if (rank == 3) rankIcon = rankingIcon[2];
            else
            {
                rankIcon = rankingIcon[3];

                // 1,2,3위가 아닐 때 만 랭크 텍스트 설정 
                rankText = userItem.rank;
            } 

            // ##TODO : extraData에 맞는 유저 닉네임을 return해야함.

            // 업데이트 
            // leaderboard.UpdateLeaderBoard(rankIcon, rankText, userItem.extraData, userItem.score);
            // 임시 - 본인 닉네임만 표시 
            leaderboard.UpdateLeaderBoard(rankIcon, rankText, userItem.nickname, userItem.score);
        }
    }

}

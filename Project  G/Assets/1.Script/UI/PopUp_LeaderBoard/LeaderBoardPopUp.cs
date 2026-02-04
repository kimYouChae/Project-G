using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    private LeaderBoardObject InstantiateLBObject()
    {
        GameObject temp = Instantiate(leaderObject);
        temp.transform.SetParent(content, false);

        LeaderBoardObject lobj = temp.GetComponent<LeaderBoardObject>();
        lbjList.Add(lobj);
        return lobj;
    }

    public void InitLeaderBoardPopUp() 
    {
        // 타이틀 로컬라이징
        // leaderBoardText.text = LocalizationManager.Instance.ReturnLocalizationString();

        // 랭커 불러오기 
        StartCoroutine(GetRank());
    }

    // On 될 때 마다 업데이트 
    public IEnumerator GetRank()
    {
        // 내 랭크 정보 출력 
        yield return GameServices.Instance.RankingService.
            GetMyRankingService(UserDataManager.Instance.SteamID, 0);
        // 랭커 정보 출력
        yield return GameServices.Instance.RankingService.GetRankerService(0);

        // 정보 바탕으로 출력하기 
        GameServices.Instance.RankingModel.PrintUserRanker();
        GameServices.Instance.RankingModel.PrintRankersList();

        // 리더보드의 총 유저 등록 수 
        /*
        long maxPlayer = BackEndLeaderBoardManager.Instance.GetTotalCountCount(MapType.Forest);
        Debug.Log("리더보드의 총 유저 수 : " + (int)maxPlayer);

        UpdateLeadrBoard((int)maxPlayer);
        */
    }

    private void UpdateLeadrBoard(int rankCnt) 
    {
        /*
        List<UserLeaderboardItem> userItems
            = BackEndLeaderBoardManager.Instance.GetRanking(MapType.Forest, rankCnt, 0);

        // for문으로 하면 안되고 while문으로 해서 cnt + 1, hasSet에 걸리면 + 2 이렇게 해야할듯 ?? 

        HashSet<string> gamdIdHash = new HashSet<string>();
        int userIdx = 0;

        while (true) 
        {
            if (userIdx >= userItems.Count)
                break;

            UserLeaderboardItem userItem = userItems[userIdx];
            string[] datas = userItem.extraData.Split("|");
            string anotherUserIndate = datas[0];
            string gamdId = datas[1];

            bool flag = gamdIdHash.Add(gamdId);
            // 중복이면
            if (!flag) 
            {
                userIdx += 2;
                continue;
            }

            LeaderBoardObject lobj;
            // 중복이 아니면 
            if (lbjList.Count <= userIdx)
                lobj = InstantiateLBObject();
            else 
                lobj = lbjList[userIdx];

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
            var bro = Backend.Social.GetUserInfoByInDate(anotherUserIndate);
            string anotherUserNickName = bro.GetReturnValuetoJSON()["row"]["nickname"].ToString();
            string namefield = UserDataManager.Instance.NickName + "/" + anotherUserNickName;

            // 리더보드 오브젝트 업데이트 
            lobj.UpdateLeaderBoard(rankIcon, rankText, namefield, userItem.score);

            userIdx += 1;
        }
        */
    }

}

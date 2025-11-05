using BackEnd;
using BackEnd.Leaderboard;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class BackEndLeaderBoardManager : Singleton<BackEndLeaderBoardManager>
{
    public List<LeaderboardTableItem> leaderBoardItem;
    protected override void Singleton_Awake()
    {
        SetUpDontDestroy();
    }

    // 리더보드 불러오기 
    public void GetLeaderBoard() 
    {
        Backend.Leaderboard.User.GetLeaderboards(bro => {

            // bro : BackendLeaderboardTableReturnObject 타입 
            // 내부에 리더보드를 담아둔 리스트 존재 
            leaderBoardItem = bro.GetLeaderboardTableList();

            Debug.Log("---리더보드 불러오기---");
            foreach (BackEnd.Leaderboard.LeaderboardTableItem item in bro.GetLeaderboardTableList())
            {
                Debug.Log(item.title);
            }
        });

    }

    // 리더보드 내 전체 순위 조회 
    public List<UserLeaderboardItem> GetRanking(MapType maptype, int rankCount, int rankStart) 
    {
        BackEnd.Leaderboard.BackendUserLeaderboardReturnObject bro = null;

        // ##TODO : 임시 MapType에 따른 차트번호 (int)mapType하면 젤 편한데 순서대로 들어오는지 모르겠음 
        int chartIdx = 0;
        switch (maptype) 
        {
            case MapType.Forest: chartIdx = 0; break;
        }

        // cnt 명 만큼, start에서 시작
        bro = Backend.Leaderboard.User.GetLeaderboard(leaderBoardItem[chartIdx].uuid, rankCount, rankStart);

        List<UserLeaderboardItem> userItems = bro.GetUserLeaderboardList();

        // 출력
        /*
        StringBuilder builder = new StringBuilder();    
        for(int i = 0; i < userItems.Count; i++) 
        {
            var userItem = userItems[i];
            builder.Append( "랭킹 : " + userItem.rank);
            builder.Append( "점수 : " + userItem.score);
            builder.Append( "extra 데이터 :" + userItem.extraName + " / " + userItem.extraData);
            Debug.Log(builder.ToString());
            builder.Clear();
        }
        */

        return userItems;
    }

    // 리더보드 업데이트 (테이블에 데이터 넣은 후 실행)
    public void UpdateLeaderBoard(MapType mapType, float score, string rowIndate, string extraData) 
    {
        Param param = new Param();
        param.Add("Score", score);
        param.Add("UserIndates", extraData);

        // ##TODO : 일단 숲밖에 없으니까 0으로 해놓기 
        string leaderBoarduuid = leaderBoardItem[0].uuid;
        string tableName = Define.MAPTYPEBY_LEADERBOARD_TABLE[mapType];

        var rankBro = Backend.Leaderboard.User.UpdateMyDataAndRefreshLeaderboard(leaderBoarduuid, tableName, rowIndate, param);
        if (rankBro.IsSuccess())
        {
            Debug.Log("리더보드 등록 성공");
        }
        else
        {
            Debug.Log("리더보드 등록 실패 : " + rankBro);
        }

    }
}

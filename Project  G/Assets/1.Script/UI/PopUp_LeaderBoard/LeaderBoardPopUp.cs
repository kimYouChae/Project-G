using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderBoardPopUp : UIPopUP
{
    [Header("===Container===")]
    [SerializeField] Sprite[] rankingIcon;                      // 1~3위 아이콘 배경 + 나머지 아이콘 
    [SerializeField] GameObject leaderObject;                   // 리더보드 오브젝트 
    [SerializeField] List<LeaderBoardObject> leaderBoardObj;

    [Header("===Dtail===")]
    [SerializeField] LeaderBoardObject myrankingObj;            // 내 랭킹에 대한 리더보드 오브젝트 

    [Header("===Conponent===")]
    [SerializeField] Transform content;     // 오브젝트 상위 부모 

    [Header("===Localization===")]
    [SerializeField] TextMeshProUGUI leaderBoardText;

    // 텍스트 사이에 구분선 ex) 감자|고구마
    private const string Divider = "|";

    public void OpenLeaderBoardPopUp() 
    {
        // UI ON, 팝업 사운드 실행
        base.OpenPopUP();

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

        // 팝업 업데이트
        StartCoroutine(UpdateLeaderBoard());
    }

    private IEnumerator UpdateLeaderBoard() 
    {
        List<UserRankDTO> rankers = GameServices.Instance.RankingModel.GetRankersList();
        UserRankDTO myRank = GameServices.Instance.RankingModel.GetUserRanker();

        // 생성된 UIㅇ 오브젝트가 없으면 
        if (leaderBoardObj.Count <= 0) 
        {
            // 생성
            yield return StartCoroutine(InstantiateLBObject(rankers.Count));
        }

        // UI 업데이트
        UpdateRankers(rankers);
        UpdateLeaderBoardObj(myrankingObj, myRank);
    }

    private IEnumerator InstantiateLBObject(int cnt)
    {
        for (int i = 0; i < cnt; i++) 
        {
            GameObject temp = Instantiate(leaderObject);
            temp.transform.SetParent(content, false);

            LeaderBoardObject lobj = temp.GetComponent<LeaderBoardObject>();
            leaderBoardObj.Add(lobj);
        }

        yield break;
    }

    private void UpdateRankers(List<UserRankDTO> rankers) 
    {
        for(int i = 0; i < rankers.Count; i++) 
        {
            // 만약 넘으면 
            if (i >= leaderBoardObj.Count)
                return;

            UpdateLeaderBoardObj(leaderBoardObj[i], rankers[i]);
        }
    }

    private void UpdateLeaderBoardObj(LeaderBoardObject lbObj, UserRankDTO userRankDTO) 
    {
        Sprite rankIcon = null;
        string rankText = string.Empty;

        // 1,2,3 등 아이콘 정하기 , 이외는 텍스트로 몇등
        if (userRankDTO.ranking == 1) rankIcon = rankingIcon[0];
        else if (userRankDTO.ranking == 2) rankIcon = rankingIcon[1];
        else if (userRankDTO.ranking == 3) rankIcon = rankingIcon[2];
        else
        {
            rankIcon = rankingIcon[3];
            rankText = userRankDTO.ranking.ToString();
        }

        // 리더보드 오브젝트 업데이트
        lbObj.UpdateLeaderBoard(rankIcon, rankText, userRankDTO.player1_nick + Divider + userRankDTO
            .player2_nick, userRankDTO.stage + Divider + userRankDTO.score);
    }


}

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
    [SerializeField] GameObject uiContentsRanking;          // 팝업 내부 ui ( 리더보드 )
    [SerializeField] GameObject uiContentsDetails;          // 팝업 내부 ui ( 디테일창 )
    [SerializeField] TextMeshProUGUI rankingLoadingText;   // 로딩중 텍스트 ( 리더보드 )
    [SerializeField] TextMeshProUGUI myrankLoadingText;     // 로딩중 텍스트 ( 디테일창 )
    [SerializeField] Transform content;     // 오브젝트 상위 부모 

    [Header("===Localization===")]
    [SerializeField] TextMeshProUGUI leaderBoardText;

    [Header("===MapType===")]
    [SerializeField] MapType mapType = MapType.Forest;

    // 텍스트 사이에 구분선 ex) 감자|고구마
    private const string Divider = "|";

    public void OpenLeaderBoardPopUp()
    {
        // UI ON, 팝업 사운드 실행
        base.OpenPopUP();

        // 타이틀 로컬라이징
        leaderBoardText.text = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.Lobby_Ranking);
        rankingLoadingText.text = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.LoadingData);
        myrankLoadingText.text = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.LoadingData);

        uiContentsRanking.gameObject.SetActive(false);
        uiContentsDetails.gameObject.SetActive(false);

        // 내 랭킹 가져오기
        StartCoroutine(GetRank());
        // 랭커 가져오기 
        StartCoroutine(GetRankers());
    }

    private IEnumerator GetRank()
    {
        // 서버에 로그인이 안됐으면 -> 내 랭킹 가져오기 API 실행 X
        if (UserDataManager.IsOfflineMode)
        {
            myrankLoadingText.text
                    = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.Ranking_MyRank_Offline);
            yield break;
        }

        // 내 랭킹 가져오기 API 실행
        yield return GameServices.Instance.RankingService.
            GetMyRankingService(UserDataManager.Instance.SteamID, 0);

        // UI 업데이트
        MyRankUIUpdate();
    }

    private IEnumerator GetRankers() 
    {
        // 랭커 가져오기 API 실행
        yield return GameServices.Instance.RankingService.GetRankerService((int)mapType);

        // 랭커 API 실패 시 
        if (!GameServices.Instance.RankingModel.GetIsSuccess()) 
        {
            rankingLoadingText.text
                    = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.Ranking_RankerList_Offline);
            yield break;
        }

        // UI 업데이트
        yield return StartCoroutine(RankersUIUpdate());
    }

    private void MyRankUIUpdate() 
    {
        UserRankDTO myRank = GameServices.Instance.RankingModel.GetUserRanker();

        if (myRank == null)
        {
            myrankLoadingText.text
                = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.Ranking_MyRank_Offline);
            return;
        }

        myrankLoadingText.text = "";
        uiContentsDetails.gameObject.SetActive(true);

        UpdateLeaderBoardObj(myrankingObj, myRank);
    }

    private IEnumerator RankersUIUpdate() 
    {
        List<UserRankDTO> rankers = GameServices.Instance.RankingModel.GetRankersList();

        // 생성된 UIㅇ 오브젝트가 없으면 
        if (leaderBoardObj.Count <= 0) 
        {
            // 생성
            yield return StartCoroutine(InstantiateLBObject(rankers.Count));
        }

        // UI 업데이트
        UpdateRankers(rankers);
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
        rankingLoadingText.text = "";
        uiContentsRanking.gameObject.SetActive(true);

        for (int i = 0; i < rankers.Count; i++) 
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
        lbObj.UpdateLeaderBoard(rankIcon, rankText, userRankDTO.player1_nick,  userRankDTO.player2_nick,
             userRankDTO.score.ToString() , userRankDTO.stage.ToString());
    }


}

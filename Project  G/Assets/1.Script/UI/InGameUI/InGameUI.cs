using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;


public class InGameUI : Singleton<InGameUI>
{
    [Space]
    [Header("===InGameUI===")]
    [SerializeField] Camera camera;
    [SerializeField] float cameraFarZ = -5f;

    [Header("===HighLight===")]
    [SerializeField] GameObject highlightBG;    // 하이라이트 이미지 
    const float highlightMinScale = 1f;
    const float highlightTime = 1f;

    [Header("===Panel===")]
    [SerializeField] GameObject gamePanel;      // 게임 패널
    [SerializeField] GameObject gameoverPanel;  // 게임오버 패널
    [SerializeField] GameObject loadingPanel;   // 로딩패널
    [SerializeField] TextMeshProUGUI countDownText;     // 카운트다운 텍스트 

    [Header("===Component===")]
    public GameUI gameUI;
    public GameOverUI gameOverUI;

    public GameObject LoadingPanel { get => loadingPanel;}
    public TextMeshProUGUI CountDownText { get => countDownText; set => countDownText = value; }
    public GameObject GamePanel { get => gamePanel; set => gamePanel = value; }

    protected override void Singleton_Awake()
    {
        gameUI = GetComponent<GameUI>();
        gameOverUI = GetComponent<GameOverUI>();
    }

    public void HighlightPlayer(Transform trs) 
    {
        // 카메라 위치 이동 
        camera.transform.position = new Vector3(trs.position.x, trs.position.y, cameraFarZ);

        if(highlightBG.activeSelf == false)
            highlightBG.SetActive(true);

        // 게임UI 끄기
        gamePanel.SetActive(false);
        
        // 크기를 min까지 줄이는 애니메이션 실행 
        RedueceAnimation();

        // 기다린후 게임오버 실행
        StartCoroutine(WaitUntilAnimation());
    }

    private void RedueceAnimation() 
    {
        highlightBG.GetComponent<RectTransform>().DOScale(
            new Vector3(highlightMinScale, highlightMinScale, 0) ,
            highlightTime)
            .SetUpdate(true);   // timel.scale에 영향 받지 않는 
    }

    IEnumerator WaitUntilAnimation() 
    {
        yield return new WaitForSecondsRealtime(highlightTime * 1.5f);

        gameoverPanel.SetActive(true);

        // 현재 저장되어있는 점수
        MapType type = PunIngameManager.Instance.GetMapType();
        float preScore = 0f;    // 이전점수
        float currScore = ScoreManager.Instance.AchiveScore;    // 현재 점수
        int currStage = ScoreManager.Instance.AchiveStage;      // 현재 스테이지

        if (type != MapType.None) 
            preScore = UserDataManager.Instance.UserData.MapTypeToScore[type];

        // 게임오버 텍스트 설정
        SetGameOverText(currScore, preScore);

        // 유저 데이터 설정
        SetUserData(currScore, currStage, preScore);

        // 리더보드 저장
        SetLeadBoard(type, currScore);

    }

    public void CountDownUpdateText(int count) 
    {
        countDownText.text = count.ToString();
    }

    private void SetGameOverText(float currScore, float preScore) 
    {
        // 게임오버 텍스트 설정
        gameOverUI.GameOverText(currScore, ScoreManager.Instance.CurrTime,
            preScore <= currScore ? true : false);
    }

    private void SetUserData(float currScore, int currStage, float preScore) 
    {
        // 최고점수일때만 업데이트
        if (preScore <= currScore)
        {
            // 점수 + 스테이지 달성 저장
            UserDataManager.Instance.SettingAchiveData(currScore, currStage);

            // 유저 정보 업데이트
            UserDataManager.Instance.UpdateUserData();
        }
    }

    private void SetLeadBoard(MapType type, float currScore) 
    {
        // 토큰이 만료되면 패스
        // = local에 유저 정보가 없으면 패스 ( 중복로그인안됨 )
        BackendReturnObject bro = Backend.BMember.IsAccessTokenAlive();
        if (bro.IsSuccess())
        {

            Debug.Log("엑세스 토큰이 살아있습니다. 리더보드 저장을 시작합니다");
            // (리더보드용) 점수저장
            var indate = ScoreDataManager.Instance.InserToLeaderBoardTableAndReturnIndate(type, currScore);

            // 리더보드 업데이트 
            BackEndLeaderBoardManager.Instance.UpdateLeaderBoard(type, currScore, indate.Item1, indate.Item2);
        }
        else
        {
            Debug.Log("엑세스 토큰이 죽었습니다. 리더보드 저장 x ");
        }

    }
}

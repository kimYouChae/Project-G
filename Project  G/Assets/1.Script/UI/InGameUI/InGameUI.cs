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

        StartCoroutine(GameOverUICoru());
    }

    private void RedueceAnimation() 
    {
        highlightBG.GetComponent<RectTransform>().DOScale(
            new Vector3(highlightMinScale, highlightMinScale, 0) ,
            highlightTime)
            .SetUpdate(true);   // timel.scale에 영향 받지 않는 
    }

    private IEnumerator GameOverUICoru() 
    {
        // (실제시간) 애니메이션 끝날 때 까지 대기 
        yield return new WaitForSecondsRealtime(highlightTime * 1.5f);

        // 게임 Over 패널 켜기
        gameoverPanel.SetActive(true);
        // 게임 오버 UI ( "로딩중..."텍스트)
        gameOverUI.OnOffGameContextTexts(false);
    }


    public void CountDownUpdateText(int count) 
    {
        countDownText.text = count.ToString();
    }

    public void SetGameOverText() 
    {
        // 게임오버 텍스트 설정
        // gameOverUI.GameOverText(score, isUpdated , gameTime);
    }

   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;  // 두트윈 

public partial class InGameUI : MonoBehaviour
{
    private static InGameUI instance;   // 인스턴스

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

    public GameObject LoadingPanel { get => loadingPanel;}
    public TextMeshProUGUI CountDownText { get => countDownText; set => countDownText = value; }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;
    }

    public static InGameUI GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("InGameUI 인스턴스가 존재하지 않습니다.");
            return null;
        }

        return instance;
    }

    private void Start()
    {
        InitGameOverUI();
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
        float preScore = 0f;
        if (type != MapType.None) 
            preScore = UserDataManager.Instance.UserData.MapTypeToScore[type];

        // 게임오버 텍스트 설정
        GameOverText(ScoreManager.Instance.CurrScore, ScoreManager.Instance.CurrTime,
            preScore <= ScoreManager.Instance.CurrScore ? true : false);

        // 점수 세팅
        UserDataManager.Instance.SettingScore(ScoreManager.Instance.CurrScore);

        // 최고점수일때만 업데이트
        if (preScore <= ScoreManager.Instance.CurrScore) 
        {
            // 유저 정보 업데이트
            UserDataManager.Instance.UpdateUserData();
        }
    }

    public void CountDownUpdateText(int count) 
    {
        countDownText.text = count.ToString();
    }

}

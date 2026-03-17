using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopUP : MonoBehaviour
{
    [Header("===State===")]
    [SerializeField] protected float onStartSize;   // ON : 팝업 시작 사이즈
    [SerializeField] protected float oriSize;       // 원래 사이즈 ( 기본 : 1f )
    [SerializeField] protected float offEndSize;    // Off : 팝업 끝 사이즈

    [SerializeField] protected float startTime;     // On할 때 팝업 진행 속도
    [SerializeField] protected float endTime;       // Off할 때 팝업 진행 속도

    [SerializeField] protected Ease popUpEase;      // 팝업 부드러운 이동 ( 기본 : Ease.OutBack )

    [Header("===Button===")]
    [SerializeField] protected Button closeButton;

    [Header("===SFX Type===")]
    [SerializeField] private bool isOpend = false;      // 최초생성이면 SFX 실행 x 
    [SerializeField] protected SFXType openSFXType = SFXType.UIPopup;    
    [SerializeField] protected SFXType closeSFXType = SFXType.UIBack;

    // Mono의 생명주기 함수 awake -> onEnable -> start
    void Awake()
    {
        onStartSize = 0.8f;
        oriSize = 1f;
        offEndSize = 0.8f;
        startTime = 0.25f;
        endTime = 0.2f;

        popUpEase = Ease.OutBack;

        // 수치 조정 필요 시 하위에서 초기화 
        InitPopUpState();

        closeButton.onClick.AddListener(() =>
        {
            // Debug.Log("close버튼");
            OffPanel();
        });
    }

    #region 필요하면 하위에서 override

    // popup 수치 조정 필요할 때 
    protected virtual void InitPopUpState() { }

    // open SFX를 조정하고 싶을 때 (text 전용 팝업 : warning 사운드)
    protected virtual SFXType GetOpenSFXType()
    {
        return openSFXType;
    }

    protected void OpenPopUP() 
    {
        // 켜질 때
        PlayShowAnimation(onStartSize, oriSize, startTime, popUpEase);

        // 사운드 실행
        SFXManager.Instance.PlaySFX(GetOpenSFXType());
    }

    #endregion

    protected void PlayShowAnimation(float startSize, float endSize, float speed, Ease ease)
    {
        // Debug.Log($"{startSize} 부터 {endSize} 까지 크기 커짐 {speed}의 속도로 ");
        // 켤 때 onStartSize  -> oriSize 만큼       , startTime 의 속도로
        // 끌 때 oriSize      -> offEndSize 만큼    , endTime의 속도로 
        transform.localScale = Vector3.one * startSize;
        transform.DOScale(Vector3.one * endSize, speed).SetEase(ease);
    }

    public void OffPanel()
    {
        // 꺼질 때 
        PlayShowAnimation(oriSize, offEndSize, endTime, popUpEase);

        Invoke(nameof(SetActiveFalsePanel), endTime / 2);

        // 사운드 실행
        SFXManager.Instance.PlaySFX(closeSFXType);
    }

    private void SetActiveFalsePanel()
    {
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;  // 두트윈 

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

    public void HighlightPlayer(Transform trs) 
    {
        // 카메라 위치 이동 
        camera.transform.position = new Vector3(trs.position.x, trs.position.y, cameraFarZ);

        if(highlightBG.activeSelf == false)
            highlightBG.SetActive(true);

        // 크기를 min까지 줄이는 애니메이션 실행 
        RedueceAnimation();
    }

    private void RedueceAnimation() 
    {
        highlightBG.GetComponent<RectTransform>().DOScale(
            new Vector3(highlightMinScale, highlightMinScale, 0) ,
            highlightTime)
            .SetUpdate(true);   // timel.scale에 영향 받지 않는 
    }
}

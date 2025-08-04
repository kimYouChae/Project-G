using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] private float currScore = 0;
    [SerializeField] private float currTime = 0;

    [SerializeField] private float rateInCrease = 0.7f;    // (임시) 상승폭  
    [SerializeField] private float rateTime = 0.1f;     // 상승 시간 쿨타임

    const float oneFrame = 0.02f;

    public float CurrScore { get => currScore; }
    public float CurrTime { get => currTime;  }

    protected override void Singleton_Awake()
    {

    }

    private void Start()
    {
        StartCoroutine(InCreaseScore());
        StartCoroutine(InCreateTime());
    }

    IEnumerator InCreaseScore() 
    {
        while(true) 
        {
            yield return new WaitForSeconds(rateTime);

            currScore += rateInCrease;
            InGameUI.GetInstance().UpdateScoreText(currScore);
        }
    }

    IEnumerator InCreateTime() 
    {
        while (true) 
        {
            yield return new WaitForSeconds(oneFrame);

            currTime += oneFrame;
            InGameUI.GetInstance().UpdateTimeText(currTime);
        }
    }

}

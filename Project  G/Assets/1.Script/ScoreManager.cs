using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] private float startScore = 0;

    [SerializeField] private float rateInCrease = 0.7f;    // (임시) 상승폭  
    [SerializeField] private float rateTime = 0.1f;     // 상승 시간 쿨타임

    protected override void Singleton_Awake()
    {

    }

    private void Start()
    {
        StartCoroutine(InCreaseScore());
    }

    IEnumerator InCreaseScore() 
    {
        while(true) 
        {
            yield return new WaitForSeconds(rateTime);

            startScore += rateInCrease;
        }
    }

}

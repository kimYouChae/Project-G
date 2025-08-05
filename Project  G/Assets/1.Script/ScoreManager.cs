using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] private float startTime;   // 네트워크상 시작시간
    [SerializeField] private bool isReadyToCount = false;

    [SerializeField] private float currScore = 0;
    [SerializeField] private float currTime = 0;


    const float oneFrame = 0.02f;

    public float CurrScore { get => currScore; }
    public float CurrTime { get => currTime;  }
    public bool IsReadyToCount { get => isReadyToCount; set => isReadyToCount = value; }

    protected override void Singleton_Awake()
    {

    }

    private void Update()
    {
        if (!isReadyToCount)
            return;

        double elapsed = PhotonNetwork.Time - startTime;

        currTime = (float)elapsed;
        currScore = (float)elapsed * 1.7f;

        InGameUI.GetInstance().UpdateScoreText(currScore);
        InGameUI.GetInstance().UpdateTimeText(currTime);
    }

    public void ScoreBegin(float networkStartTime) 
    {
        startTime = networkStartTime;

        isReadyToCount = true;
    }
}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] private float startTime;   // 네트워크상 시작시간
    [SerializeField] private bool isReadyToCount = false;

    [SerializeField] private int nowLevel = 0;      // 현재 레벨 (난이도)

    private float currScore = 0;
    private float currTime = 0;

    // Mapdata의 난이도 증가 비율이 클 수록 작은 값으로 곱할 수있게 임시 비율설정 
    const int RATE_TEMP = 30;

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
        currScore = (float)(RATE_TEMP / MapDataManager.Instance.MapRate) * nowLevel * (float)elapsed;

        // mapData의 "난이도증가비율"에 따라서 난이도 증가
        if (currTime >= nowLevel * MapDataManager.Instance.MapRate)
        { 
            nowLevel++;
            Debug.Log("현재 난이도 :" + nowLevel);

            PunIngameManager.Instance.spawnerManager.BulletSpawn(nowLevel);
        }

        // UI 업데이트
        InGameUI.GetInstance().UpdateScoreText(currScore);
        InGameUI.GetInstance().UpdateTimeText(currTime);
    }

    public void ScoreBegin(float networkStartTime) 
    {
        startTime = networkStartTime;

        isReadyToCount = true;
    }
}

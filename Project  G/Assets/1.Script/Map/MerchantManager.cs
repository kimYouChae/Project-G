using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantData 
{
    private MerchantPatternType merchantType;
    private float speed;
    private float sight;
    private float waitTime;
    private bool shouldStop;

    public MerchantData(MerchantPatternType merchantType, float speed, float sight, float waitTime, bool shouldStop)
    {
        this.merchantType = merchantType;
        this.speed = speed;
        this.sight = sight;
        this.waitTime = waitTime;
        this.shouldStop = shouldStop;
    }

    public float Speed { get => speed; }
    public float Sight { get => sight; }
    public float WaitTime { get => waitTime;  }
    public bool ShouldStop { get => shouldStop;  }
}


public class MerchantManager : Singleton<MerchantManager>
{
    private Dictionary<MerchantPatternType, MerchantData> keyValuePairs;

    protected override void Singleton_Awake()
    {
        // ##임시 하드코딩
        keyValuePairs = new Dictionary<MerchantPatternType, MerchantData>
        {
            { MerchantPatternType.Straight , new MerchantData(
                MerchantPatternType.Straight , 3f, 0, 0 ,false)},
            { MerchantPatternType.Stop , new MerchantData(
                MerchantPatternType.Stop , 3f, 0, 1f ,true)},
            { MerchantPatternType.Meeting , new MerchantData(
                MerchantPatternType.Meeting , 3f, 1.5f, 2f ,true)},
            { MerchantPatternType.Cart , new MerchantData(
                MerchantPatternType.Cart , 3f, 0, 0.5f ,true)},
        };
    }

    public MerchantData GetData(MerchantPatternType type) { return keyValuePairs[type]; }
}

using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FusionSettingRunner : MonoBehaviour 
{
    public static FusionSettingRunner instance;

    public static FusionSettingRunner GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("FusionSettingRunner 인스턴스가 존재하지 않습니다.");
            return null;
        }
        return instance;
    }
    
    [SerializeField] NetworkRunner runner;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 네트워크 러너 세팅 
    public NetworkRunner InitNetWorkRunner() 
    {
        if (!gameObject.TryGetComponent<NetworkRunner>(out runner))
            runner = gameObject.AddComponent<NetworkRunner>();

        return runner;
    }

    public NetworkRunner GetRunner() 
    {
        if (runner == null)
            InitNetWorkRunner();

        return runner;
    }


}

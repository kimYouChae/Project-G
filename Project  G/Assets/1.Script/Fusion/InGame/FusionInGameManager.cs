using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionInGameManager : MonoBehaviour
{
    private static FusionInGameManager instance;

    // 네트워크 주체
    [SerializeField] private NetworkRunner runner;
    // 콜백
    [SerializeField] private FusionInGameCallBack callback;
    // 생성할 네트워크 프리팹
    [SerializeField] private NetworkPrefabRef playerPrefab;

    [Header("===Player===")]
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
    [SerializeField] Transform[] spawnedPoint;
    [SerializeField] LayerMask[] playerLayerList;

    public NetworkPrefabRef NetworkPlayerPrefab { get => playerPrefab; }

    public static FusionInGameManager GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("FusionManager 인스턴스가 존재하지 않습니다.");
            return null;
        }
        return instance;
    }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }

        // 네트워크와 통신하기 위해서 
        if (!gameObject.TryGetComponent<FusionInGameCallBack>(out callback))
            callback = gameObject.AddComponent<FusionInGameCallBack>();

        // NetworkRunner 세팅
        runner = FusionSettingRunner.GetInstance().GetRunner();

        // 콜백 재등록
        runner.AddCallbacks(callback);

        _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
    }

    private void Start()
    {
        // InstancePlayer();
    }

    private void InstancePlayer() 
    {
        if (runner == null) 
        {
            Debug.Log("Runner가 NULL입니다. 플레이어를 생성할 수 없습니다.");
        }

        // 서버(호스트)만 오브젝트 생성 가능 
        if (runner.IsServer) 
        {
            int index = 0;
            foreach (PlayerRef pl in runner.ActivePlayers) 
            {
                NetworkObject netPlayerObj = runner.Spawn(playerPrefab , spawnedPoint[index].position , Quaternion.identity );
                netPlayerObj.gameObject.layer = playerLayerList[index];

                // 딕셔너리에 넣기
                _spawnedCharacters.Add(pl, netPlayerObj);

                index++;
            }
        }
    }

}

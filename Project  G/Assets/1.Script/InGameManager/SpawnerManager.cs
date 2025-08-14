using Photon.Pun;
using System;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] private PhotonView localPlayer;
    [SerializeField] private NetPlayer localNetPlayer;
    [SerializeField] private int localPlayerIndex;

    private Action<int> bulletSpawn;

    const string BASIC_BULLET_SPAWNER = "BulletSpawner";
    const string GUIDED_MISSILE_SPAWNER = "GuidedMissile";
    const string LASER_SPAWNER = "LaserSpawner";
    const string FOUR_DIRECET_SPAWNER = "FourDirectSpanwer";

    const int BASIC_SPAWN_STAGE = 0;
    const int GUIDED_MISSILE_SPAWN_STAGE = 3;
    const int LASER_SPAWN_STATE = 5;
    const int FOUR_DIRECT_SPAWN_STATE = 7;

    const float BASIC_SPAWNER_INTERVEL = 1; // 기본 총알 생성 스테이지 간격 

    public Action<int> BulletSpawn { get => bulletSpawn; }

    private void Start()
    {
        bulletSpawn += Temp;
    }

    public void SetLoacalPlayer(PhotonView local) 
    {
        localPlayer = local;

        localNetPlayer = localPlayer.GetComponent<NetPlayer>();
        localPlayerIndex = localNetPlayer.PlayerIndex;
    }

    public void Temp(int stage) 
    {
        switch (stage)
        {
            case 1:
                SpawnBasicBulleSpanwer(DirType.Left);
                break;
            case 2:
                SpawnBasicBulleSpanwer(DirType.Right);
                break;
            case 3:
                SpawnBasicBulleSpanwer(DirType.Top);
                break;
            case 4:
                SpawnBasicBulleSpanwer(DirType.Bottom);
                break;
        }
    }

    private void SpawnBasicBulleSpanwer(DirType dir) 
    {
        GameObject spawnerObj = PhotonNetwork.Instantiate(BASIC_BULLET_SPAWNER, new Vector3(0, 0, 0), Quaternion.identity);
        NetSpawner spawner = spawnerObj.GetComponent<NetSpawner>();

        try 
        {
            spawner.SettingParent(localPlayerIndex, dir);
            spawner.SettingOwner(localPlayer.ViewID, dir);
        }
        catch (Exception e) { Debug.LogError(e); }
    }

    private void SpawnGuideSpanwer() 
    {
        // 만약 1사분면 플레이어면 -> 오른쪽에 생성
        // 만약 2사분면 플레이어면 -> 왼쪽에 생성
        if(localNetPlayer.PlayerQuadtype == QuadrantType.one) 
        {
            CreateGuiedSpanwer(DirType.Right);
        }
        else if(localNetPlayer.PlayerQuadtype == QuadrantType.two)
        {
            CreateGuiedSpanwer(DirType.Left);
        }
    }

    private void CreateGuiedSpanwer(DirType type) 
    {
        GameObject spawnerObj = PhotonNetwork.Instantiate(GUIDED_MISSILE_SPAWNER, new Vector3(0, 0, 0), Quaternion.identity);
    }
}

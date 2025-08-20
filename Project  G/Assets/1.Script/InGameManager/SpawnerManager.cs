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
    const string GUIDED_MISSILE_SPAWNER = "GuidedMissileSpanwer";
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
        // 기본 미사일 생성
        switch (stage)
        {
            case 1:
                CreateSpanwer(DirType.Left, BASIC_BULLET_SPAWNER);
                break;
            case 2:
                CreateSpanwer(DirType.Right, BASIC_BULLET_SPAWNER);
                break;
            case 3:
                CreateSpanwer(DirType.Top, BASIC_BULLET_SPAWNER);
                break;
            case 4:
                CreateSpanwer(DirType.Bottom, BASIC_BULLET_SPAWNER);
                break;
        }

        // 따라가는 미사일 생성
        if (stage == GUIDED_MISSILE_SPAWN_STAGE) 
        {
            SpawnGuideSpanwer();
        }

        // 레이저 생성
        if (stage == LASER_SPAWN_STATE) 
        {
            SpawnLaserSpawner();
        }

        // 십자 방향 폭탄 발사
        if (stage == FOUR_DIRECT_SPAWN_STATE) 
        {
            SpawnFourDirSpawner();
        }
    }

    private void SpawnGuideSpanwer() 
    {
        // 만약 1사분면 플레이어면 -> 오른쪽에 생성
        // 만약 2사분면 플레이어면 -> 왼쪽에 생성
        if(localNetPlayer.PlayerQuadtype == QuadrantType.one) 
        {
            CreateSpanwer(DirType.Right, GUIDED_MISSILE_SPAWNER);
        }
        else if(localNetPlayer.PlayerQuadtype == QuadrantType.two)
        {
            CreateSpanwer(DirType.Left, GUIDED_MISSILE_SPAWNER);
        }
    }

    private void SpawnLaserSpawner() 
    {
        // 만약 1사분면 플레이어면 -> 오른쪽에 생성
        // 만약 2사분면 플레이어면 -> 왼쪽에 생성
        if (localNetPlayer.PlayerQuadtype == QuadrantType.one)
        {
            CreateSpanwer(DirType.Right, LASER_SPAWNER);
        }
        else if (localNetPlayer.PlayerQuadtype == QuadrantType.two)
        {
            CreateSpanwer(DirType.Left, LASER_SPAWNER);
        }
    }

    private void SpawnFourDirSpawner() 
    {
        // 만약 1사분면 플레이어면 -> 왼쪽에 생성
        // 만약 2사분면 플레이어면 -> 오른쪽에 생성
        if (localNetPlayer.PlayerQuadtype == QuadrantType.one) 
        {
            CreateSpanwer(DirType.Left, FOUR_DIRECET_SPAWNER);
        }
        else if (localNetPlayer.PlayerQuadtype == QuadrantType.two)
        {
            CreateSpanwer(DirType.Right, FOUR_DIRECET_SPAWNER);
        }
    }

    #region string에 따른 스포너 생성

    private void CreateSpanwer(DirType dir, string spawnerName) 
    {
        GameObject spawnerObj = PhotonNetwork.Instantiate(spawnerName, new Vector3(0, 0, 0), Quaternion.identity);
        NetSpawner spawner = spawnerObj.GetComponent<NetSpawner>();

        try
        {
            // 1. 부모지정
            spawner.SettingParent(localPlayerIndex, dir);
            // 2. owner 지정
            spawner.SettingOwner(localPlayer.ViewID, dir);
            // 3. dir지정 후 
            spawner.SettingDir(dir);
            // 4. 움직임 지정 / 총알 스포너 위치 지정 
            spawner.SettingMoving();
            spawner.SettingBulletShootPosi();

        }
        catch (Exception e) { Debug.LogError(e); }
    }

    #endregion
}

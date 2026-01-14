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
        // 생성할 스포너와 위치
        SpawnerType spawnerType;
        DirType dirType;

        // 스테이지데이터 길이와 stage가 넘어가면
        if (stage > StageDataManager.Instance.StageDataMaxLength) 
            return;

        // 플레이어 위치(qu)와 스테이지에 따른
        // 스테이지 데이터 가져오기
        StageData data = StageDataManager.Instance.StageData(localNetPlayer.PlayerQuadtype, stage);
        if (data == null)
        {
            Debug.Log("스테이지Data가 NULL 입니다");
            return;
        }

        spawnerType = data.SpawnerType;
        dirType = data.DirType;

        // 스포너 생성
        CreateSpanwer(spawnerType, dirType);
    }

    private void CreateSpanwer(SpawnerType sType, DirType dType) 
    {
        switch (sType) 
        {
            case SpawnerType.BasicSpanwer:
                InstanceSpanwer(sType, dType, Define.DEFAULT_SPAWNER + BASIC_BULLET_SPAWNER);
                break;
            case SpawnerType.GuideMissileSpawner: 
                InstanceSpanwer(sType, dType, Define.DEFAULT_SPAWNER + GUIDED_MISSILE_SPAWNER);
                break;
            case SpawnerType.LaserSpawner: 
                InstanceSpanwer(sType, dType, Define.DEFAULT_SPAWNER + LASER_SPAWNER);
                break;
            case SpawnerType.FourDirSpanwer: 
                InstanceSpanwer(sType, dType, Define.DEFAULT_SPAWNER + FOUR_DIRECET_SPAWNER);
                break;
        }
    }

    /*
    private void SpawnGuideSpanwer() 
    {
        // 만약 1사분면 플레이어면 -> 오른쪽에 생성
        // 만약 2사분면 플레이어면 -> 왼쪽에 생성
        if(localNetPlayer.PlayerQuadtype == QuadrantType.one) 
        {
            CreateSpanwer(SpawnerType.GuideMissileSpawner ,DirType.Right, GUIDED_MISSILE_SPAWNER);
        }
        else if(localNetPlayer.PlayerQuadtype == QuadrantType.two)
        {
            CreateSpanwer(SpawnerType.GuideMissileSpawner, DirType.Left, GUIDED_MISSILE_SPAWNER);
        }
    }

    private void SpawnLaserSpawner() 
    {
        // 만약 1사분면 플레이어면 -> 오른쪽에 생성
        // 만약 2사분면 플레이어면 -> 왼쪽에 생성
        if (localNetPlayer.PlayerQuadtype == QuadrantType.one)
        {
            CreateSpanwer(SpawnerType.LaserSpawner,DirType.Right, LASER_SPAWNER);
        }
        else if (localNetPlayer.PlayerQuadtype == QuadrantType.two)
        {
            CreateSpanwer(SpawnerType.LaserSpawner,DirType.Left, LASER_SPAWNER);
        }
    }

    private void SpawnFourDirSpawner() 
    {
        // 만약 1사분면 플레이어면 -> 왼쪽에 생성
        // 만약 2사분면 플레이어면 -> 오른쪽에 생성
        if (localNetPlayer.PlayerQuadtype == QuadrantType.one) 
        {
            CreateSpanwer(SpawnerType.FourDirSpanwer,DirType.Left, FOUR_DIRECET_SPAWNER);
        }
        else if (localNetPlayer.PlayerQuadtype == QuadrantType.two)
        {
            CreateSpanwer(SpawnerType.FourDirSpanwer,DirType.Right, FOUR_DIRECET_SPAWNER);
        }
    }
    */

    #region string에 따른 스포너 생성
    private void InstanceSpanwer(SpawnerType type, DirType dir, string spawnerName)
    {
        GameObject spawnerObj = null;
        try
        {
            // 스포너 이름 + 방향으로 가져오기 
            spawnerObj = PhotonNetwork.Instantiate(spawnerName + dir.ToString(), new Vector3(0, 0, 0), Quaternion.identity);
        }
        catch (Exception e) { Debug.Log(e); }

        if (spawnerObj == null)
        { 
            Debug.LogError(spawnerName + dir.ToString() + "을 못가져오고있음");
            return;
        }

        if (spawnerObj != null)
        {
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
                // 5. data 지정해주기
                spawner.SettingSpawnerData(type);

            }
            catch (Exception e) { Debug.LogError(e); }
        }

    }

    #endregion
}

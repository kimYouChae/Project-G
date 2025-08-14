using Photon.Pun;
using System;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] private PhotonView localPlayer;
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

        // 로컬 플레이어에 저장되어 있는 (localPlayer) 인덱스
        // 에 해당하는 스포너 기준으로 생성하면 될듯 ?
        localPlayerIndex = localPlayer.GetComponent<NetPlayer>().PlayerIndex;
    }

    public void Temp(int stage) 
    {
        switch (stage)
        {
            case 1:
                SpawnBasicBullet(DirType.Left);
                break;
            case 2:
                SpawnBasicBullet(DirType.Right);
                break;
            case 3:
                SpawnBasicBullet(DirType.Top);
                break;
            case 4:
                SpawnBasicBullet(DirType.Bottom);
                break;
        }
    }

    private void SpawnBasicBullet(DirType dir) 
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

}

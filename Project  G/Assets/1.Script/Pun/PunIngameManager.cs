using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public enum DirType 
{
    Left, Top, Right, Bottom
}

public class PunIngameManager : Singleton<PunIngameManager>
{

    [Header("===플레이어 스폰===")]
    [SerializeField] private PhotonView localPlayer;
    [SerializeField] private List<PhotonView> playerList;

    [Header("===총알 스폰===")]
    [SerializeField] private List<Transform> playerField; // 필드 위치 (현재 왼 -> 오 순서)
    [SerializeField] private Vector2[] indexToSpawnPoint;   // 부모 기준 스포너 위치 

    public List<Transform> PlayerField { get => playerField; }
    public Vector2[] IndexToSpawnPoint { get => indexToSpawnPoint;}


    protected override void Singleton_Awake()
    {
        
    }

    private void Start()
    {
        playerList = new List<PhotonView>();

        InitBulletSpawnList();

        CreatePlayer();

        StartCoroutine(GenerateBulletSpawner());
    }

    private void CreatePlayer() 
    {
        if (PhotonNetwork.InRoom)
        {
            // 고유한 ActorNum을 가짐 (1부터시작)
            int index = PhotonNetwork.LocalPlayer.ActorNumber - 1;
            // Resources 파일 하위에 동일한 이름의 오브젝트가 있어야함 ! 
            GameObject temp = PhotonNetwork.Instantiate("Player_1", playerField[index].position, Quaternion.identity);
            temp.GetComponent<NetPlayer>().SetIndex(index);
            // 리스트에 저장 
            playerList.Add(localPlayer);

            // 내것만 저장
            if(temp.GetComponent<PhotonView>().IsMine)
                localPlayer = temp.GetComponent<PhotonView>();
        }
    }

    private void InitBulletSpawnList() 
    {
        indexToSpawnPoint = new Vector2[4];

        // 부모 기준 스포너 위치 
        indexToSpawnPoint[(int)DirType.Left] = new Vector2(-0.57f,0);
        indexToSpawnPoint[(int)DirType.Top] = new Vector2(0,0.55f);
        indexToSpawnPoint[(int)DirType.Right] = new Vector2(0.57f,0);
        indexToSpawnPoint[(int)DirType.Bottom] = new Vector2(0, -0.55f);
    }

    IEnumerator GenerateBulletSpawner() 
    {
        yield return new WaitForSeconds(0.02f);

        // 로컬 플레이어에 저장되어 있는 (localPlayer) 인덱스
        // 에 해당하는 스포너 기준으로 생성하면 될듯 ?

        int index = localPlayer.GetComponent<NetPlayer>().PlayerIndex;

        for (int dir = 0; dir < 4; dir++) 
        {
            GameObject spawnerObj = PhotonNetwork.Instantiate("BulletSpawner", new Vector3(0, 0, 0), Quaternion.identity);
            NetSpawner spawner = spawnerObj.GetComponent<NetSpawner>();
            spawner.SettingParent(index, dir);

            if (localPlayer != null)
                spawner.SettingOwner(localPlayer.ViewID, (DirType)dir);
            else
                Debug.LogWarning("localPlayer가 NULL입니다 왜지/!??");

            yield return new WaitForSeconds(10f);
        }
    }

}

using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGamePlayer 
{
    // 맵 상 어느 사분면에 있는지 
    QuadrantType quadrantType;
    // Photonview
    PhotonView view;
}

public class PunIngameManager : Singleton<PunIngameManager>
{

    [Header("===플레이어 스폰===")]
    [SerializeField] private PhotonView localPlayer;
    [SerializeField] private QuadrantType localQuadrantType;
    [SerializeField] private List<PhotonView> playerList;
    [SerializeField] private Transform[] playerField;       // 사분면 순서대로 배치되어 있어야함 

    public QuadrantType LocalQuadrantType { get => localQuadrantType;  }
    public Transform[] PlayerField { get => playerField; }

    protected override void Singleton_Awake()
    {
        
    }

    private void Start()
    {
        playerList = new List<PhotonView>();
        MemberTwoCreatePlayer();

        StartCoroutine(GenerateBulletSpawner());
    }

    private void MemberTwoCreatePlayer() 
    {
        if (PhotonNetwork.InRoom)
        {
            // 고유한 ActorNum을 가짐 (1부터시작)
            int index = PhotonNetwork.LocalPlayer.ActorNumber - 1;
            // 보통 호스트가 1으로 설정되는듯

            QuadrantType quType = (QuadrantType)index;
            Vector2 playerPosi = Define.twoMemberPoint[quType];

            // Resources 파일 하위에 동일한 이름의 오브젝트가 있어야함 ! 
            GameObject temp = PhotonNetwork.Instantiate("Player_1", playerPosi, Quaternion.identity);
            temp.GetComponent<NetPlayer>().SetIndex(index);
            // 리스트에 저장 
            playerList.Add(localPlayer);

            // 내것만 저장
            if (temp.GetComponent<PhotonView>().IsMine)
            {
                localPlayer = temp.GetComponent<PhotonView>();
                localQuadrantType = quType;

                // 내 정보 업데이트
                InGameUI.GetInstance().UpdatePlayerInfoText();
            }
        }
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
            spawner.SettingParent(index, (DirType)dir);

            if (localPlayer != null)
                spawner.SettingOwner(localPlayer.ViewID, (DirType)dir);
            else
                Debug.LogWarning("localPlayer가 NULL입니다 왜지/!??");

            yield return new WaitForSeconds(10f);
        }
    }

}

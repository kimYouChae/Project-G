using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetSpawner : MonoBehaviourPun, IPunObservable
{
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

    [PunRPC]
    public void SetParentTrasform(int playerIndex, int dir)
    {
        // 플레이어에 저장되어 있는 index , 좌상우하 방향

        Transform parent = PunIngameManager.GetInstance().PlayerField[playerIndex];

        transform.SetParent(parent);
        transform.localPosition = PunIngameManager.GetInstance().IndexToSpawnPoint[dir];
    }
}

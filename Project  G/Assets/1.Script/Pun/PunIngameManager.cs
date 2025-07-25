using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunIngameManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] spawnPoint;
    [SerializeField]
    private LayerMask[] layerList;

    private void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            int index = PhotonNetwork.LocalPlayer.ActorNumber - 1;

            // Resources 파일 하위에 동일한 이름의 오브젝트가 있어야함 ! 
            GameObject temp = PhotonNetwork.Instantiate("PlayerPrefab", spawnPoint[index].position, Quaternion.identity);
            // temp.layer = layerList[index];
        }
    }
}

using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class FourDirBullet : MonoBehaviour
{
    [SerializeField] PhotonView view;
    [SerializeField] GameObject basicBullet;

    private void Start()
    {
        view = gameObject.GetComponent<PhotonView>();

        // 4방향으로 총알 발사 
        if (view.IsMine) 
        {
            view.RPC("RPC_ShootBasciBullet", RpcTarget.AllBuffered, Vector2.up);
            view.RPC("RPC_ShootBasciBullet", RpcTarget.AllBuffered, Vector2.right);
            view.RPC("RPC_ShootBasciBullet", RpcTarget.AllBuffered, Vector2.left);
            view.RPC("RPC_ShootBasciBullet", RpcTarget.AllBuffered, Vector2.down);
        }

        // 0.5초후에 삭제
        Destroy(gameObject, 0.5f);
    }

    [PunRPC]
    public void RPC_ShootBasciBullet(Vector2 dir)
    {
        GameObject bullet = Instantiate(basicBullet, transform.position, Quaternion.identity);
        try
        {
            // 방향벡터 설정
            bullet.GetComponent<BasicBullet>().DirectVector = dir.normalized;
        }
        catch (Exception e) { Debug.Log(e); }
    }
}

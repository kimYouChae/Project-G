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

        StartCoroutine(BulletShoot());
    }

    IEnumerator BulletShoot() 
    {
        yield return new WaitForSeconds(3f);

        // 4방향으로 총알 발사 
        // 총알 발사는 RPC 여야함 -> Photonview있어야함 -> 네트워크 객체여야함
        if (view.IsMine)
        {
            view.RPC(nameof(RPC_ShootBasciBullet), RpcTarget.AllBuffered, Vector2.up);
            view.RPC(nameof(RPC_ShootBasciBullet), RpcTarget.AllBuffered, Vector2.right);
            view.RPC(nameof(RPC_ShootBasciBullet), RpcTarget.AllBuffered, Vector2.left);
            view.RPC(nameof(RPC_ShootBasciBullet), RpcTarget.AllBuffered, Vector2.down);
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

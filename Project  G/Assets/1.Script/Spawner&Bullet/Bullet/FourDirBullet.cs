using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class FourDirBullet : MonoBehaviour
{
    [SerializeField] PhotonView view;
    [SerializeField] GameObject basicBullet;

    [SerializeField] float bulletInitWait;  // four Dir Bullet 생성 후 , basic bullet 생성까지 대기시간
    [SerializeField] float bulletLifeTime;  // basic bullet 발사 후, 파괴까지 걸리는 시간 
    [SerializeField] float bulletSpeed;     // four Dir Bullet에서 발사한 basic bullet의 속도 


    public void SettingFourDirBullet(float init, float life, float speed)
    {
        view = gameObject.GetComponent<PhotonView>();

        this.bulletInitWait = init;
        this.bulletLifeTime = life;
        this.bulletSpeed = speed;

        StartCoroutine(BulletShoot());
    }

    IEnumerator BulletShoot() 
    {
        yield return new WaitForSeconds(bulletInitWait);

        // 4방향으로 총알 발사 
        // 총알 발사는 RPC 여야함 -> Photonview있어야함 -> 네트워크 객체여야함
        if (view.IsMine)
        {
            // 총알생성 
            view.RPC(nameof(RPC_ShootBasciBullet), RpcTarget.AllBuffered, Vector2.up);
            view.RPC(nameof(RPC_ShootBasciBullet), RpcTarget.AllBuffered, Vector2.right);
            view.RPC(nameof(RPC_ShootBasciBullet), RpcTarget.AllBuffered, Vector2.left);
            view.RPC(nameof(RPC_ShootBasciBullet), RpcTarget.AllBuffered, Vector2.down);

            // sfx 실행 
            SFXManager.Instance.PlaySFX(SFXType.BulletSpawnerShot);
        }

        Destroy(gameObject, bulletLifeTime);
    }

    [PunRPC]
    public void RPC_ShootBasciBullet(Vector2 dir)
    {
        GameObject bullet = Instantiate(basicBullet, transform.position, Quaternion.identity);
        try
        {
            bullet.GetComponent<BasicBullet>().SettingBasicBullet(dir.normalized, bulletSpeed);
        }
        catch (Exception e) { Debug.Log(e); }
    }
}

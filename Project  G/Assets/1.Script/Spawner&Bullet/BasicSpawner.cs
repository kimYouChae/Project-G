using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSpawner : NetSpawner
{
    [Header("Basic Spawner")]
    [SerializeField] private GameObject bulletPrefab;

    public override void StartShooting()
    {
        StartCoroutine(ShootBulletCicle());
    }

    public override void SettingBulletShootPosi()
    {
        SettingBulletShotPosi();
    }

    public override void SettingMoving()
    {
        // 플레이어 따라 이동
        SettingOwnerFollowMoving();
    }

    private IEnumerator ShootBulletCicle()
    {
        while (true)
        {
            // ## 임시 쿨타임 Nf
            float coolTime = Random.Range(3f, 5f);

            yield return new WaitForSeconds(coolTime);

            // 총알생성 RPC 실행 
            // 총알 두개 생성 방지 -> isMine 검사
            if (photonView.IsMine)
            {
                spanwerAnimator.ChangeAttackAnimation(SpanwerAnimState.Attack, true);
                view.RPC("RPC_ShootBullet", RpcTarget.AllBuffered);
            }
        }
    }

    [PunRPC]
    public void RPC_ShootBullet()
    {
        GameObject temp = Instantiate(bulletPrefab, shootPosi.position , Quaternion.identity);
        Vector3 destination = ownerTrs.position;

        // 총알에 방향벡터 지정해주기
        Vector3 dir = destination - shootPosi.position;
        temp.GetComponent<BasicBullet>().DirectVector = dir;
    }


}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedMissileSpawner : NetSpawner
{
    [Header("GuidedMissile Spawner")]
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
        // 움직임 X
    }

    private IEnumerator ShootBulletCicle()
    {
        while (true)
        {
            // ## 임시 쿨타임 Nf
            float coolTime = Random.Range(5f, 7f);

            yield return new WaitForSeconds(coolTime);

            // 총알생성 RPC 실행 
            // 총알 두개 생성 방지 -> isMine 검사
            if (photonView.IsMine)
            {
                spanwerAnimator.ChangeAttackAnimation(SpanwerAnimState.Attack, true);
                view.RPC("RPC_ShootGuideMissile", RpcTarget.AllBuffered);
            }
        }
    }

    [PunRPC]
    public void RPC_ShootGuideMissile()
    {
        GameObject temp = Instantiate(bulletPrefab, shootPosi.position, Quaternion.identity);

        temp.GetComponent<GuideMissile>().OwnerPosition = ownerTrs;
    }

}

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
        StartCoroutine(ShootBulletCycle());
    }

    public override void SettingBulletShootPosi()
    {
        SettingBulletShotPosi(); 
    }

    public override void SettingMoving()
    {
        // 움직임 X
    }

    private IEnumerator ShootBulletCycle()
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
                // 스포너 애니메이션 실행 
                spawnerAnimator.ChangeAttackAnimation(SpawnerAnimState.Attack, true);

                // 총알발사 ( 나 )
                InstantiateBullet(true);

                // 총알발사 ( 나 제외 다른 클라이언트 )
                view.RPC(nameof(RPC_ShootGuideMissile), RpcTarget.Others);

                // sfx 실행
                SFXManager.Instance.PlaySFX(SFXType.MissileSpawnerShot);
            }
        }
    }

    [PunRPC]
    public void RPC_ShootGuideMissile()
    {
        InstantiateBullet(false);
    }

    private void InstantiateBullet(bool isLocalOwner) 
    {
        // 총알생성
        GameObject temp = Instantiate(bulletPrefab, shootPosi.position, Quaternion.identity);
        GuideMissile missile = temp.GetComponent<GuideMissile>();
        if (missile != null) 
        {
            missile.OwnerPosition = ownerTrs;
            missile.IsLocalOwner = isLocalOwner;
        }

    }

}

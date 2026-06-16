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
        StartCoroutine(ShootBulletCycle());
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

    private IEnumerator ShootBulletCycle()
    {
        while (true)
        {
            float coolTime = Random.Range(spawnerData.CoolTimeMin, spawnerData.CoolTimeMax);

            yield return new WaitForSeconds(coolTime);

            // 총알생성 RPC 실행 
            // 총알 두개 생성 방지 -> isMine 검사
            if (photonView.IsMine)
            {
                // 스포너 애니메이션 실행 
                spawnerAnimator.ChangeAttackAnimation(SpawnerAnimState.Attack, true);

                // 총알발사 
                view.RPC(nameof(RPC_ShootBullet), RpcTarget.All);

                // sfx 실행
                SFXManager.Instance.PlaySFX(SFXType.BulletSpawnerShot);
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
        temp.GetComponent<BasicBullet>().SettingBasicBullet(dir, spawnerData.BulletSpeed);
    }


}

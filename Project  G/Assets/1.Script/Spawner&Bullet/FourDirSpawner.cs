using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class FourDirSpawner : NetSpawner
{
    const string fourDirSpawnerName = "FourDirSpawnObj";

    [SerializeField] GameObject fourDirBulletPrefab;
    [SerializeField] GameObject fourDirBullet;

    public override void SettingBulletShootPosi()
    {
        SettingBulletShotPosi();
    }

    public override void SettingMoving()
    {
        // 움직임 X
    }

    public override void StartShooting()
    {
        StartCoroutine(ShootBulletCycle());
    }

    private IEnumerator ShootBulletCycle()
    {
        // 위치 : 필드내 랜덤 
        // puninGameManager의 Quter값 바탕으로
        QuadrantType type = ownerTrs.GetComponent<NetPlayer>().PlayerQuadtype;

        while (true)
        {
            float coolTime = Random.Range(spawnerData.CoolTimeMin, spawnerData.CoolTimeMax);

            yield return new WaitForSeconds(coolTime);

            // 총알생성 RPC 실행 
            // 총알 두개 생성 방지 -> isMine 검사
            if (photonView.IsMine)
            {
                spawnerAnimator.ChangeAttackAnimation(SpawnerAnimState.Attack, true);

                // Debug.Log("현재 로컬의 사분면 타입 : " + type);

                float ranX = Random.Range(Define.twoMemberFieldMin[type].x, Define.twoMemberFieldMax[type].x);
                float ranY = Random.Range(Define.twoMemberFieldMin[type].y, Define.twoMemberFieldMax[type].y);

                CreateFourBulletObj(ranX, ranY);
            }
        }
    }

    private void CreateFourBulletObj(float ranX, float ranY) 
    {
        if (!photonView.IsMine)
            return;

        // Four Dir Bullet 오브젝트는 RPC를 사용해야함
        // Photon view가 있어야함 -> 포톤네트워크를 통해 생성해야 할당됨.
        // RPC 방식으로 생성 X , 포톤 네트워크 인스턴스로 생성 O
        // 데이터 동기화를 위한 object[]배열로 넘기기
        object[] data = { spawnerData.BulletInitWait, spawnerData.BulletLifeTime, spawnerData.BulletSpeed };
        var obj
            = PhotonNetwork.Instantiate(Define.DEFAULT_SPAWNER + fourDirSpawnerName, 
            new Vector3(ranX, ranY, 0), Quaternion.identity, 0, data);

        // sfx 실행 
        SFXManager.Instance.PlaySFX(SFXType.FourBulletObjPut);
    }

}

using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class FourDirSpawner : NetSpawner
{
    [Header("===FourDirSpawner===")]
    [SerializeField]
    private GameObject fourBulletSpawnObj;

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
        StartCoroutine(ShootBulletCicle());
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
                // 위치 : 필드내 랜덤 
                // puninGameManager의 Quter값 바탕으로
                QuadrantType type = ownerTrs.GetComponent<NetPlayer>().PlayerQuadtype;

                Debug.Log("현재 로컬의 사분면 타입 : " + type);

                float ranX = Random.Range(Define.twoMemberFieldMin[type].x, Define.twoMemberFieldMax[type].x);
                float ranY = Random.Range(Define.twoMemberFieldMin[type].y, Define.twoMemberFieldMax[type].y);

                view.RPC(nameof(RPC_ShootFourDirSpawn), RpcTarget.AllBuffered, ranX, ranY);
            }
        }
    }

    [PunRPC]
    public void RPC_ShootFourDirSpawn(float ranX, float ranY)
    {
        // 네방향총알(스포너) : 네트워크 객체로 동기화
        GameObject temp = Instantiate(fourBulletSpawnObj, new Vector3(ranX, ranY, 0), Quaternion.identity);
    }
}

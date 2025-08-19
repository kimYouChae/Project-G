using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourDirSpawner : NetSpawner
{
    const string FOUR_DIR_BULLET = "FourDirBullet";

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
                view.RPC("RPC_ShootFourDirSpawn", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void RPC_ShootFourDirSpawn()
    {
        // 네방향총알(스포너) : 네트워크 객체로 동기화
        GameObject temp = PhotonNetwork.Instantiate(FOUR_DIR_BULLET, shootPosi.position, Quaternion.identity);

        // 위치 : 필드내 랜덤 
        // puninGameManager의 Quter값 바탕으로
        QuadrantType type = PunIngameManager.Instance.LocalQuadrantType;
        float ranX = Random.Range(Define.twoMemberFieldMin[type].x, Define.twoMemberFieldMax[type].x);
        float ranY = Random.Range(Define.twoMemberFieldMin[type].y, Define.twoMemberFieldMax[type].y);

        temp.transform.position = new Vector3(ranX, ranY, 0);
        
    }
}

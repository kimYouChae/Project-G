using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

public class LaserSpawner : NetSpawner
{
    [Header("===Object===")]
    [SerializeField] GameObject laserChargingObjPrefab;
    [SerializeField] GameObject laserObjPreafab;
    [SerializeField] GameObject currLaserChargeObj;
    [SerializeField] GameObject currLaserObj;

    private Vector3 laserRightRotation = new Vector3(0,0, 270f);
    private Vector3 laserLeftRotation = new Vector3(0,0,90f);

    public override void StartShooting()
    {
        StartCoroutine(ShootLaserCycle());
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

    IEnumerator ShootLaserCycle() 
    {
        while (true)
        {
            float coolTime = Random.Range(spawnerData.CoolTimeMin, spawnerData.CoolTimeMax);

            yield return new WaitForSeconds(coolTime);

            // 잠시 움직임 멈춤
            canMove = false;

            if (photonView.IsMine)
            {
                // 레이저 차징 오브젝트 생성
                view.RPC(nameof(RPC_ChargingObject), RpcTarget.AllBuffered);
                // sfx 차징 사운드 실행
                SFXManager.Instance.PlaySFX(SFXType.LaserCharge);

                // charging 시간 후 삭제
                yield return new WaitForSeconds(spawnerData.ChargeTime);
                if (currLaserChargeObj != null)
                {
                    // 레이저 차징 오브젝트 삭제
                    view.RPC(nameof(RPC_DestoryChargingObj), RpcTarget.AllBuffered);
                    currLaserChargeObj = null;
                }

                // 레이저 오브젝트 생성 
                view.RPC(nameof(RPC_LaserObject), RpcTarget.AllBuffered);
                // sfx 레이저 발사 실행
                SFXManager.Instance.PlaySFX(SFXType.LaserFire);

                // charging 시간 후 삭제 
                yield return new WaitForSeconds(spawnerData.ChargeDuration);
                if (currLaserObj != null) 
                {
                    view.RPC(nameof(RPC_DestroyLaser), RpcTarget.AllBuffered);
                    currLaserObj = null;
                }
            }

            // 다시 움직임
            canMove = true;
        }
    }

    // 
    [PunRPC]
    public void RPC_ChargingObject() 
    {
        currLaserChargeObj = Instantiate(laserChargingObjPrefab, shootPosi.position, Quaternion.identity);
    }

    [PunRPC]
    public void RPC_LaserObject() 
    {
        currLaserObj = Instantiate(laserObjPreafab, shootPosi.position, Quaternion.identity);

        // 내 방향에 따라 회전값이 달라짐
        // 내가 right에 있을 때 회전 270
        // 내가 left에 있을 때 회전 90

        if (directType == DirType.Right)
        {
            currLaserObj.transform.eulerAngles = laserRightRotation;
        }
        else if (directType == DirType.Left)
        {
            currLaserObj.transform.eulerAngles = laserLeftRotation;
        }
    }

    [PunRPC]
    public void RPC_DestroyLaser() 
    {
        Destroy(currLaserObj);
    }

    [PunRPC]
    public void RPC_DestoryChargingObj() 
    {
        Destroy(currLaserChargeObj);
    }
}

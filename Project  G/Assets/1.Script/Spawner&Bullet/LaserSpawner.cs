using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LaserSpawner : NetSpawner
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] bool isMoveLaser = false;
    [SerializeField] bool flag = true;
    [SerializeField] bool isEffectPlaying = true;

    public override void StartShooting()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;    // 점 두개
        lineRenderer.enabled = false;       // 처음엔 안보이게 

        StartCoroutine(Test());
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

    private void Update()
    {
        if (isMoveLaser) 
        {
            LaserPlay();

            if (flag) 
            {
                // 깜빡깜빡 효과
                view.RPC("RPC_LaserEffect", RpcTarget.AllBuffered);
                flag = false;
            }
        }
    }

    private void LaserPlay() 
    {
        // 첫번째 점
        lineRenderer.SetPosition(0, shootPosi.position);

        // 왼쪽에 있을 때 
        if (directType == DirType.Left) 
        {
            // 두번째 점 
            lineRenderer.SetPosition(1, shootPosi.position + new Vector3(10f, 0, -0.1f));
        }
        // 오른쪽에 있을 때 
        else if (directType == DirType.Right) 
        {
            // 두번째 점 
            lineRenderer.SetPosition(1, shootPosi.position + new Vector3(-10f, 0, -0.1f));
        }
    }

    IEnumerator Test() 
    {
        while (true)
        {
            // ## 임시 쿨타임 Nf
            float coolTime = Random.Range(5f, 7f);

            yield return new WaitForSeconds(coolTime);

            // 총알생성 RPC 실행 
            // 총알 두개 생성 방지 -> isMine 검사
            if (photonView.IsMine)
                view.RPC("RPC_ShootLaser", RpcTarget.AllBuffered);
        }
    }

    IEnumerator LaserEffect() 
    {

        Debug.Log("레이저 발사");
        for(float i = 1f; i >= -0.01f; i -= 0.2f) 
        {
            lineRenderer.enabled = false;
            yield return new WaitForSeconds(i);
            lineRenderer.enabled = true;
            yield return new WaitForSeconds(i);
        }

        lineRenderer.enabled = false;

        // 레이캐스트 후 플레이어 검사
        PlayerCheck();
    }

    private void PlayerCheck() 
    {
        RaycastHit2D hit = default;
        if (directType == DirType.Left)
        {
            hit = Physics2D.Raycast(transform.position, Vector2.right);
        }
        // 오른쪽에 있을 때 
        else if (directType == DirType.Right)
        {
            hit = Physics2D.Raycast(transform.position, Vector2.left);
        }

        // 플레이어와 충돌
        if (hit.collider.gameObject.layer == 6) 
        {
            try 
            {
                NetPlayer player = hit.collider.GetComponent<NetPlayer>();
                player.DiePlayer();
            }
            catch(Exception e) { Debug.Log(e);  }
        }
    }

    [PunRPC]
    public void RPC_ShootLaser()
    {
        isMoveLaser = true;

        lineRenderer.enabled = true;
    }

    [PunRPC]
    public void RPC_LaserEffect() 
    {
        StartCoroutine(LaserEffect());
    }

}

using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LaserSpawner : NetSpawner
{
    [Header("===LaserSpawner===")]
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] bool isMoveLaser = false;
    [SerializeField] bool flag = true;

    Vector3 laserDistanceToLeft = new Vector3(-20,0,0);
    Vector3 laserDistanceToRight = new Vector3(20,0,0);
    Vector3 laserZDistance = new Vector3(0, 0, -0.01f);

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
        if (photonView.IsMine == false)
            return;

        if (isMoveLaser) 
        {
            Vector3 p0 = shootPosi.position;
            Vector3 p1 = directType == DirType.Left ?
                shootPosi.position + (laserDistanceToRight + laserZDistance) :
                shootPosi.position + (laserDistanceToLeft + laserZDistance);

            view.RPC(nameof(RPC_DrawLine), RpcTarget.AllBuffered, p0, p1);

            if (flag) 
            {
                // 깜빡깜빡 효과
                view.RPC(nameof(RPC_LaserEffect), RpcTarget.AllBuffered);
                flag = false;
            }
        }
    }

    IEnumerator Test() 
    {
        while (true)
        {
            // ## 임시 쿨타임 Nf
            float coolTime = Random.Range(5f, 7f);

            yield return new WaitForSeconds(coolTime);

            if (photonView.IsMine)
                view.RPC(nameof(RPC_ShootLaser), RpcTarget.AllBuffered);
        }
    }

    IEnumerator LaserEffect() 
    {
        Debug.Log("레이저 발사");
        for(float i = 1f; i >= -0.01f; i -= 0.2f) 
        {
            view.RPC(nameof(RPC_OnOffLineRenderer), RpcTarget.AllBuffered, false);
            yield return new WaitForSeconds(i);
            view.RPC(nameof(RPC_OnOffLineRenderer), RpcTarget.AllBuffered, true);
            yield return new WaitForSeconds(i);
        }

        view.RPC(nameof(RPC_OnOffLineRenderer), RpcTarget.AllBuffered, false);
        isMoveLaser = false;
        flag = true;

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

        if (hit == default)
            return;

        // 플레이어와 충돌
        if (hit.collider.gameObject.layer == 6) 
        {
#if !UNITY_EDITOR
            try 
            {
                NetPlayer player = hit.collider.GetComponent<NetPlayer>();
                player.DiePlayer();
            }
            catch(Exception e) { Debug.Log(e);  }
#endif
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

    [PunRPC]
    public void RPC_DrawLine(Vector3 p0, Vector3 p1) 
    {
        // 첫번째 점
        lineRenderer.SetPosition(0, p0);
        lineRenderer.SetPosition(1, p1);
    }

    [PunRPC]
    public void RPC_OnOffLineRenderer(bool flag) 
    {
        if(lineRenderer != null)
            lineRenderer.enabled = flag;
    }

}

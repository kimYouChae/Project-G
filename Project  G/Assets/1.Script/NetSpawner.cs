using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetSpawner : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform ownerTrs;    // 따라다닐 기준이 되는 trs
    [SerializeField] private DirType directType;    // 내가 위치한 방향 
    [SerializeField] private Action moveNetSpawner;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine == false)
            return;

        moveNetSpawner?.Invoke();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 스트림에 데이터 쓰기
        if (stream.IsWriting) 
        {
            stream.SendNext(transform.position);
        }
        if(stream.IsReading) 
        {
            object posobj = stream.ReceiveNext();
            if (posobj is Vector3 pos)
            {
                transform.position = pos;
            }
            else 
            {
                Debug.LogError("NetSpawner : Vector3 형변환 실패" + posobj.GetType());
            }
        }
    }

    public void SettingOwner(Transform trs, DirType type) 
    {
        this.ownerTrs = trs;
        this.directType = type;

        // 방향에 따라 움직임 다르게 
        switch (directType)
        {
            case DirType.Left:
            case DirType.Right:
                moveNetSpawner += MoveFllowToUpDown;
                break;
            case DirType.Top:
            case DirType.Bottom:
                moveNetSpawner += MoveFllowToLeftRIght;
                break;
        }
    }

    private void MoveFllowToUpDown() 
    {
        // 목표지점 - 내위치 = 방향벡터 
        float directionY = ownerTrs.position.y - transform.position.y;

        // 절댓값이 1 이하면 -> 작은떨림 방지 
        if (Mathf.Abs(directionY) < 1f) 
        {
            rb.velocity = Vector3.zero;
            return;
        }
        
        rb.velocity = new Vector2(0, directionY).normalized * 3f;
    }

    private void MoveFllowToLeftRIght() 
    {
        // 목표지점 - 내위치 = 방향벡터 
        float directionX = ownerTrs.position.x - transform.position.x;

        // 절댓값이 1 이하면 -> 작은떨림 방지 
        if (Mathf.Abs(directionX) < 1f)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        rb.velocity = new Vector2(directionX, 0).normalized * 3f;
    }

    [PunRPC]
    public void SetParentTrasform(int playerIndex, int dir)
    {
        // 플레이어에 저장되어 있는 index , 좌상우하 방향

        Transform parent = PunIngameManager.GetInstance().PlayerField[playerIndex];

        transform.SetParent(parent);
        transform.localPosition = PunIngameManager.GetInstance().IndexToSpawnPoint[dir];
    }
}

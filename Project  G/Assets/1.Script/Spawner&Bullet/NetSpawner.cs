using Photon.Pun;
using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

public class NetSpawner : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private PhotonView view;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform ownerTrs;    // 따라다닐 기준이 되는 trs
    [SerializeField] private DirType directType;    // 내가 위치한 방향 
    [SerializeField] private Action moveNetSpawner;

    [Header("===Bullet===")]
    [SerializeField] private Transform shootPosi;   //총알 쏠 위치 
    [SerializeField] private GameObject bulletPrefab;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        view = GetComponent<PhotonView>();
    }

    private void Start()
    {
        StartCoroutine(ShootBulletCicle());
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

    public void SettingParent(int index, int dir) 
    {
        view.RPC("RPC_SetParentTrasform", RpcTarget.AllBuffered, index, dir);
    }

    public void SettingOwner(int viewId, DirType type) 
    {
        // this.ownerTrs = trs;
        // owner 지정은 RPC : view아이디는 로컬의 플레이어 id
        view.RPC("RPC_SettingOwner", RpcTarget.AllBuffered , viewId);

        this.directType = type;

        // 방향에 따라 움직임 다르게 
        switch (directType)
        {
            case DirType.Left:
                moveNetSpawner += MoveFllowToUpDown;

                // 회전 동기화 
                view.RPC("RPC_SettingAngle", RpcTarget.AllBuffered, new Vector3(0, 0, -90f));
                break;
            case DirType.Right:
                moveNetSpawner += MoveFllowToUpDown;

                // 회전 동기화
                view.RPC("RPC_SettingAngle", RpcTarget.AllBuffered, new Vector3(0, 0, 90f));
                break;
            case DirType.Top:
                moveNetSpawner += MoveFllowToLeftRIght;

                // 회전 동기화
                view.RPC("RPC_SettingAngle", RpcTarget.AllBuffered, new Vector3(0, 0, -180));
                break;
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

    private IEnumerator ShootBulletCicle() 
    {
        while (true) 
        {
            // ## 임시 쿨타임 Nf
            float coolTime = Random.Range(3f, 5f);
            
            yield return new WaitForSeconds(coolTime);

            // 총알생성 RPC 실행 
            // 총알 두개 생성 방지 -> isMine 검사
            if(photonView.IsMine)
                view.RPC("RPC_ShootBullet", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void RPC_SetParentTrasform(int playerIndex, int dir)
    {
        // 플레이어에 저장되어 있는 index , 좌상우하 방향

        Transform parent = PunIngameManager.GetInstance().PlayerField[playerIndex];

        transform.SetParent(parent);
        transform.localPosition = PunIngameManager.GetInstance().IndexToSpawnPoint[dir];
    }

    [PunRPC]
    public void RPC_ShootBullet() 
    {
        GameObject temp = Instantiate(bulletPrefab, shootPosi );
        Vector3 destination = ownerTrs.position;

        // 총알에 방향벡터 지정해주기
        Vector3 dir = destination - shootPosi.position;
        temp.GetComponent<BasicBullet>().DirectVector = dir;
    }

    [PunRPC]
    public void RPC_SettingOwner(int viewID) 
    {
        PhotonView temp = PhotonView.Find(viewID);

        if (temp != null)
            ownerTrs = temp.transform;
        else
            Debug.Log($"{viewID}에 해당하는 PhotonView없음");
    }

    [PunRPC]
    public void RPC_SettingAngle(Vector3 enAle) 
    {
        transform.eulerAngles = enAle;
    }
}

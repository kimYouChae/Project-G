using Photon.Pun;
using System;
using System.Collections;
using System.Reflection;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


public abstract class NetSpawner : MonoBehaviourPun, IPunObservable
{
    [Header("===Component===")]
    [SerializeField] protected PhotonView view;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected Transform ownerTrs;    // 따라다닐 기준이 되는 trs
    [SerializeField] protected SpanwerAnimator spanwerAnimator;
    

    [SerializeField] protected DirType directType;    // 내가 위치한 방향 
    [SerializeField] protected Action moveNetSpawner;
    [SerializeField] protected bool canMove = true;

    [Header("===Data===")]
    [SerializeField] SpawnerData spawnerData;

    [Header("===Bullet===")]
    [SerializeField] protected Transform[] shootPosiList;   //총알 쏠 위치 - left,top,right,bottom 순
    [SerializeField] protected Transform shootPosi;   // 현재 총 쏠 위치 

    private Vector3 velRefX = Vector3.zero;     // SmoothDamp 내부 상태
    private Vector3 velRefY = Vector3.zero;     // SmoothDamp 내부 상태
    const float stopEps = 1f;                 // owner와 거리, N 이하이면 움직이지않음

    // 총알 발사 시작
    public abstract void StartShooting();

    // 하위 스포너에서 움직임 세팅 
    public abstract void SettingMoving();

    // 하위 스포너에서 총알 발사 위치 세팅
    public abstract void SettingBulletShootPosi();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        view = GetComponent<PhotonView>();

        // 애니메이터 초기화 
        if(spanwerAnimator == null)
            spanwerAnimator = gameObject.AddComponent<SpanwerAnimator>();
        spanwerAnimator.SetAnimator(GetComponent<Animator>());
    }

    private void Start()
    {
        StartShooting();
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine == false)
            return;

        if(canMove)
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

    public void SettingParent(int index, DirType dir) 
    {
        view.RPC("RPC_SetParentTrasform", RpcTarget.AllBuffered, index, dir);
    }

    public void SettingOwner(int viewId, DirType type) 
    {
        // this.ownerTrs = trs;
        // owner 지정은 RPC : view아이디는 로컬의 플레이어 id
        view.RPC("RPC_SettingOwner", RpcTarget.AllBuffered , viewId);
    }

    public void SettingDir(DirType type) 
    {
        this.directType = type;
    }

    public void SettingSpawnerData(SpawnerType type) 
    {
        var temp = SpanwerDataManager.Instance.spanwerData(type);
        if (temp != null) 
        {
            spawnerData = temp;
        }
    }

    protected void SettingOwnerFollowMoving() 
    {
        // dirType세팅 후
        // 방향에 따라 움직임 다르게 
        switch (directType)
        {
            case DirType.Left:
                moveNetSpawner += MoveFllowToUpDown;
                break;
            case DirType.Right:
                moveNetSpawner += MoveFllowToUpDown;
                break;
            case DirType.Top:
                moveNetSpawner += MoveFllowToLeftRIght;
                break;
            case DirType.Bottom:
                moveNetSpawner += MoveFllowToLeftRIght;
                break;
        }
    }

    protected void SettingBulletShotPosi() 
    {
        // dirType세팅 후
        // 방향에 따라 움직임 다르게 
        switch (directType)
        {
            case DirType.Left:
                view.RPC("RPC_SettingAngle", RpcTarget.AllBuffered, DirType.Right);
                break;
            case DirType.Right:
                view.RPC("RPC_SettingAngle", RpcTarget.AllBuffered, DirType.Left);
                break;
            case DirType.Top:
                view.RPC("RPC_SettingAngle", RpcTarget.AllBuffered, DirType.Bottom);
                break;
            case DirType.Bottom:
                view.RPC("RPC_SettingAngle", RpcTarget.AllBuffered, DirType.Top);
                break;
        }
    }

    private void MoveFllowToUpDown() 
    {
        // 목표지점 - 내위치 = 방향벡터 
        float directionY = ownerTrs.position.y - transform.position.y;

        // 절댓값이 1 이하면 -> 작은떨림 방지 
        if (Mathf.Abs(directionY) < stopEps) 
        {
            velRefY = Vector3.zero;
            return;
        }

        Vector3 current = transform.position;
        Vector3 target = new Vector3(current.x, ownerTrs.position.y, current.z);

        transform.position = Vector3.SmoothDamp(
            current, target, ref velRefY, spawnerData.Acceleration, spawnerData.Speed, Time.deltaTime);
    }

    private void MoveFllowToLeftRIght() 
    {
        // 목표지점 - 내위치 = 방향벡터 
        float directionX = ownerTrs.position.x - transform.position.x;

        // 절댓값이 1 이하면 -> 작은떨림 방지 
        if (Mathf.Abs(directionX) < stopEps)
        {
            velRefX = Vector3.zero;
            return;
        }

        Vector3 current = transform.position;
        Vector3 target = new Vector3(ownerTrs.position.x, current.y, current.z);

        transform.position = Vector3.SmoothDamp(
            current, target, ref velRefX, spawnerData.Acceleration, spawnerData.Speed, Time.deltaTime);

    }


    [PunRPC]
    public void RPC_SetParentTrasform(int playerIndex, DirType dir)
    {
        // 플레이어에 저장되어 있는 index , 좌상우하 방향

        Transform parent = PunIngameManager.Instance.PlayerField[playerIndex];

        transform.SetParent(parent);
        transform.localPosition = Define.twoMemberSpawnerPoint[dir];
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
    public void RPC_SettingAngle(DirType type) 
    {
        shootPosi = shootPosiList[(int)type];
    }
}

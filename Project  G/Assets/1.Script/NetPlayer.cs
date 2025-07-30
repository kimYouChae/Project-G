using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public class NetPlayer : MonoBehaviourPun, IPunObservable
{
    [Header("===Info===")]
    [SerializeField] private int playerIndex;

    [Header("===Move===")]
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private Vector3 dir;

    [Header("===Component===")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PhotonView view;
    [SerializeField] NetPlayerAnimator netAnimator;
    [SerializeField] private Animator animator;

    [Header("===Test===")]
    [SerializeField] bool flag = true; // true : 테스트할 때 충돌 x 

    public int PlayerIndex { get => playerIndex; }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        view = GetComponent<PhotonView>();
        netAnimator = GetComponent<NetPlayerAnimator>();
        netAnimator.SetAnimator(animator);
    }

    private void FixedUpdate()
    {
        // 내거만 조종 가능
        if (photonView.IsMine == false)
            return;

        dir.x = Input.GetAxisRaw("Horizontal");
        dir.y = Input.GetAxisRaw("Vertical");

        if (dir.x < 0)  // 왼
            netAnimator.ChangeAnimation(CharaterAniState.left);
        if(dir.x > 0 )  // 오
            netAnimator.ChangeAnimation(CharaterAniState.right);
        if(dir.y < 0)   // 아래
            netAnimator.ChangeAnimation(CharaterAniState.back);
        if(dir.y > 0 )  // 위
            netAnimator.ChangeAnimation(CharaterAniState.front);
        if(dir.x == 0 && dir.y == 0)    // 가만히
            netAnimator.ChangeAnimation(CharaterAniState.none); 

        rb.velocity = dir.normalized * speed;
    }

    public void SetIndex(int idx) 
    {
        this.playerIndex = idx;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 스트림에 데이터 쓰기 
        // isMine이 true인것
        if(stream.IsWriting) 
        {
            // 로컬 데이터 전송
            stream.SendNext(transform.position);
            //stream.SendNext(transform.rotation);
        }

        // 데이터 읽기
        // isMine이 false인것
        // PhotonView의 오브젝트 고유ID로 매핑
        if (stream.IsReading) 
        {
            // 다른사람이 조종하는 객체 
            // 데이터 수신
            object posObj = stream.ReceiveNext();
            if (posObj is Vector3 pos)
            {
                transform.position = pos;
            }
            else
            {
                Debug.LogError("NetPlayer : Position 형변환 실패: " + posObj.GetType());
            }

            /*
            object rotObj = stream.ReceiveNext();
            if (rotObj is Quaternion rot)
            {
                transform.rotation = rot;
            }
            else
            {
                Debug.LogError("Rotation 형변환 실패: " + rotObj.GetType());
            }
            */
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!photonView.IsMine)
            return;

#if UNITY_EDITOR
        // 테스트용 -> 충돌 x 
        if (flag)
            return;
#endif

        // 임시 총알 레이어 번호 설정 
        if (collision.gameObject.layer == 7)
        {
            view.RPC("RPC_TriggerBullet", RpcTarget.AllBuffered, photonView.ViewID) ;
        }
    }

    [PunRPC]
    public void RPC_TriggerBullet(int viewId) 
    {
        Debug.Log($"충돌했습니다");

        PhotonView view = PhotonView.Find(viewId);
        if (view != null) 
        {
            InGameUI.GetInstance().HighlightPlayer(view.transform);
        }

        TimeManager.Stop();
    }
}

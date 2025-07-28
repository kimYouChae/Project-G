using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public class NetPlayer : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private Vector3 dir;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private int playerIndex;

    public int PlayerIndex { get => playerIndex; }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // 내거만 조종 가능
        if (photonView.IsMine == false)
            return;

        dir.x = Input.GetAxisRaw("Horizontal");
        dir.y = Input.GetAxisRaw("Vertical");

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
}

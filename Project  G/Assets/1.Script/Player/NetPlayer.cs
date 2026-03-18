using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public class NetPlayer : MonoBehaviourPun, IPunObservable
{
    [Header("===Info===")]
    [SerializeField] private QuadrantType playerQuadtype;
    [SerializeField] private int playerIndex;

    [Header("===Move===")]
    [SerializeField] private bool isReadToMove = false;
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private Vector3 dir;
    [SerializeField] private Vector3 lastMoveDir = Vector3.down; // 기본 정면 아래
    
    [Header("===Component===")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PhotonView view;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color originalColor;

    [Header("===Script===")]
    [SerializeField] NetPlayerAnimator netAnimator;
    [SerializeField] IPlayerSkill playerSkill;

    [Header("===Test===")]
    [SerializeField] bool flag = true; // true : 테스트할 때 충돌 x 

    public int PlayerIndex { get => playerIndex; }
    public bool IsReadToMove { get => isReadToMove; set => isReadToMove = value; }
    public QuadrantType PlayerQuadtype { get => playerQuadtype; }
    public Vector3 Dir { get => dir; }
    public Vector3 LastMoveDir => lastMoveDir;
    public Color OriginalColor { get => originalColor; }

    private void Start()
    {
        // 컴포넌트
        rb = GetComponent<Rigidbody2D>();
        view = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        // 스크립트
        netAnimator = GetComponent<NetPlayerAnimator>();
        playerSkill = GetComponent<IPlayerSkill>();
        netAnimator.SetAnimator(animator);

        // 스킬인터페이스 - start
        playerSkill.IOnStart(this);
    }

    private void FixedUpdate()
    {
        if (!isReadToMove)
            return;

        // 내거만 조종 가능
        if (photonView.IsMine == false)
            return;

        dir.x = Input.GetAxisRaw("Horizontal");
        dir.y = Input.GetAxisRaw("Vertical");

        if (dir.x < 0)  // 왼
            netAnimator.ChangeAnimation(CharaterAniState.left);
        if (dir.x > 0)  // 오
            netAnimator.ChangeAnimation(CharaterAniState.right);
        if (dir.y < 0)   // 아래
            netAnimator.ChangeAnimation(CharaterAniState.back);
        if (dir.y > 0)  // 위
            netAnimator.ChangeAnimation(CharaterAniState.front);
        if (dir.x == 0 && dir.y == 0)    // 가만히
            netAnimator.ChangeAnimation(CharaterAniState.none);

        // 입력이 있을 때 (이동할 때)
        if (dir != Vector3.zero)
        {
            lastMoveDir = dir.normalized; 
            rb.velocity = lastMoveDir * speed;

            // 걷는 사운드 실행
            SFXManager.Instance.PlayCharacterFootStep(SFXType.CharacterFootstep);
        }
        // 입력이 없을 때 (멈출때)
        else
        {
            rb.velocity = Vector2.zero;

            // 걷는 사운드 멈추기
            SFXManager.Instance.StopCharacterFootStep(SFXType.CharacterFootstep);
        }

        // 앞 방향 시각화
        Debug.DrawRay(transform.position, lastMoveDir * 2f, Color.red);
    }

    public void SetIndex(QuadrantType qType)
    {
        this.playerQuadtype = qType;
        this.playerIndex = (int)qType;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 스트림에 데이터 쓰기 
        // isMine이 true인것
        if (stream.IsWriting)
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

        // 테스트용 -> 충돌 x 
        if (flag)
            return;

        // 임시 총알 레이어 번호 설정 
        if (collision.gameObject.layer == 7)
        {
            // 총알 삭제
            // ##TODO : 추후 pooling 추가 예정
            Destroy(collision.gameObject);

            // 스킬인터페이스 - start
            playerSkill.IOnCollision(this, collision);
        }
    }

    public void DiePlayer() 
    {
        // 사망 로직 동기화 
        view.RPC(nameof(RPC_TriggerBullet), RpcTarget.AllBuffered, photonView.ViewID);

        // 사망 사운드
        SFXManager.Instance.PlaySFX(SFXType.CharacterDeath);
    }

    [PunRPC]
    public void RPC_TriggerBullet(int viewId) 
    {
        Debug.Log($"충돌했습니다");

        // 호스트,클라 공통 동작
        // 1. 시간 멈추기
        TimeManager.Stop();
        // 2. 점수-시간 변동 x 
        ScoreManager.Instance.IsReadyToCount = false;

        // UI 표시
        PhotonView view = PhotonView.Find(viewId);
        if (view != null)
        {
            InGameUI.Instance.HighlightPlayer(view.transform);
        }

        // ONLY 호스트
        if (PhotonNetwork.IsMasterClient) 
        {
            PunGameoverManager.Instance.GameOver();
        }
    }

    public void ChangeColor(Color color) 
    {
        spriteRenderer.color = color;
    }
}

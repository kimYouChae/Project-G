using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MerchantStausType 
{
    Move,
    Stop
}

public class Merchant : OutOfBounds
{
    [SerializeField]
    private MerchantPatternType patternType;
    [SerializeField]
    private DirType dirType;
    [SerializeField]
    private Vector2 directVector;
    [SerializeField]
    private float stopX;
    [SerializeField]
    private MerchantStausType stausType;

    [Header("===Component===")]
    [SerializeField]
    private Rigidbody2D rb;

    // ##TODO : 주민 속도 / 인식 범위 / stop했을 때 대기시간 -> 하드코딩 바꾸기 필요
    const float speed = 3f;
    const float sight = 1.5f;
    const float waitTime = 2f;
    private bool isConnect = false; // stopX에 마주쳤는지
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetupMerchant(MerchantPatternType mtype, DirType dirtype, float stopX) 
    {
        this.patternType = mtype;
        this.dirType = dirtype;
        this.stopX = stopX;


        // dir방향이 left -> 오른쪽으로 가야함
        if (dirtype == DirType.Left) directVector = Vector2.right;
        // dir방향이 right -> 왼쪽으로 가야함
        else if(dirtype == DirType.Right) directVector = Vector2.left;
        //잘못들어오면 일단 Up
        else directVector = Vector2.up;

        stausType = MerchantStausType.Move;
        rb.velocity = speed * directVector;
    }

    protected override void UpdateLogic()
    {
        // 혹시 몰라 방어코드
        if (stausType == MerchantStausType.Stop) return;

        // 목표가 sight 내에 들어오면 멈추기
        // ( 수다떠는 주민 때문에 , 물건 두고 가는 주민도 적용 ( 정확히 stopX에 안멈춰도됨 )
        // 내 위치 - 목표 위치의 절대값이 sight 이하이면 
        if (!isConnect && Math.Abs(transform.position.x - stopX) < sight )
        {
            StartCoroutine(Temp());
            isConnect = true;
        }
    }

    IEnumerator Temp() 
    {
        stausType = MerchantStausType.Stop;
        rb.velocity = 0 * directVector;

        // 대기 
        yield return new WaitForSeconds(waitTime);
        // 주민 별 각 동작 실행
        // ex) 짐을 내려놓는다 or 이야기 애니메이션 실행 

        rb.velocity = speed * directVector;
        stausType = MerchantStausType.Move;
    }

}

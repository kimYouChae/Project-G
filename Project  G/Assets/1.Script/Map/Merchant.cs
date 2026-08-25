using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

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
    [SerializeField]
    private IMerchant merchantLogic;
    [SerializeField]
    private MerchantData merchantData;

    private float targetX;  // StopX 와 주민의 Sight로 정해지는 목표 X 
    private bool isConnect = false; // stopX에 마주쳤는지
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        TryGetComponent<IMerchant>(out merchantLogic);
    }

    public void SetupMerchant(MerchantPatternType mtype, DirType dirtype, float stopX) 
    {
        merchantData = MerchantManager.Instance.GetData(mtype);
        if(merchantLogic != null)
            merchantLogic.IOnStart(merchantData);

        this.patternType = mtype;
        this.dirType = dirtype;
        this.stopX = stopX;

        // dir방향이 left -> 오른쪽으로 가야함
        if (dirtype == DirType.Left) directVector = Vector2.right;
        // dir방향이 right -> 왼쪽으로 가야함
        else if(dirtype == DirType.Right) directVector = Vector2.left;
        //잘못들어오면 일단 Up
        else directVector = Vector2.up;

        targetX = dirType == DirType.Left ?
            stopX - merchantData.Sight :
            stopX + merchantData.Sight;
        stausType = MerchantStausType.Move;
        rb.velocity = merchantData.Speed * directVector;
    }

    protected override void UpdateLogic()
    {
        // 혹시 몰라 방어코드
        if (stausType == MerchantStausType.Stop) return;

        // 멈춰야 하거나
        // connect 한 적이 없거나 
        // 각 주민의 stop 조건을 만족하면 
        // -> Stop후 코루틴 시작 
        if (merchantData.ShouldStop 
            && !isConnect 
            && IsReached())
        {
            StartCoroutine(Temp());
            isConnect = true;
        }
    }

    private bool IsReached() 
    {
        return dirType == DirType.Left ? 
            transform.position.x >= targetX   // 왼쪽 -> 오른쪽으로 갈 때, 내가 target 보다 커지면 도착(=지나감)
            : transform.position.x <= targetX; // 오른쪽 -> 왼쪽으로 갈 때 , 내가 target 보다 작아지면 도착 (=지나감)
    }

    IEnumerator Temp() 
    {
        stausType = MerchantStausType.Stop;
        rb.velocity = 0 * directVector;

        // 주민 별 각 동작 실행
        // ex) 짐을 내려놓는다 or 이야기 애니메이션 실행 
        if (merchantLogic != null)
            yield return merchantLogic.IMerChantLogic(stopX);

        rb.velocity = merchantData.Speed * directVector;
        stausType = MerchantStausType.Move;
    }

}

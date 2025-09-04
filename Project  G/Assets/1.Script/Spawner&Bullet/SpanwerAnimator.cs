using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class SpanwerAnimator : MonoBehaviour
{
    [SerializeField]
    Dictionary<SpanwerAnimState, Action> spanwerAniStateToAction;
    [SerializeField]
    private Animator animator;

    const string AttackParameter = "isAttack";

    // flag가 true이면 attack끝난후 바로 idle로 전환, 
    // `` false이면 명시적으로 idle로 전환 필요 
    public void ChangeAttackAnimation(SpanwerAnimState nextState , bool flag) 
    {
        Action action;
        if (spanwerAniStateToAction.TryGetValue(nextState, out action))
        {
            action?.Invoke();

            // flag가 true일때만
            if(flag)
                StartCoroutine(ChangeToIdleImmediately());
        }
    }

    public void ChangeToIdle(SpanwerAnimState nextState) 
    {
        Action action;
        if (spanwerAniStateToAction.TryGetValue(nextState, out action))
        {
            action?.Invoke();
        }
    }

    IEnumerator ChangeToIdleImmediately() 
    {
        int hash = Animator.StringToHash("SlimeAttackBase");

        // 상태까지 대기
        while (animator.GetCurrentAnimatorStateInfo(0).shortNameHash != hash)
        {
            yield return null;
        }

        // 진입 완료 후 끝날 때까지 대기
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        Debug.Log("Attack 끝남 -> Idle 전환");
        ChangeToIdle(SpanwerAnimState.Idle);
    }

    public void SetAnimator(Animator ani)
    {
        this.animator = ani;
        spanwerAniStateToAction = new Dictionary<SpanwerAnimState, Action>() 
        {
            { SpanwerAnimState.Idle, () =>
            {
                animator.SetBool(AttackParameter , false);
            } },
            { SpanwerAnimState.Attack, () =>
            {
                animator.SetBool(AttackParameter , true);
            } }
        };
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMerchant : MonoBehaviour, IMerchant
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private MerchantData data;

    public void IOnStart(MerchantData data)
    {
        this.data = data;
    }

    public IEnumerator IMerChantLogic(float stopX = 0)
    {
        if (animator != null)
        {
            // 정차 애니메이션 실행
            animator.SetBool("", true);

            yield return data.WaitTime;

            // 정차 애니메이션 끝 
            animator.SetBool("", false);
        }
    }


}

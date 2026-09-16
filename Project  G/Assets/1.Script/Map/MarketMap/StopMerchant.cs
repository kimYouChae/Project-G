using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMerchant : MonoBehaviour, IMerchant
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private MerchantData data;

    const string MiningParemeter = "Mining";

    public void IOnStart(MerchantData data)
    {
        this.data = data;
    }

    public IEnumerator IMerChantLogic(float stopX = 0)
    {
        if (animator != null)
        {
            // 스탑 애니메이션 실행
            animator.SetTrigger(MiningParemeter);

            yield return new WaitForSeconds(data.WaitTime);
        }
    }


}

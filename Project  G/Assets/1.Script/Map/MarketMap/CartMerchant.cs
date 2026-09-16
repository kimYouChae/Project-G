using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartMerchant : MonoBehaviour, IMerchant
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private MerchantData data;

    [SerializeField]
    private GameObject appleObj;

    const string DropParemeter = "Drop";

    public void IOnStart(MerchantData data)
    {
        this.data = data;
    }

    public IEnumerator IMerChantLogic(float stopX = 0)
    {
        if (animator != null) 
        {
            animator.SetTrigger(DropParemeter);

            // 사과 떨어트리기, StopX 위치에 
            Vector2 posi = new Vector2(stopX, transform.position.y);
            Instantiate(appleObj, posi, Quaternion.identity);

            yield return new WaitForSeconds(data.WaitTime);
        }
    }


}

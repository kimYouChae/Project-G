using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MeetingMerchant : MonoBehaviour, IMerchant
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private MerchantData data;

    [SerializeField]
    private GameObject bubbleObj;
    [SerializeField]
    private Animator bubbleAnimator;

    const string TalkParameter = "Talk";

    public void IOnStart(MerchantData data)
    {
        this.data = data;
    }

    public IEnumerator IMerChantLogic(float stopX = 0)
    {
        if (animator != null) 
        {
            bubbleObj.SetActive(true);
            animator.SetTrigger(TalkParameter);

            yield return new WaitForSeconds(data.WaitTime);

            bubbleObj.SetActive(false);
        }
    }

}

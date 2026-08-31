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
    private float meetWaitTime;

    [SerializeField]
    private GameObject bubbleObj;
    [SerializeField]
    private Animator bubbleAnimator;

    const string TalkParameter = "Talk";

    public void SetWaitTime(float waitTime) 
    {
        this.meetWaitTime = waitTime;
    }

    public void IOnStart(MerchantData data)
    {
        this.data = data;
    }

    public IEnumerator IMerChantLogic(float stopX = 0)
    {
        if (animator != null) 
        {
            // 다른 주민이 올 때 까지 대기 
            // ( 호스트에서 계산 후 이벤트전파 )
            yield return new WaitForSeconds(meetWaitTime);

            bubbleObj.SetActive(true);
            animator.SetTrigger(TalkParameter);

            yield return new WaitForSeconds(data.WaitTime);

            bubbleObj.SetActive(false);
        }
    }

}

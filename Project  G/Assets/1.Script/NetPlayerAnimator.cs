using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharaterAniState
{
    none,front, back, left, right
}

public class NetPlayerAnimator : MonoBehaviour
{
    [SerializeField]
    Dictionary<CharaterAniState, Action> charaterAniStateToAction;
    [SerializeField]
    private Animator animator;

    const string backParameter = "isWalk";
    const string frontParameter = "isBackWalk";
    const string leftParemeter = "isLeftWalk";
    const string rightParameter = "isRightWalk";

    public void ChangeAnimation( CharaterAniState nextState ) 
    { 
        Action action;
        if(charaterAniStateToAction.TryGetValue( nextState, out action) ) 
        {
            action?.Invoke();
        }
    }
    

    public void SetAnimator(Animator ani) 
    {   
        this.animator = ani;
        charaterAniStateToAction = new Dictionary<CharaterAniState, Action>() 
        {
            { CharaterAniState.none , () =>
            {
                animator.SetBool(frontParameter , false);
                animator.SetBool(backParameter , false);
                animator.SetBool (rightParameter , false);
                animator.SetBool(leftParemeter , false);
            } } ,
            { CharaterAniState.front , () => 
            {
                animator.SetBool(frontParameter , true);
                animator.SetBool(backParameter , false);
                animator.SetBool (rightParameter , false);
                animator.SetBool(leftParemeter , false);
            } } ,
            { CharaterAniState.right , () =>
            {
                animator.SetBool(frontParameter , false);
                animator.SetBool(backParameter , false);
                animator.SetBool (rightParameter , true);
                animator.SetBool(leftParemeter , false);
            } } ,
            { CharaterAniState.left , () =>
            {
                animator.SetBool(frontParameter , false);
                animator.SetBool(backParameter , false);
                animator.SetBool (rightParameter , false);
                animator.SetBool(leftParemeter , true);
            } } ,
            { CharaterAniState.back , () =>
            {
                animator.SetBool(frontParameter , false);
                animator.SetBool(backParameter , true);
                animator.SetBool (rightParameter , false);
                animator.SetBool(leftParemeter , false);
            } } ,
        };
    
    }

       
    


}

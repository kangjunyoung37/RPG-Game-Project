using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateMachineBehavior : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<AttackStateController>()?.OnStartOfAttackState();
    }
    //public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
        
    //    animator.gameObject.GetComponent<AttackStateController>()?.OnEndOfAttackState();
    //}

}

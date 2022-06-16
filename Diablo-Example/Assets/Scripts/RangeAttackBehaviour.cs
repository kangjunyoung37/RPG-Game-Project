using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kang.Characters;
public class RangeAttackBehaviour : StateMachineBehaviour
{
  

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        
        animator.gameObject.GetComponent<AttackStateController>()?.OnEndOfAttackState();
        
    }
}

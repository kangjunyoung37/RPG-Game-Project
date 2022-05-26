using System.Collections;
using System.Collections.Generic;
using kang.AI;
using UnityEngine;


namespace kang.Characters { 
    public class AttackState : State<EnemyController>
    {
        private Animator animator;

        private int hasAttack = Animator.StringToHash("Attack");

        public override void OnInitialized()
        {
            animator = context.GetComponent<Animator>();

        }
        public override void OnEnter()
        {
            
            if (context.IsAvailableAttack)
            {
                animator?.SetTrigger(hasAttack);
            }
            else
            {
                stateMachine.ChageState<IdleState>();

            }

        }

        public override void Update(float deltaTime)
        {
            
        }
    }
}

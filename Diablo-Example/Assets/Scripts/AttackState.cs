using System.Collections;
using System.Collections.Generic;
using kang.Characters;
using UnityEngine;


namespace kang.AI { 
    public class AttackState : State<EnemyController>
    {
        private Animator animator;
        
        private AttackStateController attackStateController;
        private IAttackable attackable;

        private int hasAttack = Animator.StringToHash("Attack");
        private int attackIndexHash = Animator.StringToHash("AttackIndex");
        public override void OnInitialized()
        {
            animator = context.GetComponent<Animator>();
            attackStateController = context.GetComponent<AttackStateController>();
            attackable = context.GetComponent<IAttackable>();
        }
        public override void OnEnter()
        {
            if(attackable == null || attackable.CurrentAttackBehaviour == null)          
            {
                stateMachine.ChageState<IdleState>();
               
                return;
                
            }
            
            attackStateController.enterAttackStateHandler += OnEnterAttackState;
            attackStateController.exitAttackStateHandler += OnExitAttackState;

            animator?.SetInteger(attackIndexHash, attackable.CurrentAttackBehaviour.animationIndex);
            animator?.SetTrigger(hasAttack);


        }
        public void OnEnterAttackState()
        {
            
        }
        public void OnExitAttackState()
        {
           
            stateMachine.ChageState<IdleState>();
        }

        public override void OnExit()
        {
            attackStateController.enterAttackStateHandler -= OnEnterAttackState;
            attackStateController.exitAttackStateHandler -= OnExitAttackState;
        }
        public override void Update(float deltaTime)
        {
            
        }


    }
}

using System.Collections;
using System.Collections.Generic;
using kang.AI;
using UnityEngine;

namespace kang.Characters
{
    public class IdleState : State<EnemyController>
    {
        private Animator animator;
        private CharacterController controller;

        protected int hasMove = Animator.StringToHash("Move");
        protected int hasMoveSpeed = Animator.StringToHash("MoveSpeed");
        public override void OnInitialized()
        {
            animator = context.GetComponent<Animator>();
            controller = context.GetComponent<CharacterController>();
        }

        public override void OnEnter()
        {
            animator?.SetBool(hasMove, false);
            animator?.SetFloat(hasMoveSpeed, 0);
            controller?.Move(Vector3.zero);
        }
        public override void Update(float deltaTime)
        {
            Transform enemy = context.SearchEnemy();
            Debug.Log(enemy.name);
            if(enemy)
            {
                Debug.Log("dasdasd");
                if (context.IsAvailableAttack)
                {
                    stateMachine.ChageState<AttackState>();
                }
                else
                {
                    stateMachine.ChageState<MoveState>();
                }
            }

        }
        public override void OnExit()
        {

        }
    }
}
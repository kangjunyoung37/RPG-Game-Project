using System.Collections;
using System.Collections.Generic;
using kang.Characters;
using UnityEngine;

namespace kang.AI
{
    public class IdleState : State<EnemyController>
    {
        public bool isPatrol = false;
        private float minIdleTime = 0.0f;
        private float maxIdleTime = 3.0f;
        private float idleTime = 0.0f;
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

            if (isPatrol)
            {
                idleTime = Random.Range(minIdleTime, maxIdleTime);
            }
        }
        public override void Update(float deltaTime)
        {


            if (context.Target)
            {
                
                if (context.IsAvailableAttack)
                {
                    Debug.Log("°¡´ÉÇÔ");
                    stateMachine.ChageState<AttackState>();
                }
                else
                {
                    stateMachine.ChageState<MoveState>();
                }
            }
            else if( isPatrol && stateMachine.ElapsedTimeInState > idleTime)
            {
               
                stateMachine.ChageState<MoveToWayPoint>();
            }

        }
        public override void OnExit()
        {

        }
    }
}
using System.Collections;
using kang.AI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace kang.Characters
{
    public class MoveState : State<EnemyController>
    {
        private Animator animator;
        private CharacterController controller;
        private NavMeshAgent agent;

        private int hasMove = Animator.StringToHash("Move");
        private int hasMoveSpeed = Animator.StringToHash("MoveSpeed");
        public override void OnInitialized()
        {
            animator = context.GetComponent<Animator>();
            controller = context.GetComponent<CharacterController>();
            agent = context.GetComponent<NavMeshAgent>();
        }
        public override void OnEnter()
        {
            agent?.SetDestination(context.Target.position);
            animator?.SetBool(hasMove, true);
        }

        public override void Update(float deltaTime)
        {
            Transform enemy = context.SearchEnemy();
           
            if (enemy)
            {
                agent.SetDestination(context.Target.position);
                if(agent.remainingDistance > agent.stoppingDistance)
                {
                    controller.Move(agent.velocity * deltaTime);
                    animator.SetFloat(hasMoveSpeed, agent.velocity.magnitude / agent.speed, 1f, deltaTime);
                    return;
                }
            }
            
            if (!enemy || agent.remainingDistance <= agent.stoppingDistance)
            {
                stateMachine.ChageState<IdleState>();
            }
        }

        public override void OnExit()
        {
            animator?.SetBool(hasMove, false);
            animator?.SetFloat(hasMoveSpeed, 0f);
            agent.ResetPath();
        }

    }
}


using System.Collections;
using kang.Characters;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace kang.AI
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


            if (context.Target)
            {
                agent.SetDestination(context.Target.position);
            }
            controller.Move(agent.velocity * Time.deltaTime);
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                    
                    animator.SetFloat(hasMoveSpeed, agent.velocity.magnitude / agent.speed, 0.1f, Time.deltaTime);
                    
             
            }
            else
            {
                if (!agent.pathPending)
                {
                    animator.SetFloat(hasMoveSpeed, 0);
                    animator.SetBool(hasMove, false);
                    agent.ResetPath();

                    stateMachine.ChageState<IdleState>();
                }
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


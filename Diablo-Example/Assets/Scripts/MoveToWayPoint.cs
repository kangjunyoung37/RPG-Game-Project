using System.Collections;
using System.Collections.Generic;
using kang.AI;
using UnityEngine;
using UnityEngine.AI;

namespace kang.Characters
{
    public class MoveToWayPoint : State<EnemyController>
    {
        private Animator animator;
        private CharacterController controller;
        private NavMeshAgent agent;

        protected int hasMove = Animator.StringToHash("Move");
        protected int hasMoveSpeed = Animator.StringToHash("MoveSpeed");
        public override void OnInitialized()
        {
            animator = context.GetComponent<Animator>();
            controller = context.GetComponent<CharacterController>();
            agent = context.GetComponent<NavMeshAgent>();   

        }

        public override void OnEnter()
        {
            
            if(context.targetWaypoint == null)
            {
                context.FindNextWayPoint();
                
            }
           

            if (context.targetWaypoint)
            {
                agent?.SetDestination(context.targetWaypoint.position);
                animator?.SetBool(hasMove, true);
            }
        }
        public override void Update(float deltaTime)
        {
            Transform enemy = context.SearchEnemy();
            
            if (enemy)
            {
                
                if (context.IsAvailableAttack)
                {
                    stateMachine.ChageState<AttackState>();
                }
                else
                {
                    stateMachine.ChageState<MoveState>();
                }
            }
            else
            {
                
                if(!agent.pathPending && (agent.remainingDistance <= agent.stoppingDistance))//pathPending 이동해야할 경로가 존재하는지 안하는지 확인
                {

                    
                    Transform nextDest = context.FindNextWayPoint();
                    Debug.Log(nextDest);
                    
                    if (nextDest)
                    {
                        agent.SetDestination(nextDest.position);
                        
                    }
                    stateMachine.ChageState<IdleState>();
                }
                else
                {
                    
                    controller.Move(agent.velocity * deltaTime);
                    animator.SetFloat(hasMoveSpeed, agent.velocity.magnitude / agent.speed, .1f, deltaTime);
                }
            }

        }
        public override void OnExit()
        {
            animator?.SetBool(hasMove, false);
            agent.ResetPath();
        }
    }
}
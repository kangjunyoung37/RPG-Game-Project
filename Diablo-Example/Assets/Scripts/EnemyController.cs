using kang.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kang.Characters
{


    public class EnemyController : MonoBehaviour
    {
        #region Variables
        protected StateMachine<EnemyController> stateMachine;
        public StateMachine<EnemyController> StateMachine
        {
            get { return stateMachine; }
        }
        private FiledOfView fov;
        public Transform Target => fov?.NearestTarget;

        public Transform[] waypoints;
        [HideInInspector]
        public Transform targetWaypoint = null;
        private int waypoinIndex = 0;

        public float attackRange;
        
        #endregion Variables
        #region Unity Methods
        private void Start()
        {
            stateMachine = new StateMachine<EnemyController>(this, new MoveToWayPoint());
            IdleState idleState = new IdleState();
            idleState.isPatrol = true;
            stateMachine.AddState(idleState);
            stateMachine.AddState(new MoveState());
            stateMachine.AddState(new AttackState());
            fov = GetComponent<FiledOfView>();
        }
        private void Update()
        {
            
            stateMachine.Update(Time.deltaTime);
        }
        #endregion Unity Methods

        #region Other Methods

        
        public bool IsAvailableAttack
        {
            get
            {
                if (!Target)
                {
                    return false;
                }
                float distance = Vector3.Distance(transform.position, Target.position);
                
                return (distance <= attackRange);
            }
        }
        public Transform SearchEnemy()
        {
            return Target;
            /*Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
            if(targetInViewRadius.Length > 0)
            {
                target = targetInViewRadius[0].transform;
            }
            return target;*/
        }
        #endregion Other Methods
       /* private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, viewRadius);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }*/
       public Transform FindNextWayPoint()
        {
            targetWaypoint = null;
            if(waypoints.Length > 0)
            {
                targetWaypoint = waypoints[waypoinIndex];
            }
            
            waypoinIndex = (waypoinIndex+1) % waypoints.Length;
            Debug.Log(waypoinIndex);
            return targetWaypoint;
        }
    }

}
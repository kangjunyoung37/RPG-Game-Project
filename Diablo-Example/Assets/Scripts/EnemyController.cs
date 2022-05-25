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

        public LayerMask targetMask;
        public Transform target;
        public float viewRadius;
        public float attackRange;
        
        #endregion Variables
        #region Unity Methods
        private void Start()
        {
            stateMachine = new StateMachine<EnemyController>(this, new IdleState());
            stateMachine.AddState(new MoveState());
            stateMachine.AddState(new AttackState());

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
                if (!target)
                {
                    return false;
                }
                float distance = Vector3.Distance(transform.position, target.position);
                Debug.Log(distance );
                return (distance <= attackRange);
            }
        }
        public Transform SearchEnemy()
        {
            target = null;
            Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
            if(targetInViewRadius.Length > 0)
            {
                target = targetInViewRadius[0].transform;
            }
            return target;
        }
        #endregion Other Methods
    }

}
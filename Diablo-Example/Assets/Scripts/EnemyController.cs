using kang.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace kang.Characters
{


    public class EnemyController : MonoBehaviour, IAttackable, IDamageable
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

        
        public virtual float attackRange => CurrentAttackBehaviour?.range ?? 6.0f;

        public Transform projectileTransform;
        public Transform hitTransform;
        [SerializeField]
        private List<AttackBehaviour> attackBehaviours = new List<AttackBehaviour>();

        public LayerMask TargetMask;
        public int maxHealth = 100;
        public int health;

        NavMeshAgent agent;
        Animator animator;
        #endregion Variables

        #region Unity Methods
        private void Start()
        {


            agent = GetComponent<NavMeshAgent>();
            stateMachine = new StateMachine<EnemyController>(this, new IdleState());
            stateMachine.AddState(new MoveState());
            stateMachine.AddState(new AttackState());
            stateMachine.AddState(new DeadState());
            InitAttackBehaviour();
            fov = GetComponent<FiledOfView>();
            animator = GetComponent<Animator>();
            health = maxHealth;
        }
        private void Update()
        {
            CheckAttackBehaviour();
            if (!(stateMachine.CurrentState is MoveState) && !(stateMachine.CurrentState is DeadState))
            {
                FaceTarget();
            }
            stateMachine.Update(Time.deltaTime);

            
        }

        #endregion Unity Methods
        #region Helper Methods
        private void InitAttackBehaviour()
        {
            foreach(AttackBehaviour behaviour in attackBehaviours)
            {
                if(CurrentAttackBehaviour == null)
                {
                    CurrentAttackBehaviour = behaviour;
                }
                behaviour.targetMask = TargetMask;
            }
        }
        private void CheckAttackBehaviour()
        {
            if(CurrentAttackBehaviour == null || !CurrentAttackBehaviour.IsAvailable)
            {
                CurrentAttackBehaviour = null;
                foreach(AttackBehaviour behaviour in attackBehaviours)
                {
                    if (behaviour.IsAvailable)
                    {
                        if((CurrentAttackBehaviour == null || (CurrentAttackBehaviour.priority < behaviour.priority)))
                        {
                            CurrentAttackBehaviour = behaviour;
                        }
                    }
                }
            }
        }

        #endregion Helper Methods
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
                bool result = distance <= attackRange;
               
                return (distance <= attackRange);
            }
        }

        

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

        public void FaceTarget()
        {
            if(Target)
            {
                Vector3 direction = (Target.position - transform.position).normalized;
                Quaternion quaternion = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, quaternion, Time.deltaTime * 5f);
            }
        }
        #endregion Other Methods

        #region IAttackable interfaces

        public AttackBehaviour CurrentAttackBehaviour
        {
            get;
            private set;
        }
        public void OnExecuteAttack(int attackIndex)
        {
 
            if (CurrentAttackBehaviour != null ) //&& Target != null)
            {
               
                CurrentAttackBehaviour.ExecuteAttack(Target.gameObject, projectileTransform);
            }
        }
        #endregion IAttackable interfaces
        #region IDamamgeable interfaces
        public bool IsAlive => health > 0;

        public void TakveDamage(int damage, GameObject hitEffectPrefabs)
        {
            if(!IsAlive)
            {
                return;
            }
            health -= damage;

            if (hitEffectPrefabs)
            {
                Instantiate(hitEffectPrefabs, hitTransform);
            }
            if(IsAlive)
            {
                //animator?.SetTrigger(hitTriggerHash);
            }
            else
            {
                stateMachine.ChageState<DeadState>();
            }
        }


        #endregion IDamamgeable interfaces
    }



}
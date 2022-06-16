using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using kang.Characters;
using UnityEngine.EventSystems;
using kang.InventorySystem.Inventory;
using kang.InventorySystem.Items;

namespace kang.Characters
{


    public class ControllerCharacter : MonoBehaviour , IAttackable, IDamageable
    {

        #region Variables

        [SerializeField]
        private InventoryObject equipment;

        [SerializeField]
        private InventoryObject inventory;

        private CharacterController characterController;
        private NavMeshAgent agent;
        private Camera camera;

        public Transform Target;
        public Transform projectileTransform;
        public Transform hitTransform;

        public LayerMask groundLayerMask;
        public float groundCheckDistance = 0.3f;

        [SerializeField]
        private List<AttackBehaviour> attackBehaviours = new List<AttackBehaviour>();

        public LayerMask TargetMask;

        [SerializeField]
        private Animator animator;

        readonly int moveHash = Animator.StringToHash("Move");
        readonly int moveSpeed = Animator.StringToHash("moveSpeed");
        readonly int Attack = Animator.StringToHash("isAttack");
        readonly int attackIndexHash = Animator.StringToHash("AttackIndex");
        public TargetPicker picker;


        [SerializeField]
        public StatsObject playerStats;
        #endregion Variables

        #region Unity Methods
        void Start()
        {
            InitAttackBehaviour();
            inventory.OnUseItem += OnUseItem;
            characterController = GetComponent<CharacterController>();
            agent = GetComponent<NavMeshAgent>();
            camera = Camera.main;
            agent.updatePosition = false;//NavMeshAgent를 가지고 움직이지 않음
            agent.updateRotation = true;
           
        }
        void Update()
        {
            if(!IsAlive)
            {
                return;

            }

            bool isOnUI = EventSystem.current.IsPointerOverGameObject();

            if (!isOnUI && Input.GetMouseButtonDown(0))
            {
                
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100, groundLayerMask))
                {
                    RemoveTarget();
                    agent.SetDestination(hit.point);
                    if(picker)
                    {
                        picker.SetPosition(hit);
                    }

                }
            }
            else if ((!isOnUI && Input.GetMouseButtonDown(1)))
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100))
                {
                    
                    IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                    IDamageable damageable = hit.collider.GetComponent<IDamageable>();
                    
                    if(hit.collider.GetComponent<ControllerCharacter>()!=null)
                    {
                        return;
                    }
                    if(damageable != null && damageable.IsAlive )
                    {
                        SetTarget(hit.collider.transform,CurrentAttackBehaviour?.range?? 1.5f);
                       
                    }
                    if (interactable != null)
                    {

                        SetTarget(hit.collider.transform, interactable.Distance);
                    }


                }
            }
            if(Target != null)
            {
                if (Target.GetComponent<IInteractable>() != null)
                {
                    float calcDistance = Vector3.Distance(Target.position, transform.position);
                    float range = Target.GetComponent<IInteractable>().Distance;
                    if (calcDistance > range)
                    {
                        SetTarget(Target, range);
                    }

                    FaceToTarget();
                }
                else if (!(Target.GetComponent<IDamageable>()?.IsAlive ?? false))
                {
                    RemoveTarget();
                }
                else
                {
                    float calcDistance = Vector3.Distance(Target.position, transform.position);
                    float range = CurrentAttackBehaviour?.range ?? 1.5f;
                    if (calcDistance > range)
                    {
                        SetTarget(Target, range);
                    }

                    FaceToTarget();
                }
            }


            if (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
            {
                
                characterController.Move(agent.velocity * Time.deltaTime);

                animator.SetBool(Attack, false);
                animator.SetFloat(moveSpeed, agent.velocity.magnitude / agent.speed, 0.1f, Time.deltaTime);
                animator.SetBool(moveHash , true);
                
            }
            else
            {
                characterController.Move(agent.velocity * Time.deltaTime);
                if(!agent.pathPending)
                {
                        animator.SetFloat(moveSpeed, 0);
                        animator.SetBool(moveHash, false);
                        agent.ResetPath();
                }
                if(Target!= null)
                {
                    if(Target.GetComponent<IInteractable>() != null)
                    {
                        IInteractable interactable = Target.GetComponent<IInteractable>();
                        interactable.Interact(this.gameObject);
                        Target = null;
                        
                          
                    }
                    if (Target?.GetComponent<IDamageable>() != null )
                    {
                        if(IsAvailableAttack)
                        {
                            animator.SetInteger(attackIndexHash, CurrentAttackBehaviour.animationIndex);
                            animator.SetBool(Attack, true);
                        }
                        
                      
                            
                            

                    }

                }

                
            }
        
        
        }
        private void LateUpdate()
        {
            transform.position = agent.nextPosition;
        }
        #endregion Unity Methods

        #region IAttackable interfaces

        public AttackBehaviour CurrentAttackBehaviour
        {
            get;
            private set;
        }
        public void OnExecuteAttack(int attackIndex)
        {

            if (CurrentAttackBehaviour != null && Target != null)
            {

                CurrentAttackBehaviour.ExecuteAttack(Target.gameObject, projectileTransform);
            }
        }
        #endregion IAttackable interfaces

        #region IDamamgeable interfaces
        public bool IsAlive => playerStats.Health > 0;

        public void TakeDamage(int damage, GameObject hitEffectPrefabs)
        {
            if (!IsAlive)
            {
                return;
            }
            playerStats.AddHealth(-damage);

            //if (battleUI)
            //{

            //    battleUI.Value = health;
            //    battleUI.CreateDamageText(damage);
            //}
            if (hitEffectPrefabs)
            {
                Instantiate(hitEffectPrefabs, hitTransform);

                hitEffectPrefabs.transform.LookAt(Camera.main.transform);

                
            }
            if (IsAlive)
            {
                //animator?.SetTrigger(hitTriggerHash);
            }
            //else
            //{
            //    if (battleUI != null)
            //    {
            //        battleUI.enabled = false;
            //    }
            //    stateMachine.ChageState<DeadState>();
            //}
        }


        #endregion IDamamgeable interfaces

        #region Inventory
        private void OnUseItem(ItemObject itemObject)
        {
            foreach(ItemBuff buff in itemObject.data.buffs)
            {
                if(buff.state == AttributeType.Health)
                {
                    playerStats.AddHealth(buff.value);
                   
                }
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            var item = other.GetComponent<GroundItem>();
            if(item)
            {
                if(inventory.AddItem(new Item(item.itemObject),1))

                {
                    Destroy(other.gameObject);
                }
            }
        }
        public bool PickupItem(PickupItem pickupItem, int amount = 1)
        {
            if(pickupItem.itemObject != null && inventory.AddItem(new Item(pickupItem.itemObject), amount))
            {
                Destroy(pickupItem.gameObject);
                return true;
            }
            return false;
        }
        #endregion Inventory

        #region Help Method
        void SetTarget(Transform newTarget, float stoppingDistance )
        {
           Target = newTarget;
            agent.stoppingDistance = stoppingDistance;
            agent.SetDestination(newTarget.transform.position);
            agent.updateRotation = false;
            if(picker)
            {
                picker.target = newTarget.transform;
            }

        }

        void RemoveTarget()
        {
            Target = null;
            agent.stoppingDistance = 0f;
            agent.updateRotation = true;

            agent.ResetPath();
        }
        void FaceToTarget()
        {
            if(Target)
            {
                Vector3 Direction = (Target.transform.position - transform.position).normalized;
                Quaternion LookQuaternion = Quaternion.LookRotation(new Vector3(Direction.x, 0, Direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, LookQuaternion, Time.deltaTime*10.0f);

            }
        }

        private void InitAttackBehaviour()
        {
            foreach (AttackBehaviour behaviour in attackBehaviours)
            {

           
                if (CurrentAttackBehaviour == null)
                {
                    CurrentAttackBehaviour = behaviour;
                }
                behaviour.targetMask = TargetMask;
            }
        }
        public bool IsAvailableAttack
        {
            get
            {
                if (!Target)
                {
                    return false;
                }
                float distance = Vector3.Distance(transform.position, Target.position);
                return distance <= CurrentAttackBehaviour.range;
            }
        }
        #endregion Help Method
    }

}
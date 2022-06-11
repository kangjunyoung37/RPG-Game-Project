using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using kang.Characters;
using UnityEngine.EventSystems;
using kang.InventorySystem.Inventory;
using kang.InventorySystem.Items;
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
    
    public int maxHealth = 100;
    public int health;

    [SerializeField]
    private Animator animator;

    readonly int moveHash = Animator.StringToHash("Move");
    readonly int moveSpeed = Animator.StringToHash("moveSpeed");
    public TargetPicker picker;


    #endregion Variables

    #region Unity Methods
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        camera = Camera.main;
        agent.updatePosition = false;//NavMeshAgent를 가지고 움직이지 않음
        agent.updateRotation = true;
        health = maxHealth;
    }
    void Update()
    {
        if(!IsAlive)
        {
            return;

        }

        bool isOnUI = EventSystem.current.IsPointerOverGameObject();
        if (!isOnUI && Input.GetMouseButtonDown(1))
        {

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, groundLayerMask))
            {
               
                agent.SetDestination(hit.point);
                if(picker)
                {
                    picker.SetPosition(hit);
                }

            }
        }
        //else if((!isOnUI && Input.GetMouseButtonDown(0)))
        //{
        //    Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        //    RaycastHit hit;
        //    if (Physics.Raycast(ray, out hit, 100, groundLayerMask))
        //    {
        //        IInteractable interactable = hit.collider.GetComponent<IInteractable>();
        //        if(interactable != null)
        //        {
                    
        //        }


        //    }
        //}


        if(agent.remainingDistance > agent.stoppingDistance)
        {
                
            characterController.Move(agent.velocity * Time.deltaTime);
                
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
    public bool IsAlive => health > 0;

    public void TakveDamage(int damage, GameObject hitEffectPrefabs)
    {
        if (!IsAlive)
        {
            return;
        }
        health -= damage;

        //if (battleUI)
        //{

        //    battleUI.Value = health;
        //    battleUI.CreateDamageText(damage);
        //}
        if (hitEffectPrefabs)
        {
            Instantiate(hitEffectPrefabs, hitTransform);
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
}

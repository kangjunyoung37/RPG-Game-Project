using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using kang.Characters;
public class ControllerCharacter : MonoBehaviour , IAttackable, IDamageable
{

    #region Variables

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
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        camera = Camera.main; 
        agent.updatePosition = false;//NavMeshAgent를 가지고 움직이지 않음
        agent.updateRotation = true;
        health = maxHealth; 
    }
    #region Unity Methods
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
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
        else
        {
            //if (battleUI != null)
            //{
            //    battleUI.enabled = false;
            //}
            //stateMachine.ChageState<DeadState>();
        }
    }


    #endregion IDamamgeable interfaces
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ControllerCharacter : MonoBehaviour
{

    #region Variables

    private CharacterController characterController;
    private NavMeshAgent agent;
    private Camera camera;
    private bool isGrounded = false;

    public LayerMask groundLayerMask;
    public float groundCheckDistance = 0.3f;

    [SerializeField]
    private Animator animator;

    readonly int moveHash = Animator.StringToHash("Move");
    readonly int fallingHash = Animator.StringToHash("Falling");


    #endregion Variables
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        camera = Camera.main; 
        agent.updatePosition = false;//NavMeshAgent를 가지고 움직이지 않음
        agent.updateRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {


            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, groundLayerMask))
            {
                Debug.Log("we hit" + hit.collider.name + " " + hit.point);

                agent.SetDestination(hit.point);

            }
        }
        if(agent.remainingDistance > agent.stoppingDistance)
            {
                characterController.Move(agent.velocity * Time.deltaTime);
                animator.SetBool(moveHash , true);
            }
        else
            {
                characterController.Move(Vector3.zero);
                animator.SetBool(moveHash, false);
            }
        
        
    }
    private void LateUpdate()
    {
        transform.position = agent.nextPosition;
    }
    
    
}

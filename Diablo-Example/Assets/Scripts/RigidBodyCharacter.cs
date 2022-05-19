using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyCharacter : MonoBehaviour
{

    #region Variables

    public float speed = 5f;
    public float jumpHeight = 2f;
    public float dashDIstance = 5f;

    private Rigidbody rigidbody;


    private Vector3 inputDirection;

    private bool isGrounded = false;
    public LayerMask groundLayerMask;
    public float groundCheckDistance = 0.3f;

   
    #endregion Variables
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckGroundStatus();
        // 사용자의 입력값 받아오기
        inputDirection = Vector3.zero;
        inputDirection.x = Input.GetAxis("Horizontal");
        inputDirection.z = Input.GetAxis("Vertical");
        if(inputDirection != Vector3.zero)
        {
            transform.forward = inputDirection;
        }
        // 점프
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Vector3 jumpVelocity = Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
            rigidbody.AddForce(jumpVelocity,ForceMode.VelocityChange);

        }
        //대쉬
        if (Input.GetButtonDown("Dash"))
        {
            Vector3 dashVelocity = Vector3.Scale(transform.forward,
                dashDIstance * new Vector3((Mathf.Log(1f / (Time.deltaTime * rigidbody.drag + 1)) / -Time.deltaTime), 
                0, (Mathf.Log(1f / (Time.deltaTime * rigidbody.drag + 1)) / -Time.deltaTime)));

            rigidbody.AddForce(dashVelocity,ForceMode.VelocityChange);
        }
    }
    private void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + inputDirection * speed * Time.fixedDeltaTime);

    }

    #region Helper Methods
    void CheckGroundStatus()
    {
#if UNITY_EDITOR
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * groundCheckDistance));

#endif
        RaycastHit hitInfo;

        if(Physics.Raycast(transform.position + (Vector3.up * 0.1f),Vector3.down,out hitInfo, groundCheckDistance,groundLayerMask))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
#endregion
}

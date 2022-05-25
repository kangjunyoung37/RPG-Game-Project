using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kang.Cameras
{
public class TopDownCamera : MonoBehaviour
{

    #region Varaibles
    public float height = 5f;
    public float distanse = 10;
    public float angle = 45;
    public float lookAtHeight = 2f;
    public float smoothSpeed = 0.5f;

    private Vector3 refVelocity;

    public Transform target;
    #endregion Varaibles
    // Start is called before the first frame update
    private void LateUpdate()
    {
        HandleCamera();
    }

    public void HandleCamera()
    {
        if (!target)
        {
            return;
        }
        Vector3 worldPosition = (Vector3.forward * -distanse) + (Vector3.up * height);
        //Debug.DrawLine(target.position, worldPosition, Color.red);

        Vector3 rotateVector = Quaternion.AngleAxis(angle, Vector3.up) * worldPosition;
       // Debug.DrawLine(target.position, rotateVector, Color.green);

        Vector3 finalTargetPosition = target.position;
        finalTargetPosition.y += lookAtHeight;

        Vector3 finalPosition = finalTargetPosition + rotateVector;
       // Debug.DrawLine(target.position, finalPosition, Color.white);

        transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, smoothSpeed);

        transform.LookAt(target.position);

        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        if(target)
        {
            Vector3 lookAtPosition = target.position;
            lookAtPosition.y += lookAtHeight;
            Gizmos.DrawLine(transform.position, lookAtPosition);
            Gizmos.DrawSphere(lookAtPosition, 0.25f);


        }

        Gizmos.DrawSphere(transform.position, 0.25f);
    }
}
}

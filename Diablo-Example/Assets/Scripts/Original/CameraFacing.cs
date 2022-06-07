using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFacing : MonoBehaviour
{
    public bool reverseFace = false;
    Camera referenceCamera;

    public enum Axis
    {
        up,
        down,
        left,
        right,
        foward,
        back
    };
    public Axis axis = Axis.up;
    public Vector3 GetAxis(Axis refAxix)
    {
        switch (refAxix)
        {
            case Axis.up:
                return Vector3.up;
            case Axis.down:
                return Vector3.down;
            case Axis.left:
                return Vector3.left;
            case Axis.right:
                return Vector3.right;
            case Axis.foward:
                return Vector3.forward;
            case Axis.back:
                return Vector3.back;
        }
        return Vector3.up;
    }

    private void Awake()
    {
        if(referenceCamera == null)
        {
            referenceCamera = Camera.main;
        }
    }
    private void LateUpdate()
    {
        Vector3 targetPos = transform.position + referenceCamera.transform.rotation * (reverseFace ? Vector3.forward : Vector3.back);
        Vector3 targetOrientation = referenceCamera.transform.rotation * GetAxis(axis);
        transform.LookAt(targetPos, targetOrientation);
    }
}

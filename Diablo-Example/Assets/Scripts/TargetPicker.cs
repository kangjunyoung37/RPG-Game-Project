using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPicker : MonoBehaviour
{
    public float surfaceOffset = 1.5f;
    public Transform target = null;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            transform.position = target.position + Vector3.up * surfaceOffset;
        }
    }
    public void SetPosition(RaycastHit hit)
    {
        target = null;
        transform.position = hit.point + hit.normal * surfaceOffset;
    }
}

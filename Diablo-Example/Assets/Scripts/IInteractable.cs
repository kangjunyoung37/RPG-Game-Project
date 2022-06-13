using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    float Distance
    {
        get;
    }
    void Interact(GameObject other);
    void StopInteract(GameObject other);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    float Distance
    {
        get;
    }
    bool Interact(GameObject other);
    void StopInteract(GameObject other);
}

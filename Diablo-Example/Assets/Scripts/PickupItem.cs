using kang.InventorySystem.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour , IInteractable
{
    public float distance = 3.0f;
    public float Distance => distance;

    public ItemObject itemObject;

    public bool Interact(GameObject other)
    {
        float calcDistance = Vector3.Distance(transform.position, other.transform.position);
        if(calcDistance > distance)
        {
            return false ;
        }
        return other.GetComponent<CharacterController>()?.PickupItem(this) ?? false;

    }

    public void StopInteract(GameObject other)
    {

    }
    private void OnValidate()
    {
#if UNITY_EDITOR
        GetComponent<SpriteRenderer>().sprite = itemObject?.icon;

#endif
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distance);
    }
}

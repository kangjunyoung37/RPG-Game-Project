using kang.InventorySystem.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kang.Characters;
public class PickupItem : MonoBehaviour , IInteractable
{
    public float distance = 3.0f;
    public float Distance => distance;

    public ItemObject itemObject;

    public void Interact(GameObject other)
    {
        float calcDistance = Vector3.Distance(transform.position, other.transform.position);
        if(calcDistance > distance)
        {
            return;
        }
        ControllerCharacter controllerCharacter = other.GetComponent<ControllerCharacter>();
        if(controllerCharacter?.PickupItem(this) ?? false)
        {
            Destroy(gameObject);
        }
        

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

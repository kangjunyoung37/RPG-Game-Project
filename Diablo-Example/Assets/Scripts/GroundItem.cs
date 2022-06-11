using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kang.InventorySystem.Items;
public class GroundItem : MonoBehaviour
{

    public ItemObject itemObject;

    private void OnValidate()
    {

        GetComponent<SpriteRenderer>().sprite = itemObject?.icon;

    }
}


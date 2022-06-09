using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kang.InventorySystem.Items
{

    [CreateAssetMenu(fileName ="New Item Database ", menuName ="Inventory System/Items/Databse")]
    public class ItemObjectDatabase : ScriptableObject
    {
        public ItemObject[] itemObjects;
        public void OnValidate()
        {
            for(int i = 0; i< itemObjects.Length; i++)
            {
                itemObjects[i].data.id = i;
            }
        }
    }

}

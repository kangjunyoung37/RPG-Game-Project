using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kang.InventorySystem.Items;
using System;
using System.Linq;

namespace kang.InventorySystem.Inventory
{

    [Serializable]
    public class Inventory
    {
        public InventorySlot[] slots = new InventorySlot[24];

        public void Clear()
        {
            foreach (InventorySlot slot in slots)
            {
                slot.RemoveItem();
            }
        }
        public bool IsContain(ItemObject itemObject)
        {
            //return Array.Find(slots, i => i.item.id == itemObject.data.id) != null;
            return IsContain(itemObject.data.id);
        }
        public bool IsContain(int id)
        {
            return slots.FirstOrDefault(i => i.item.id == id) != null;
        }
    }

}
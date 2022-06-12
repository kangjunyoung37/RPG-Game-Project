using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using kang.InventorySystem.Items;
namespace kang.InventorySystem.Inventory {

[Serializable]
public class InventorySlot 
{
    public ItemType[] allowedItems = new ItemType[0];

    [NonSerialized]
    public InventoryObject parent;
    [NonSerialized]
    public GameObject slotUI;

    [NonSerialized]
    public Action<InventorySlot> OnPreUpdate;
    [NonSerialized]
    public Action<InventorySlot> OnPostUpdat;
    public Item item;
    public int amount;
    public ItemObject ItemObject
    {
        get
        {
            return item.id >= 0 ? parent.database.itemObjects[item.id] : null;
        }
    }
    public InventorySlot() => UpdateSlot(new Item(), 0);
    public InventorySlot(Item item, int amount) => UpdateSlot(item, amount);

    public void AddItem(Item itme, int amount) => UpdateSlot(item, amount);
    public void RemoveItem() => UpdateSlot(new Item(), 0);
    public void AddAmount(int value) => UpdateSlot(item, amount += value);

    public void UpdateSlot(Item item , int amount)
    {
        if(amount <= 0)
        {
            item = new Item();
        }
        OnPreUpdate?.Invoke(this);

        this.item = item;
        this.amount = amount;

        OnPostUpdat?.Invoke(this);
    }
    public bool CanPlaceInSlot(ItemObject itemObject)
    {
        if(allowedItems.Length <= 0 || itemObject ==null || itemObject.data.id <0)
        {
            return true;
        }
        foreach(ItemType type in allowedItems)
        {
            if(itemObject.type == type)
            {
                return true;
            }

        }
        return false;
    }
        
}

}
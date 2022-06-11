using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kang.InventorySystem.Inventory;
using kang.InventorySystem.Items;
using System.Linq;
public class PlayerEquipment : MonoBehaviour
{
    public InventoryObject equipment;

    private EquipMentCombiner combiner;

    private ItemInstances[] itemInstances = new ItemInstances[8];

    public ItemObject[] defaultItemObjects = new ItemObject[8];

    private void Awake()
    {
        combiner = new EquipMentCombiner(gameObject);
        for (int i = 0; i<equipment.Slots.Length; i++)
        {
            equipment.Slots[i].OnPreUpdate += OnRemoveItem;
            equipment.Slots[i].OnPostUpdat += OnEquipItem;
        }
    }
    void Start()
    {
        foreach (InventorySlot slot in equipment.Slots)
        {
            OnEquipItem(slot);
        }
    }

    private void OnEquipItem(InventorySlot slot)
    {
        ItemObject itemObject = slot.ItemObject;
        if(itemObject == null)
        {
            EquipDefaultItemBy(slot.allowedItems[0]);//장비하고 있는 아이템이 없을 때 기본 아이템으로 설정
            return;
        }
        int index = (int)slot.allowedItems[0];
        switch(slot.allowedItems[0])
        {
            case ItemType.Helmet:
            case ItemType.Chest:
            case ItemType.Pants:
            case ItemType.Boots:
            case ItemType.Gloves:
                itemInstances[index] = EquipSkinnedItem(itemObject);
                break;
            case ItemType.Pauldrons:
            case ItemType.LeftWeapon:
            case ItemType.RightWeapon:
                itemInstances[index] = EquipMeshItem(itemObject);
                break;
        }

    }
    private void EquipDefaultItemBy(ItemType type)
    {
        int index = (int)type;
        ItemObject itemObject = defaultItemObjects[index];

        switch (type)
        {
            case ItemType.Helmet:
            case ItemType.Chest:
            case ItemType.Pants:
            case ItemType.Boots:
            case ItemType.Gloves:
                itemInstances[index] = EquipSkinnedItem(itemObject);
                break;
            case ItemType.Pauldrons:
            case ItemType.LeftWeapon:
            case ItemType.RightWeapon:
                itemInstances[index] = EquipMeshItem(itemObject);
                break;
        }

    }
    private ItemInstances EquipSkinnedItem(ItemObject itemObject)
    {
        if(itemObject == null)
        {
            return null;

        }

        Transform itemTransfrom = combiner.AddLimb(itemObject.modelPrefab, itemObject.boneNames);
        
        if(itemTransfrom != null)
        {
            ItemInstances instances = new ItemInstances();
            instances.itemTransfroms.Add(itemTransfrom);
            return instances;
        }
        return null;
        
    }
    private ItemInstances EquipMeshItem(ItemObject itemObject)
    {
        if (itemObject == null)
        {
            return null;

        }
        Transform[] itemTransforms = combiner.AddMesh(itemObject.modelPrefab);
        if(itemTransforms.Length >0)
        {
            ItemInstances instances = new ItemInstances();
            instances.itemTransfroms.AddRange(itemTransforms.ToList<Transform>());

            return instances;
        }
        return null;

    }
    private void OnDestroy()
    {
        foreach (ItemInstances item in itemInstances)
        {
            item.Destroy();
        }
    }
    private void OnRemoveItem(InventorySlot slot)
    {
        ItemObject itemObject = slot.ItemObject;
        if(itemObject == null)
        {
            RemoveItemBy(slot.allowedItems[0]);
            return;
        }
        if(slot.ItemObject.modelPrefab != null)
        {
            RemoveItemBy(slot.allowedItems[0]);
        }
    }

    private void RemoveItemBy(ItemType type)
    {
        int index = (int)type;
        if(itemInstances[index] != null)
        {
            itemInstances[index].Destroy();
            itemInstances[index] = null;
        }
    }
}

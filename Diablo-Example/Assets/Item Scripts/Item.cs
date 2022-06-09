using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace kang.InventorySystem.Items { 
[Serializable]
public class Item
{
    public int id = -1;//비어 있는 상태
    public string name;
    public ItemBuff[] buffs;

    public Item()
    {
        id = -1;
        name = "";
    }
    public Item(ItemObject itemObject)
    {
        name = itemObject.name;
        id = itemObject.data.id;

            buffs = new ItemBuff[itemObject.data.buffs.Length];
            for(int i = 0; i < buffs.Length; i ++ )
            {
                buffs[i] = new ItemBuff(itemObject.data.buffs[i].Min, itemObject.data.buffs[i].Max)
                {
                    state = itemObject.data.buffs[i].state
                };


            }
    }

    
}
}

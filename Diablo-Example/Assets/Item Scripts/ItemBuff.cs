using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace kang.InventorySystem.Items
{


public enum CharacterAttribute
{
    Agility,
    Intellect,
    Stamina,
    Strength
}
[Serializable]
public class ItemBuff
{
    public CharacterAttribute state;
    public int value;
    [SerializeField]
    private int min;
    [SerializeField]
    private int max;

    public int Min => min;
    public int Max => max;

    public ItemBuff(int min , int max)
    {
        this.min = min;
        this.max = max;

        GenerateValue();
    }
    public void GenerateValue()
    {
        value = UnityEngine.Random.Range(min, max);
    }
     public void AddValue(ref int v)
    {
        v += value;
    }

}
}

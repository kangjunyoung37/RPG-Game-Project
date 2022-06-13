using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stats", menuName = "Stats System/New Character Stats New")]
public class StatsObject : ScriptableObject
{
    public Attribute[] attributes;
    public int level;
    public int exp;
    public int Health
    {
        get;
        set;
    }
    public int Mana
    {
        get;
        set;
    }
    public float HealthPercentage
    {
        get { int health = Health;
            int maxHealth = health;
            foreach (Attribute attribute in attributes)
            {
                if(attribute.type == AttributeType.Health)
                {
                    maxHealth = attribute.value.ModfiedValue;
                }
            }
            return (maxHealth > 0 ? ((float)health / (float)maxHealth) : 0f);
        }
        
    }

    public float ManaPercentage
    {
        get
        {
            int mana = Mana;
            int maxMana = mana;
            foreach (Attribute attribute in attributes)
            {
                if (attribute.type == AttributeType.Mana)
                {
                    maxMana = attribute.value.ModfiedValue;
                }
            }
            return (maxMana > 0 ? ((float)mana / (float)maxMana) : 0f);
        }

    }

    [NonSerialized]
    private bool isInitialize = false;
    public void OnEnable()
    {
        InitializeAttribute();
    }
    public void InitializeAttribute()
    {
        if (isInitialize)
        {
            return;
        }
        isInitialize = true;

        foreach (Attribute attribute in attributes)
        {
            attribute.value = new ModifiableInt(OnModifiedValue);
        }
        level = 1;
        exp = 0;

        SetBaseValue(AttributeType.Agility,100);
        SetBaseValue(AttributeType.Intellect, 100);
        SetBaseValue(AttributeType.Stamina, 100);
        SetBaseValue(AttributeType.Strength, 100);
        SetBaseValue(AttributeType.Health, 100);
        SetBaseValue(AttributeType.Mana, 100);

        Health = GetModifiedValue(AttributeType.Health);
        Mana = GetModifiedValue(AttributeType.Mana);

    }
    public Action<StatsObject> OnChangedStats;
    private void OnModifiedValue(ModifiableInt value)
    {
        OnChangedStats?.Invoke(this);
    }

    public int GetBaseValue(AttributeType type)
    {
        foreach(Attribute attribute in attributes)
        {
            if (attribute.type == type)
                return attribute.value.BaseValue;
        }
        return -1;
    }
    public void SetBaseValue(AttributeType type, int value)
    {
        foreach (Attribute attribute in attributes)
        {
            if (attribute.type == type)
            {
                attribute.value.BaseValue = value;
            }
        }
        
    }

    public int GetModifiedValue(AttributeType type)
    {
        foreach (Attribute attribute in attributes)
        {
            if (attribute.type == type)
                return attribute.value.ModfiedValue;
        }
        return -1;
    }
    public int AddHealth(int value)
    {
        Health += value;
        OnChangedStats?.Invoke(this);

        return Health;
    }

    public int AddMana(int value)
    {
        Mana += value;
        OnChangedStats?.Invoke(this);

        return Mana;

    }
}

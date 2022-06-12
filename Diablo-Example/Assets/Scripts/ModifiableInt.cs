using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using kang.Core;

[Serializable]
public class ModifiableInt 
{
    [NonSerialized]
    private int baseValue;
    [SerializeField]
    private int modfiedValue;

    public int BaseValue
    {
        get { return baseValue; }
        set { baseValue = value;
            UpdateModifiedValue();
        }
    }
    public int ModfiedValue
    {
        get { return modfiedValue; }
        set { modfiedValue = value;}
    }

    private event Action<ModifiableInt> OnModifiedValue;

    private List<IModifier> modifiers = new List<IModifier> ();

    public ModifiableInt(Action<ModifiableInt> method = null)
    {
        modfiedValue = baseValue;
        RegisterModEvent(method);
    }
    public void RegisterModEvent(Action<ModifiableInt> methods)
    {
        if (methods != null)
        {
            OnModifiedValue += methods;
        }
    }
    public void UnregisterModEvent(Action<ModifiableInt> methods)
    {
        if(methods != null)
        {
            OnModifiedValue -= methods;
        }
    }
    private void UpdateModifiedValue()
    {
        int valueToAdd = 0;
        foreach (IModifier modifier in modifiers)
        {
            modifier.AddValue(ref valueToAdd);
        }
        ModfiedValue = baseValue + valueToAdd;

        OnModifiedValue?.Invoke(this);
    }
    public void AddModifier(IModifier modifier)
    {
        modifiers.Add(modifier);
        UpdateModifiedValue();
    }
    public void RemoveModifier(IModifier modifier)
    {
        modifiers.Remove(modifier);
        UpdateModifiedValue();
    }
}

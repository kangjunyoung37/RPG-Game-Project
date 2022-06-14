using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kang.InventorySystem.Inventory;
using UnityEngine.UI;
using kang.InventorySystem.Items;
public class PlayerStatsUI : MonoBehaviour
{
    public InventoryObject equipement;
    public StatsObject playerStats;
    public Text[] attributeText;

    private void OnEnable()
    {
        playerStats.OnChangedStats += OnChangedStats;
        if(equipement != null && playerStats != null)
        {
            foreach(InventorySlot slot in equipement.Slots)
            {
                slot.OnPreUpdate += OnRemoveItem;
                slot.OnPostUpdat += OnEquipItem;
            }
        }
        UpdateAttributeTexts();
    }
    private void OnDisable()
    {
        playerStats.OnChangedStats -= OnChangedStats;
       
        if (equipement != null && playerStats != null)
        {
            foreach (InventorySlot slot in equipement.Slots)
            {
                slot.OnPreUpdate -= OnRemoveItem;
                slot.OnPostUpdat -= OnEquipItem;
            }
        }
    }
    private void UpdateAttributeTexts()
    {
        attributeText[0].text = playerStats.GetModifiedValue(AttributeType.Agility).ToString("n0");
        attributeText[1].text = playerStats.GetModifiedValue(AttributeType.Intellect).ToString("n0");
        attributeText[2].text = playerStats.GetModifiedValue(AttributeType.Stamina).ToString("n0");
        attributeText[3].text = playerStats.GetModifiedValue(AttributeType.Strength).ToString("n0");

    }
    private void OnRemoveItem(InventorySlot slot)
    {
        if(slot.ItemObject == null)
        {
            return;
        }
        foreach(ItemBuff buff in slot.item.buffs)
        {
            foreach(Attribute attribute in playerStats.attributes)
            {
                if (attribute.type == buff.state)
                {
                    attribute.value.RemoveModifier(buff);
                }
            }
        }

    }
    private void OnEquipItem(InventorySlot slot)
    {
        if (slot.ItemObject == null)
        {
            return;
        }
        foreach (ItemBuff buff in slot.item.buffs)
        {
            foreach (Attribute attribute in playerStats.attributes)
            {
                if (attribute.type == buff.state)
                {
                    attribute.value.AddModifier(buff);
                }
            }
        }
    }

    private void OnChangedStats(StatsObject statsObject)
    {
        UpdateAttributeTexts();
    }
}

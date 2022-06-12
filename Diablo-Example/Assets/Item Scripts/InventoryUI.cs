using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using kang.InventorySystem.Inventory;
using UnityEngine.UI;
using TMPro;
public static class MouseData
{
    public static InventoryUI interfaceMouseIsOver;
    public static GameObject slotHoverdOver;
    public static GameObject tempItemBeingDragged;
}
[RequireComponent(typeof(EventTrigger))]
public abstract class InventoryUI : MonoBehaviour
{
    public InventoryObject inventoryObject;
    private InventoryObject previousInventoryObject;
    public Dictionary<GameObject, InventorySlot> slotUIs = new Dictionary<GameObject, InventorySlot>();

    private void Awake()
    {
        CreateSlotUIs();
        for(int i = 0; i < inventoryObject.Slots.Length; i++)
        {
            inventoryObject.Slots[i].parent = inventoryObject;
            inventoryObject.Slots[i].OnPostUpdat += OnPostUpdate;
        }
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
    }
    protected virtual void Start()
    {
        for(int i = 0; i< inventoryObject.Slots.Length; i++)
        {
            inventoryObject.Slots[i].UpdateSlot(inventoryObject.Slots[i].item, inventoryObject.Slots[i].amount);
        }
    }
    public abstract void CreateSlotUIs();
    protected void AddEvent(GameObject go, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = go.GetComponent<EventTrigger>();
        
        if (trigger == null)
        {
            Debug.LogWarning("No EventTrigger component found!");
            return;
        }
        EventTrigger.Entry eventTrigger = new EventTrigger.Entry { eventID = type };
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);

    }
    public void OnPostUpdate(InventorySlot slot)
    {
        slot.slotUI.transform.GetChild(0).GetComponent<Image>().sprite = slot.item.id < 0 ? null : slot.ItemObject.icon;
        slot.slotUI.transform.GetChild(0).GetComponent<Image>().color = slot.item.id < 0 ? new Color(1, 1, 1, 0) : new Color(1, 1, 1, 1);
        slot.slotUI.GetComponentInChildren<TextMeshProUGUI>().text = slot.item.id < 0 ? string.Empty : (slot.amount == 1 ? string.Empty : slot.amount.ToString("n0"));

    }
    public void OnEnterInterface(GameObject go)
    {
        MouseData.interfaceMouseIsOver = go.GetComponent<InventoryUI>();
    }

    public void OnExitInterface(GameObject go)
    {
        MouseData.interfaceMouseIsOver = null;
    }
    public void OnEnterSlot(GameObject go)
    {
        MouseData.slotHoverdOver = go;
    }
    public void OnExitSlot(GameObject go)
    {
        MouseData.slotHoverdOver = null;
    }
    public void OnStartDrag(GameObject go)
    {
        MouseData.tempItemBeingDragged = CreateDragImage(go);
    }
    public void OnDrag(GameObject go)
    {
        if(MouseData.tempItemBeingDragged == null)
        {
            return;
        }
        MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;

    }
    public void OnEndDrag(GameObject go)
    {
        Destroy(MouseData.tempItemBeingDragged);
        if(MouseData.interfaceMouseIsOver == null)
        {
            slotUIs[go].RemoveItem();
        }
        else if(MouseData.interfaceMouseIsOver)
        {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotUIs[MouseData.slotHoverdOver];
            inventoryObject.SwapItems(slotUIs[go], mouseHoverSlotData);
        }
    }
    private GameObject CreateDragImage(GameObject go)
    {
        if(slotUIs[go].item.id < 0)
        {
            return null;
        }
        GameObject dragImageGo = new GameObject();
        RectTransform rectTrasform = dragImageGo.AddComponent<RectTransform>();
        rectTrasform.sizeDelta = new Vector2(50, 50);
        dragImageGo.transform.SetParent(transform.parent);

        Image image = dragImageGo.AddComponent<Image>();
        image.sprite = slotUIs[go].ItemObject.icon;
        image.raycastTarget = false;

        dragImageGo.name = "Drag Image";
        return dragImageGo; 

    }
    public void OnClick(GameObject go, PointerEventData data)
    {
        InventorySlot slot = slotUIs[go];
        if(slot == null)
        {
            return;
        }
        if(data.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick(slot);
        }
        if (data.button == PointerEventData.InputButton.Right)
        {
            OnRightClick(slot);
        }
    }
    protected virtual void OnRightClick(InventorySlot slot)
    {

    }
    protected virtual void OnLeftClick(InventorySlot slot)
    {

    }

}

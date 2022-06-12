using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kang.InventorySystem.Inventory;
using UnityEngine.EventSystems;
using kang.InventorySystem.Items;
public class DynamicInventoryUI : InventoryUI
{
    [SerializeField]
    protected GameObject slotPrefab;
    [SerializeField]
    protected Vector2 start;

    [SerializeField]
    protected Vector2 size;

    [SerializeField]
    protected Vector2 space;

    [Min(1), SerializeField]
    protected int numberOfColum = 4;
    public override void CreateSlotUIs()
    {
        slotUIs = new Dictionary<GameObject , InventorySlot>();
        for (int i = 0; i< inventoryObject.Slots.Length; ++i)
        {
            GameObject go = Instantiate(slotPrefab,Vector3.zero,Quaternion.identity,transform);
            go.GetComponent<RectTransform>().anchoredPosition = CalculatePosition(i);

            AddEvent(go, EventTriggerType.PointerEnter, delegate { OnEnterSlot(go); });
            AddEvent(go, EventTriggerType.PointerExit, delegate { OnExitSlot(go); });
            AddEvent(go, EventTriggerType.BeginDrag, delegate { OnStartDrag(go); });
            AddEvent(go, EventTriggerType.EndDrag, delegate { OnEndDrag(go); });
            AddEvent(go, EventTriggerType.Drag, delegate { OnDrag(go); });
            AddEvent(go, EventTriggerType.PointerClick, (data) => { OnClick(go, (PointerEventData)data); });
            inventoryObject.Slots[i].slotUI = go;
            slotUIs.Add(go, inventoryObject.Slots[i]);

            go.name += ": " + i;


        }
    }
    public Vector3 CalculatePosition(int i)
    {
        float x = start.x + ((space.x + size.x) * (i % numberOfColum));
        float y = start.y + (-(space.y + size.y) * (i / numberOfColum));

        return new Vector3(x, y, 0f);
    }

    protected override void OnRightClick(InventorySlot slot)
    {
        inventoryObject.UseItem(slot);

    }

}

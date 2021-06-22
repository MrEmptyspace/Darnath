using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemDragHandler : ItemDragHandler
{
    private Inventory inventory;

    public bool IsSplitting { get; set; }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        if(eventData.hovered.Count == 0 && eventData.button == PointerEventData.InputButton.Left)
        {
            if(ItemSlotUI != null)
            {
                if(inventory != null)
                {
                    InventorySlot thisSlot = ItemSlotUI as InventorySlot;
                    inventory = thisSlot.Inventory;
                    Debug.Log("Drop Item and remove it from inventory with left click");

                    inventory.DropItem(thisSlot);
                    //inventory.ItemContainer.RemoveAt(thisSlot.SlotIndex);
                }
                else
                {
                    InventorySlot thisSlot = ItemSlotUI as InventorySlot;
                    inventory = thisSlot.Inventory;
                    inventory.DropItem(thisSlot);
                }
            }
        }

        IsSplitting = false;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            InventorySlot thisSlot = ItemSlotUI as InventorySlot;
            inventory = thisSlot.Inventory;
     
            if(thisSlot.ItemSlot.quantity != 0)
            {
                inventory.Split(thisSlot.SlotIndex, eventData);
                IsSplitting = true;
            }
            //Debug.Log("Splitting slot at index " + thisSlot.SlotIndex + " with quantity : " + thisSlot.ItemSlot.quantity);

        }

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            base.OnPointerDown(eventData);
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
    }

}

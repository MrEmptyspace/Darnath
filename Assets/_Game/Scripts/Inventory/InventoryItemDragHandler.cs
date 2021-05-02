using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemDragHandler : ItemDragHandler
{
    private Inventory inventory;

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        //May have to skip the first trigger of this if for some reason
        if(eventData.hovered.Count == 0)
        {
            if(ItemSlotUI != null)
            {
                if(inventory != null)
                {
                    InventorySlot thisSlot = ItemSlotUI as InventorySlot;
                    inventory = thisSlot.Inventory;
                    Debug.Log("Drop Item and remove it from inventory");

                    inventory.DropItem(thisSlot);
                    inventory.ItemContainer.RemoveAt(thisSlot.SlotIndex);
                }
                else
                {
                    InventorySlot thisSlot = ItemSlotUI as InventorySlot;
                    inventory = thisSlot.Inventory;
                }
            }
        }
    }

}

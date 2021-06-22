using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HotbarItemDragHandler : ItemDragHandler
{
    private Inventory inventory;

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            base.OnPointerUp(eventData);
            if(eventData.hovered.Count == 0)
            {

                if (ItemSlotUI != null)
                {
                    HotbarSlot thisSlot = ItemSlotUI as HotbarSlot;

                    inventory = thisSlot.Inventory;

                    if (inventory != null)
                    {
                        inventory.DropItem(thisSlot);
                    }
                }
            }
        }

    }
}

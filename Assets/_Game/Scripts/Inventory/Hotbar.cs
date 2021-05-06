using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotbar: MonoBehaviour
{
    [SerializeField] public HotbarSlot[] hotbarSlots = new HotbarSlot[10];

    private void Start()
    {
         hotbarSlots = GetComponentsInChildren<HotbarSlot>();
    }

    //Shift click something to hotbar, put anywhere
    public void Add(HotbarItem itemToAdd)
    {
        foreach (HotbarSlot hotbarSlot in hotbarSlots)
        {
            //if (hotbarSlot.AddItem(itemToAdd))
            //{
             //   return;
            //}
        }
    }

    internal void Add(ItemSlot itemSlotWithItem, int slotIndex)
    {
        bool added = false;

        //foreach (HotbarSlot hotbarSlot in hotbarSlots)
        //{
        //    if(hotbarSlot.SlotItem != null)
        //    {
        //        if (hotbarSlot.SlotItem == itemSlotWithItem.item)
        //        {
        //            hotbarSlot.SlotItem = itemSlotWithItem.item;
        //            hotbarSlot.ReferenceSlotIndex = slotIndex;
        //            hotbarSlot.UpdateSlotUI(null);//Pull the new quantity using the reference link;
        //            if (hotbarSlot)
        //            {

        //            }
        //            //break;
        //        }
        //    }
        //    else
        //    {
        //        hotbarSlot.SlotItem = itemSlotWithItem.item;
        //        hotbarSlot.ReferenceSlotIndex = slotIndex;
        //        hotbarSlot.UpdateSlotUI(null);
        //        break;
        //    }

        //    //Loop through each slot and check if the item im adding has the index of any in the hot bar slot
        //    ////If it does match 
        //    ////if I find the same item in my hot bar, update the UI
        //    if (hotbarSlot.ReferenceSlotIndex == slotIndex)
        //    {
        //        hotbarSlot.SlotItem = itemSlotWithItem.item;
        //        hotbarSlot.ReferenceSlotIndex = slotIndex;
        //        hotbarSlot.UpdateSlotUI(null);//Pull the new quantity using the reference link;
        //        break;
        //    }
        //    else
        //    {
        //        //Add to the nearest null hotbar

        //    }
            //else 
            //{
            //    //if (hotbarSlot.SlotItem != null) { break; }
            //    hotbarSlot.SlotItem = itemSlotWithItem.item;
            //    hotbarSlot.ReferenceSlotIndex = slotIndex;
            //}

            ////If hot bar does not have and Item in it.
            //if (hotbarSlot.ReferenceSlotIndex == slotIndex)
            //{
            //    //Update the ui of that slot to show new quantity
            //    if(hotbarSlot.SlotItem == null)
            //    {
            //        hotbarSlot.SlotItem = itemSlotWithItem.item;
            //        hotbarSlot.ReferenceSlotIndex = slotIndex;
            //    }
            //    hotbarSlot.UpdateSlotUI(null);
            //    break;
            //}
            //else 
            //{
            //    //if (hotbarSlot.SlotItem != null) { break; }
            //    hotbarSlot.SlotItem = itemSlotWithItem.item;
            //    hotbarSlot.ReferenceSlotIndex = slotIndex;
            //}

            //if (hotbarSlot.AddItem(itemSlotWithItem))
            //{
            //    return;
            //}
        //}
    }

    public void Remove(HotbarSlot thisSlot)
    {
        foreach (var slot in hotbarSlots)
        {
           if(slot.SlotItem == thisSlot.SlotItem)
            {
                slot.SlotItem = null;
                slot.UpdateSlotUI(null);
            }
        }
    }

    internal void Remove(InventorySlot thisSlot)
    {
        foreach (var slot in hotbarSlots)
        {
            if (slot.SlotItem == thisSlot.SlotItem)
            {
                slot.SlotItem = null;
                slot.UpdateSlotUI(null);
            }
        }
    }
}

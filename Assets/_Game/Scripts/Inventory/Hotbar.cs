using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MCEvents;

public class Hotbar: MonoBehaviour
{
    [SerializeField] public HotbarSlot[] hotbarSlots = new HotbarSlot[9];

    private void Start()
    {
        hotbarSlots = GetComponentsInChildren<HotbarSlot>();

        EventManager.StartListening(Events.HotbarUpdated, UpdateHotbarSlots);
    }

    private void UpdateHotbarSlots(Dictionary<string, object> message)
    {
        //Merge what happens when two slots get merged from the inventory that are in the hotbar
        int firstSlotIndex = -1;
        int secondSlotIndex = -1;
        bool merge;
        if (message != null && message.ContainsKey("firstSlotIndex"))
        {
            firstSlotIndex = (int)message["firstSlotIndex"];
            //Debug.Log("firstSlotIndex = " + firstSlotIndex);
        }
        if (message != null && message.ContainsKey("secondSlotIndex"))
        {
            secondSlotIndex = (int)message["secondSlotIndex"];
            //Debug.Log("secondSlotIndex = " + secondSlotIndex);
        }
        if (message != null && message.ContainsKey("merge"))
        {
            merge = (bool)message["merge"];
            //Debug.Log("merge = " + merge);
        }

        if(firstSlotIndex != -1)
        {
            foreach (HotbarSlot hotbarSlot in hotbarSlots)
            {
                if (hotbarSlot.SlotIndex == firstSlotIndex)
                {
                    //return;
                    hotbarSlot.ItemSlot = hotbarSlot.Inventory.ItemContainer.GetSlotByIndex(firstSlotIndex);
                    hotbarSlot.UpdateSlotUI(null);
                }
            }
        }
    }

    public void AddItemSlotToHotbar(ItemSlot itemSlot)
    {
        foreach (HotbarSlot hotbarSlot in hotbarSlots)
        {
            if (Add(itemSlot))
            {
               return;
            }
        }
    }

    internal bool Add(ItemSlot itemSlotWithItem)
    {
        bool added = false;
        foreach (HotbarSlot hotbarSlot in hotbarSlots)
        {
            if (hotbarSlot.SlotItem != null)
            {
                if (hotbarSlot.ItemSlot.EqualsWithoutQuantity(itemSlotWithItem))
                {
                    //If the slot is full continue to the next empty slot
                    if(hotbarSlot.ItemSlot.quantity >= hotbarSlot.ItemSlot.item.MaxStack)
                    {
                        continue;
                    }
                    else
                    {
                        hotbarSlot.SlotItem = itemSlotWithItem.item;
                        hotbarSlot.ItemSlot = itemSlotWithItem;
                        hotbarSlot.UpdateSlotUI(null);
                        added = true;
                        break;
                    }
                }
            }
            else
            {
                hotbarSlot.SlotItem = itemSlotWithItem.item;
                hotbarSlot.ItemSlot = itemSlotWithItem;
                hotbarSlot.UpdateSlotUI(null);
                added = true;
                break;
            }
        }

        return added;
    }
    public void Remove(HotbarSlot thisSlot)
    {
        foreach (HotbarSlot slot in hotbarSlots)
        {
            if (slot.SlotIndex == thisSlot.SlotIndex)
            {
                slot.SlotIndex = -1;
                slot.SlotItem = null;
                slot.ItemSlot = new ItemSlot();
                slot.UpdateSlotUI(null);
                break;
            }
        }
    }

    internal void Remove(InventorySlot thisSlot)
    {
        foreach (HotbarSlot slot in hotbarSlots)
        {
            if (slot.ItemSlot == thisSlot.ItemSlot)
            {
                slot.SlotItem = null;
                slot.ItemSlot = new ItemSlot();
                slot.UpdateSlotUI(null);
                break;
            }
        }
    }
}

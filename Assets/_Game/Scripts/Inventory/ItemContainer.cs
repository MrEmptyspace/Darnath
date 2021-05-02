using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class ItemContainer : IItemContainer
{
    private ItemSlot[] itemSlots = new ItemSlot[24];

    //public GameEvent OnItemsUpdated;

    public ItemContainer(int size) => itemSlots = new ItemSlot[size];

    public ItemSlot GetSlotByIndex(int index) => itemSlots[index];
    public ItemSlot AddItem(ItemSlot itemSlot)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].item != null)//If it exists
            {
                //Is already in the list
                //if (itemSlots[i].item == itemSlot.item)
                if (itemSlots[i].item.itemName == itemSlot.item.itemName)
                {
                    int slotRemainingSpace = itemSlots[i].item.MaxStack - itemSlots[i].quantity;

                    //Add item into existing stack
                    if (itemSlot.quantity <= slotRemainingSpace)
                    {
                        itemSlots[i].quantity += itemSlot.quantity;
                        itemSlot.quantity = 0;

                        //Setup Stack implementation
                        itemSlots[i].items.AddRange(itemSlot.items);
                        itemSlot.items = new List<InventoryItem>();

                        EventManager.TriggerEvent("InventoryUpdated");
                        return itemSlot;
                    }
                    else if (slotRemainingSpace > 0) //Stack as much as possible
                    {
                        itemSlots[i].quantity += slotRemainingSpace;
                        itemSlot.quantity -= slotRemainingSpace;

                        //Add as much as possible from the added item slot to the new item slot and remove Inventory items from list
                        for (int x = slotRemainingSpace - 1; x >= 0; x--)
                        {
                            itemSlots[i].items.Add(itemSlot.items[x]);
                            itemSlot.items.Remove(itemSlot.items[x]);
                        }

                        //Setup Stack implementation
                        //The item we just added needs to have its stack 
                        //itemSlots[i].items.Push(itemSlots[i].item);
                        //itemSlot.items = new Stack<InventoryItem>();
                    }

                }

            }
        }

        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].item == null)
            {
                if (itemSlot.quantity <= itemSlot.item.MaxStack)
                {
                    itemSlots[i] = itemSlot;
                    itemSlot.quantity = 0;

                    itemSlots[i].items= itemSlot.items;
                    itemSlot.items = new List<InventoryItem>();

                    //OnItemsUpdated.Raise();
                    EventManager.TriggerEvent(EventManager.Events.InventoryUpdated);
                    return itemSlot;
                }
                else
                {
                    itemSlots[i] = new ItemSlot(itemSlot.item, itemSlot.item.MaxStack);

                    itemSlot.quantity -= itemSlot.item.MaxStack;

                }
            }
        }
        //OnItemsUpdated.Raise();
        EventManager.TriggerEvent(EventManager.Events.InventoryUpdated);

        return itemSlot;
    }
    public int GetTotalQuantity(InventoryItem item)
    {
        int totalCount = 0;

        foreach (ItemSlot itemSlot in itemSlots)
        {
            if (itemSlot.item == null) { continue; }
            if (itemSlot.item != item) { continue; }

            totalCount += itemSlot.quantity;
        }

        return totalCount;
    }

    public bool HasItem(InventoryItem item)
    {
        foreach (ItemSlot itemSlot in itemSlots)
        {
            if (itemSlot.item == null) { continue; }
            if (itemSlot.item != item) { continue; }

            return true;
        }

        return false;
    }

    public void RemoveAt(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex > itemSlots.Length - 1) { return; }

        itemSlots[slotIndex] = new ItemSlot();

        //OnItemsUpdated.Raise();
        EventManager.TriggerEvent(EventManager.Events.InventoryUpdated);

    }

    public void RemoveItem(ItemSlot itemSlot)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].item == itemSlot.item)
            {
                if (itemSlots[i].quantity < itemSlot.quantity)
                {
                    itemSlot.quantity -= itemSlots[i].quantity;

                    itemSlots[i] = new ItemSlot();
                }
                else
                {
                    itemSlots[i].quantity -= itemSlot.quantity;

                    if (itemSlots[i].quantity == 0)
                    {
                        itemSlots[i] = new ItemSlot();
                        //OnItemsUpdated.Raise();
                        EventManager.TriggerEvent(EventManager.Events.InventoryUpdated);                      
                    }

                    return;
                }

            }
        }
    }

    public void Swap(int indexOne, int indexTwo)
    {
        ItemSlot firstSlot = itemSlots[indexOne];
        ItemSlot secondSlot = itemSlots[indexTwo];

        //If dropping onto same slot do nothing
        if (firstSlot == secondSlot) { return; }

        //Dragging to filled slot
        if (secondSlot.item != null)
        {
            //Both items are the same
            if (firstSlot.item == secondSlot.item)
            {
                int secondSlotRemainingSpace = secondSlot.item.MaxStack - secondSlot.quantity;
                //Combine stack if possible
                if (firstSlot.quantity <= secondSlotRemainingSpace)
                {
                    itemSlots[indexTwo].quantity += firstSlot.quantity;
                    itemSlots[indexOne] = new ItemSlot();

                    //OnItemsUpdated.Raise();
                    EventManager.TriggerEvent(EventManager.Events.InventoryUpdated);

                    return;
                }
            }
        }

        itemSlots[indexOne] = secondSlot;
        itemSlots[indexTwo] = firstSlot;
        EventManager.TriggerEvent(EventManager.Events.InventoryUpdated);
        //OnItemsUpdated.Raise();
    }
}


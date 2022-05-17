using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MCEvents;

using System;
using System.Linq;

public class ItemContainer : IItemContainer
{
    private ItemSlot[] itemSlots = new ItemSlot[24];

    public ItemContainer(int size) => itemSlots = new ItemSlot[size];

    public ItemSlot GetSlotByIndex(int index) => itemSlots[index];
    
   public int GetIndexBySlot(ItemSlot itemSlot)
    {
        int index = -1;
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if(itemSlots[i].item != null)
            {
                if (itemSlots[i].item == itemSlot.item)
                {
                    index = i;
                    break;
                }     
            }
        }
        Debug.Log("Searched for index " + index);
        return index;
    }

    public ItemSlot AddItem(ItemSlot itemSlot)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].item != null)//If it exists
            {
                //Is already in the list
                if (itemSlots[i].item.itemName == itemSlot.item.itemName)
                {
                    int slotRemainingSpace = itemSlots[i].item.MaxStack - itemSlots[i].quantity;

                    //Add item into existing stack
                    if (itemSlot.quantity <= slotRemainingSpace)
                    {
                        itemSlots[i].quantity += itemSlot.quantity;
                        //itemSlot.quantity = 0;

                        //Setup Stack implementation
                        itemSlots[i].items.AddRange(itemSlot.items);
                        itemSlot.items = new List<InventoryItem>();

                        EventManager.TriggerEvent("InventoryUpdated");
                        return itemSlots[i];
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
                    EventManager.TriggerEvent(Events.InventoryUpdated);
                    return itemSlot;
                }
                else
                {
                    itemSlots[i] = new ItemSlot(itemSlot.item, itemSlot.item.MaxStack);

                    itemSlot.quantity -= itemSlot.item.MaxStack;

                }
            }
        }
        EventManager.TriggerEvent(Events.InventoryUpdated);

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

        EventManager.TriggerEvent(Events.InventoryUpdated);
    }

    public void RemoveItem(ItemSlot itemSlot, int slotIndex = 0)
    {
        throw new NotImplementedException();
    }

    public void Swap(int indexOne, int indexTwo)
    {
        ItemSlot firstSlot = itemSlots[indexOne];
        ItemSlot secondSlot = itemSlots[indexTwo];

        //Dragging to filled slot
        if (secondSlot.item != null)
        {
            if (firstSlot.item.Equals(secondSlot.item))
            {
                Merge(indexTwo, firstSlot , indexOne);
                return;
            }
        }

        itemSlots[indexOne] = secondSlot;
        itemSlots[indexTwo] = firstSlot;

        EventManager.TriggerEvent(Events.InventoryUpdated);
        //Both slots have swapped. loop through hotbar and update indexes
        EventManager.TriggerEvent(Events.HotbarUpdated, new Dictionary<string, object> { { "firstSlotIndex", indexOne }, { "secondSlotIndex", indexTwo } });
    }

    public void Merge(int mergeSlotIndex, ItemSlot slotToMerge , int mergedFromIndex = -1)
    {
        ItemSlot slotMergedTo = itemSlots[mergeSlotIndex];

        //If empty
        if(slotMergedTo != null && slotMergedTo.quantity == 0)
        {
            itemSlots[mergeSlotIndex] = slotToMerge;
        }
        else if (slotMergedTo.item.Equals(slotToMerge.item))
        {
            int secondSlotRemainingSpace = slotMergedTo.item.MaxStack - slotMergedTo.quantity;
            //Combine stack if possible
            if (slotToMerge.quantity <= secondSlotRemainingSpace)
            {
                slotMergedTo.quantity += slotToMerge.quantity;
                slotToMerge.items.ForEach(item => slotMergedTo.items.Add(item));

                itemSlots[mergeSlotIndex] = slotMergedTo;

                if(mergedFromIndex != -1)
                {
                    itemSlots[mergedFromIndex] = new ItemSlot();
                }

                EventManager.TriggerEvent(Events.InventoryUpdated);
                //indexOne Item is now null, so check for that index and make it equal second index
                EventManager.TriggerEvent(Events.HotbarUpdated, new Dictionary<string, object> { { "firstSlotIndex", null }, { "secondSlotIndex", mergeSlotIndex }, { "merge", true } });
                return;
            }
        }

        EventManager.TriggerEvent(Events.InventoryUpdated);
    }

    public ItemSlot Split(int indexOne)
    {
        ItemSlot firstSlot = itemSlots[indexOne];  
        
        if(firstSlot.quantity == 1) { return firstSlot; }

        int splitQuantity = (int) Math.Round(firstSlot.quantity / 2.0);

        ItemSlot splitSlot = new ItemSlot();
        splitSlot.items = new List<InventoryItem>();

        //Take last items out of stack and put them into split slot
        int counter = 0;
        List<int> removedFromSlot = new List<int>();
        for (int i = firstSlot.items.Count - 1; i >= 0; i--)
        {
            //Shitty break condition
            if (counter == splitQuantity)
            {
                break;
            }
            splitSlot.items.Add(firstSlot.items[i]);
            removedFromSlot.Add(i);
            
            counter++;
        }

        removedFromSlot.ForEach(itemIndex => firstSlot.items.RemoveAt(itemIndex));

        splitSlot.item = splitSlot.items[0];
        splitSlot.quantity = splitQuantity;

        //firstSlot.items.RemoveRange(firstSlot.items.Count - splitQuantity, splitQuantity);
        firstSlot.item = firstSlot.items[0];
        firstSlot.quantity = firstSlot.quantity - splitQuantity;
        itemSlots[indexOne] = firstSlot;

        EventManager.TriggerEvent(Events.InventoryUpdated);
        EventManager.TriggerEvent(Events.HotbarUpdated, new Dictionary<string, object> { { "firstSlotIndex", indexOne }});

        return splitSlot;
    }
}


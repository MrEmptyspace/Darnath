using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ItemSlot 
{
    public InventoryItem item;
    public List<InventoryItem> items;
    public int quantity;

    public ItemSlot(InventoryItem item, int quantity)
    {
        items = new List<InventoryItem>();
        items.Add(item);
        this.item = item;
        this.quantity = quantity;
    }

    public static bool operator == (ItemSlot a, ItemSlot b) { return a.Equals(b); }
    public static bool operator != (ItemSlot a, ItemSlot b) { return !a.Equals(b); }
}
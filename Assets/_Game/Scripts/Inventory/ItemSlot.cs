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

    public override bool Equals(object obj)
    {
        if (!(obj is ItemSlot))
        {
            return false;
        }

        var slot = (ItemSlot)obj;

        if(this.quantity != slot.quantity)
        {
            return false;
        }

        if(!item.Equals(slot.item))
        {
            return false;
        }

        return true;
        //return EqualityComparer<InventoryItem>.Default.Equals(item, slot.item) &&
        //       quantity == slot.quantity;
    }

    public bool EqualsWithoutQuantity(object obj)
    {
        if (!(obj is ItemSlot))
        {
            return false;
        }

        var slot = (ItemSlot)obj;

        //if (this.quantity != slot.quantity)
        //{
        //    return false;
        //}

        if (!item.Equals(slot.item))
        {
            return false;
        }

        return true;
        //return EqualityComparer<InventoryItem>.Default.Equals(item, slot.item) &&
        //       quantity == slot.quantity;
    }


    //public static bool operator == (ItemSlot a, ItemSlot b) { return a.Equals(b); }
    public static bool operator == (ItemSlot a, ItemSlot b) { return a.Equals(b); }
    public static bool operator != (ItemSlot a, ItemSlot b) { return !a.Equals(b); }



}

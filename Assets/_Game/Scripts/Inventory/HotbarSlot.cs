using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HotbarSlot : ItemSlotUI , IDropHandler
{
    [SerializeField] private Inventory inventory = null;
    [SerializeField] private TextMeshProUGUI itemQuantityText = null;
    public Inventory Inventory { get => inventory; set => inventory = value; }
    private int referenceSlotIndex = -1;
    private HotbarItem slotItem = null;

    //public override HotbarItem SlotItem
    //{
    //    get { return slotItem; }
    //    set { slotItem = value; UpdateSlotUI(null); }
    //}

    public override HotbarItem SlotItem
    {
        get { return slotItem; }
        set { slotItem = value; UpdateSlotUI(null); }
    }
    // Switch out the HotBarItem Slotitem for an acutal ItemSlot and remove all referenceSlotIndex as we now store the reference to the inventory.

    //public int ReferenceSlotIndex { get => referenceSlotIndex; set => referenceSlotIndex = value; }

    public void OnEnable()
    {
        //EventManager.StartListening("InventoryUpdated", UpdateSlotUI);
        UpdateSlotUI(null);
    }

    public void Start()
    {
        referenceSlotIndex = -1;
        UpdateSlotUI(null);
    }

    //internal bool AddItem(ItemSlot itemSlotWithItem, int slotIndex)
    //{
    //    if (!= null)
    //    {

    //    }
    //        //if (slotItem != null) { return false; }

    //        SlotItem = itemSlotWithItem.item;
    //    referenceSlotIndex = slotIndex;
    //    //SlotIndex = slotIndex;

    //    return true;

    //}

    //internal bool AddItem(HotbarItem itemToAdd)
    //{
    //    //if(slotItem != null) { return false; }

    //    SlotItem = itemToAdd;
    //    return true;
    //}

    public void UseSlot(int index)
    {
        if(index != SlotIndex) { return; }
        //Use Item
    }

    public override void OnDrop(PointerEventData eventData)
    {
        ItemDragHandler itemDragHandler = eventData.pointerDrag.GetComponent<ItemDragHandler>();
        if (itemDragHandler == null) { return; }

        InventorySlot inventorySlot = itemDragHandler.ItemSlotUI as InventorySlot;
        if (inventorySlot != null)
        {
            SlotItem = inventorySlot.ItemSlot.item;
            //SlotIndex = inventorySlot.SlotIndex;
            referenceSlotIndex = inventorySlot.SlotIndex;
        }

        //If dropped onto another hotbar slot
        HotbarSlot hotBarSlot = itemDragHandler.ItemSlotUI as HotbarSlot;
        if (hotBarSlot != null)
        {           
            HotbarItem oldItem = SlotItem;
            SlotItem = hotBarSlot.SlotItem;
            //referenceSlotIndex = hotBarSlot.referenceSlotIndex;
            hotBarSlot.SlotItem = oldItem;
            return;
        }


    }

    public override void UpdateSlotUI(Dictionary<string, object> message)
    {
        if (SlotItem == null)
        {
            EnableSlotUI(false);
            return;
        }
        itemIconImage.sprite = SlotItem.Icon;
        EnableSlotUI(true);

        //Set new index for connection to inventory
        //SlotIndex = SlotItem.;
        
        if(referenceSlotIndex != -1)
        {
            SetItemQuantityUI();
        }
    }



    private void SetItemQuantityUI()
    {
        if(SlotItem is InventoryItem inventoryItem)
        {
            var referencedSlot = inventory.ItemContainer.GetSlotByIndex(referenceSlotIndex);
            if (referencedSlot.item != null)
            {
                //int quantityCount = inventory.ItemContainer.GetTotalQuantity(inventoryItem);
                int quantityCount = referencedSlot.quantity;
                itemQuantityText.text = quantityCount > 1 ? quantityCount.ToString() : "";
            }
            else
            {
                //SlotItem = null;
            }
            //If getSlotbyIndexes item is nott null
            //if (inventory.ItemContainer.HasItem(inventoryItem))
            //{
            //    int quantityCount = inventory.ItemContainer.GetTotalQuantity(inventoryItem);
            //    itemQuantityText.text = quantityCount > 1 ? quantityCount.ToString() : "";
            //}
            //else
            //{
            //    SlotItem = null;
            //}
        }
        else
        {
            itemQuantityText.enabled = false;
        }
    }

    protected override void EnableSlotUI(bool enable)
    {
        base.EnableSlotUI(enable);
        itemQuantityText.enabled = enable;
        //If you wanted spell cool down
    }
}

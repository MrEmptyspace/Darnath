using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SplitHandler : ItemSlotUI, IDropHandler
{
    [SerializeField] private Inventory inventory = null;
    [SerializeField] private TextMeshProUGUI itemQuantityText = null;
    public Inventory Inventory { get => inventory; set => inventory = value; }
    //private int referenceSlotIndex = -1;
    private HotbarItem slotItem = null;
    private ItemSlot itemSlot;

    public override HotbarItem SlotItem
    {
        get { return slotItem; }
        set { slotItem = value; UpdateSlotUI(null); }
    }

    public ItemSlot ItemSlot { get => itemSlot; set => itemSlot = value; }

    public void OnEnable()
    {
        //EventManager.StartListening("InventoryUpdated", UpdateSlotUI);
        UpdateSlotUI(null);
    }

    public void Start()
    {
        UpdateSlotUI(null);
    }

    public void UseSlot(int index)
    {
        if (index != SlotIndex) { return; }
        //Use Item
    }

    public override void OnDrop(PointerEventData eventData)
    {
        //ItemDragHandler itemDragHandler = eventData.pointerDrag.GetComponent<ItemDragHandler>();
        //if (itemDragHandler == null) { return; }

        //InventorySlot inventorySlot = itemDragHandler.ItemSlotUI as InventorySlot;
        //if (inventorySlot != null)
        //{
        //    SlotItem = inventorySlot.SlotItem;
        //    ItemSlot = inventorySlot.ItemSlot;

        //    UpdateSlotUI(null);
        //}

        ////If dropped onto another hotbar slot
        //HotbarSlot hotBarSlot = itemDragHandler.ItemSlotUI as HotbarSlot;
        //if (hotBarSlot != null)
        //{
        //    //Swap items and itemslots
        //    ItemSlot oldItemSlot = ItemSlot;

        //    SlotItem = hotBarSlot.SlotItem;
        //    ItemSlot = hotBarSlot.ItemSlot;

        //    hotBarSlot.SlotItem = oldItemSlot.item;
        //    hotBarSlot.ItemSlot = oldItemSlot;

        //    UpdateSlotUI(null);
        //    return;
        //}
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
        SetItemQuantityUI(); //itemIconImage.sprite = ItemSlot.item.Icon;
    }



    private void SetItemQuantityUI()
    {
        if (SlotItem is InventoryItem inventoryItem)
        {
            if (itemSlot.item != null)
            {
                int quantityCount = itemSlot.quantity;
                itemQuantityText.text = quantityCount > 1 ? quantityCount.ToString() : "";
            }
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

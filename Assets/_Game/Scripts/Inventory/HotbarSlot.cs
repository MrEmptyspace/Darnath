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
    [SerializeField] public TextMeshProUGUI activeSlotText = null;
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
        //EventManager.StartListening("HotB", UpdateSlotUI);
        UpdateSlotUI(null);
    }

    protected override void Start()
    {
        //SlotIndex = transform.GetSiblingIndex();
        SlotIndex = -1;
        UpdateSlotUI(null);
    }

    public override void OnDrop(PointerEventData eventData)
    {
        ItemDragHandler itemDragHandler = eventData.pointerDrag.GetComponent<ItemDragHandler>();
        if (itemDragHandler == null) { return; }

        InventorySlot inventorySlot = itemDragHandler.ItemSlotUI as InventorySlot;
        if (inventorySlot != null)
        {
            SlotItem = inventorySlot.SlotItem;
            ItemSlot = inventorySlot.ItemSlot;
            SlotIndex = inventorySlot.SlotIndex;
            UpdateSlotUI(null);
        }
        HotbarSlot hotbarSlot2 = itemDragHandler.ItemSlotUI as HotbarSlot;
        if (eventData.button == PointerEventData.InputButton.Left && hotbarSlot2 != null)
        {
            ItemSlot hotbarItemSlot1 = ItemSlot;
            int itemSlot1Index = SlotIndex;

            //If this slot was empty just replace it and cleanup the other one          
            if(ItemSlot.quantity == 0)
            {
                SlotItem = hotbarSlot2.SlotItem;
                ItemSlot = hotbarSlot2.ItemSlot;
                SlotIndex = hotbarSlot2.SlotIndex;

                //reset hotbar slot that was dropped
                hotbarSlot2.SlotItem = null;
                hotbarSlot2.ItemSlot = new ItemSlot();
                hotbarSlot2.SlotIndex = -1;

            }
            else {
                //Set this slot to the slot that was dropped on it
                SlotItem = hotbarSlot2.SlotItem;
                ItemSlot = hotbarSlot2.ItemSlot;
                SlotIndex = hotbarSlot2.SlotIndex;

                hotbarSlot2.SlotItem = SlotItem;
                hotbarSlot2.ItemSlot = ItemSlot;
                hotbarSlot2.SlotIndex = itemSlot1Index;
            }

            UpdateSlotUI(null);
        }
        //If dropped onto another hotbar slot
        //HotbarSlot hotbarSlot2 = itemDragHandler.ItemSlotUI as HotbarSlot;
        //if (hotbarSlot2 != null)
        //{
        //    //Swap items and itemslots
        //    //ItemSlot hotbarItemSlot1 = ItemSlot;
        //    //ItemSlot itemslot2 = hotbarSlot2.ItemSlot;

        //    int itemSlot1Index = hotbarSlot2.SlotIndex;

        //    SlotItem = hotbarSlot2.SlotItem;
        //    ItemSlot = hotbarSlot2.ItemSlot;
        //    SlotIndex = hotbarSlot2.SlotIndex;

        //    hotbarSlot2.SlotItem = ItemSlot.item;
        //    hotbarSlot2.ItemSlot = ItemSlot;
        //    hotbarSlot2.SlotIndex = itemSlot1Index;

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
        if(SlotItem is InventoryItem inventoryItem)
        {
            //var referencedSlot = inventory.ItemContainer.GetSlotByIndex(referenceSlotIndex);
            if (itemSlot.item != null)
            {
                //int quantityCount = inventory.ItemContainer.GetTotalQuantity(inventoryItem);
                int quantityCount = itemSlot.quantity;
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

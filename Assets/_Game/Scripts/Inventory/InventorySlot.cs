using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : ItemSlotUI, IDropHandler
{
    //[SerializeField] private Inventory inventory = null;
    private Inventory inventory = null;
    [SerializeField] private TextMeshProUGUI itemQuantityText = null;


    public override HotbarItem SlotItem
    {
        get { return ItemSlot.item; }
        set { }
    }

    public void OnEnable()
    {
        inventory = this.transform.parent.parent.GetComponent<Inventory>();
        SlotIndex = transform.GetSiblingIndex();
        EventManager.StartListening("InventoryUpdated",UpdateSlotUI);
        UpdateSlotUI(null);
    }

    public void OnDisable()
    {
       //EventManager.StopListening(EventManager.Events.InventoryUpdated, UpdateSlotUI);
    }

    public ItemSlot ItemSlot => inventory.ItemContainer.GetSlotByIndex(SlotIndex);

    public Inventory Inventory { get => inventory; set => inventory = value; }

    public override void OnDrop(PointerEventData eventData)
    {
        ItemDragHandler itemDragHandler = eventData.pointerDrag.GetComponent<ItemDragHandler>();

        if(itemDragHandler == null) { return; }

        if((itemDragHandler.ItemSlotUI as InventorySlot) != null)
        {
            inventory.ItemContainer.Swap(itemDragHandler.ItemSlotUI.SlotIndex, SlotIndex);
        }
    }


    public override void UpdateSlotUI(Dictionary<string, object> message)
    {
        if (ItemSlot.item == null)
        {
            EnableSlotUI(false);
            return;
        }

        EnableSlotUI(true);

        itemIconImage.sprite = ItemSlot.item.Icon;
        itemQuantityText.text = ItemSlot.quantity > 1 ? ItemSlot.quantity.ToString() : "";
    }

    protected override void EnableSlotUI(bool enable)
    {
        base.EnableSlotUI(enable);
        itemQuantityText.enabled = enable;
    }
}

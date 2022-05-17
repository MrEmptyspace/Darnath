using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using MCEvents;

//[CreateAssetMenu(fileName = "New Inventory", menuName = "Items/Inventory")]
public class Inventory : MonoBehaviour
{
    //private readonly UnityEvent onInventoryItemsUpdated = null;
    //public GameEvent inventoryUpdated;

    public ItemContainer ItemContainer { get; } = new ItemContainer(5);
    [SerializeField] private Transform inventoryLocation;
    [SerializeField] private GameObject testItem1;
    [SerializeField] private GameObject testItem2;
    [SerializeField] private GameObject dropLocation;
    [SerializeField] private Hotbar hotbar;
    [SerializeField] private GameObject splitHolder;
    [SerializeField] private GameObject slotHolder;

    private InventoryItemDragHandler dragHandlerOfSplitSlot;
    private SplitDragHandler splitDragHandler;
    private SplitHandler splitHandler;
    private List<InventorySlot> inventorySlots;
    private ItemSlot splitSlotItemSlotRef;
    private int previousHotKeyIndex = -1;

    private int randomDebugInt = 0;

    private PointerEventData splitEventData;
    private void Start()
    {
        //Testing Inventory
        TestAddCauliflowerToInventory();
        TestAddCauliflowerToInventory();
        TestAddCauliflowerToInventory();

        //Split logic Init
        splitDragHandler = splitHolder.GetComponentInChildren<SplitDragHandler>();
        splitHandler = splitHolder.GetComponent<SplitHandler>();

        inventorySlots = new List<InventorySlot>();
        var temp = slotHolder.GetComponentsInChildren<InventorySlot>();
        foreach (var slot in temp)
        {
            inventorySlots.Add(slot);
        }

        //Setting the default Hotbar slot pressed to 1
        HotKeyPressed(0);
    }

    public void Update()
    {
        if(splitHolder.activeSelf == true)
        {
            splitDragHandler.transform.position = Input.mousePosition;
        }    
    }

    //this takes the items that is currently selected in the hot by index
    public void UseCurrentItem()
    {
        //If Hotkey was not set yet
        if(previousHotKeyIndex != -1)
        {
            HotbarSlot slotPressed = hotbar.hotbarSlots[previousHotKeyIndex];
            if (slotPressed.SlotIndex != -1)
            {
                DropItem(slotPressed);
            }
        }
    }

    //Assigns hotbar slot with the 1-9 + 0 hotkeys
    public void HotKeyPressed(int numberPressed)
    {
        //Remove active status from last hot bar now that new one is set
        if (previousHotKeyIndex != numberPressed && previousHotKeyIndex != -1)
        {
            HotbarSlot previousSlot = hotbar.hotbarSlots[previousHotKeyIndex];
            previousSlot.activeSlotText.text = "F";
            previousSlot.activeSlotText.ForceMeshUpdate(true);
        }
        //Set Active hotbar slot to active T
        HotbarSlot slotPressed = hotbar.hotbarSlots[numberPressed];
        slotPressed.activeSlotText.text = "T";
        slotPressed.activeSlotText.ForceMeshUpdate(true);
        previousHotKeyIndex = numberPressed;
    
    }

    public void Merge(int slotIndex)
    {
        ItemContainer.Merge(slotIndex, splitSlotItemSlotRef);

        if(dragHandlerOfSplitSlot != null)
        {
            dragHandlerOfSplitSlot.IsSplitting = false;
        }
        splitSlotItemSlotRef = new ItemSlot();

        //reset split holder stuff
        splitHolder.SetActive(false);
    }

    public void Split(int slotIndex , PointerEventData eventData)
    {
        int quantityBeforeSplit = ItemContainer.GetSlotByIndex(slotIndex).quantity;
        ItemSlot splitSlotForMouse = ItemContainer.Split(slotIndex);

        //If there is only one item and the item was not split
        if(splitSlotForMouse.items.Count == 1 && quantityBeforeSplit == splitSlotForMouse.quantity)
        {
            SplitHandler splitHandler = splitHolder.GetComponent<SplitHandler>();
            splitHandler.ItemSlot = splitSlotForMouse;
            splitHolder.SetActive(true);

            InventorySlot slotBeingSplit = inventorySlots[slotIndex];

            splitHandler.ItemSlot.item.Icon = slotBeingSplit.ItemSlot.item.Icon;
            splitHandler.SlotItem = slotBeingSplit.SlotItem;

            splitSlotItemSlotRef = splitHandler.ItemSlot;

            dragHandlerOfSplitSlot = slotBeingSplit.GetComponentInChildren<InventoryItemDragHandler>();
            splitHandler.UpdateSlotUI(null);
            ItemContainer.RemoveAt(slotIndex);

        }else if(splitSlotForMouse.quantity > 0)
        {
            SplitHandler splitHandler = splitHolder.GetComponent<SplitHandler>();
            splitHandler.ItemSlot = splitSlotForMouse;
            splitHolder.SetActive(true);

            InventorySlot slotBeingSplit = inventorySlots[slotIndex];

            splitHandler.ItemSlot.item.Icon = slotBeingSplit.ItemSlot.item.Icon;
            splitHandler.SlotItem = slotBeingSplit.SlotItem;

            splitSlotItemSlotRef = splitHandler.ItemSlot;

            dragHandlerOfSplitSlot = slotBeingSplit.GetComponentInChildren<InventoryItemDragHandler>();
            splitHandler.UpdateSlotUI(null);
        }   
    }

    //Called by Input system on pickup
    public void AddItem(InventoryItem item)
    {
        ItemSlot itemSlotWithItem = new ItemSlot(item, 1);
        itemSlotWithItem = ItemContainer.AddItem(itemSlotWithItem);       
        item.transform.position = inventoryLocation.position;
    }

    //Called by Input system on pickup
    public void AddItem(ItemSlot itemSlotWithItem)
    {
        itemSlotWithItem = ItemContainer.AddItem(itemSlotWithItem);
        itemSlotWithItem.item.gameObject.transform.position = inventoryLocation.position;
    }

    public void DropItem(InventorySlot thisSlot)
    {
        Vector3 dropPostion = CalcDropPosition();
        hotbar.Remove(thisSlot);

        float counter = 0;
        foreach (InventoryItem item in thisSlot.ItemSlot.items)
        {
            item.transform.position = dropPostion + new Vector3(0, counter, 0);
            counter += 0.5f;
        }
        ItemContainer.RemoveAt(thisSlot.SlotIndex);
    }

    public void DropItem(SplitHandler splitSlot)
    {
        Vector3 dropPostion = CalcDropPosition();

        float counter = 0;
        foreach (InventoryItem item in splitSlot.ItemSlot.items)
        {
            item.transform.position = dropPostion + new Vector3(0, counter, 0);
            counter += 0.5f;
        }

        //Remove items from split holder 
        splitSlot.ItemSlot = new ItemSlot();

        if (dragHandlerOfSplitSlot != null)
        {
            dragHandlerOfSplitSlot.IsSplitting = false;
        }
        splitSlotItemSlotRef = new ItemSlot();

        splitHolder.SetActive(false);
        EventManager.TriggerEvent(Events.onMouseEndHoverTooltip);
    }

    public void DropItem(HotbarSlot thisSlot)
    {
        Vector3 dropPostion = CalcDropPosition();
        float counter = 0;
        foreach (InventoryItem item in thisSlot.ItemSlot.items)
        {
            item.transform.position = dropPostion + new Vector3(0, counter, 0);
            counter += 0.5f;
        }

        ItemContainer.RemoveAt(thisSlot.SlotIndex);
        hotbar.Remove(thisSlot);

    }

    public Vector3 CalcDropPosition()
    {
        Vector3 dropLocationPos = dropLocation.transform.position;
        Vector3 dropLocationDirection = dropLocation.transform.forward;
        Quaternion dropLocationRotation = dropLocation.transform.rotation;
        float spawnDistance = 2;
        Vector3 dropPostion = dropLocationPos + dropLocationDirection * spawnDistance;

        return dropPostion;
    }

    [ContextMenu("Test Add Carrot")]
    public void TestAddCarrotToInventoy()
    {
        GameObject test1GameObject = Instantiate(testItem1, inventoryLocation);
        InventoryItem test1Item = test1GameObject.GetComponent<InventoryItem>();

        ItemSlot itemSlotWithItem = new ItemSlot(test1Item, 1);

        AddItem(itemSlotWithItem);
    }

    [ContextMenu("Test Add Cauliflower")]
    public void TestAddCauliflowerToInventory()
    {
        randomDebugInt += 5;
        Vector3 spawnPos = inventoryLocation.position + new Vector3(0, 0, randomDebugInt);
        GameObject test2GameObject = Instantiate(testItem2, spawnPos, Quaternion.identity);
        InventoryItem test2Item = test2GameObject.GetComponent<InventoryItem>();

        ItemSlot itemSlotWithItem = new ItemSlot(test2Item, 1);

        AddItem(itemSlotWithItem);
    }

}

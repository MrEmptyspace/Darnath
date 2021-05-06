using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//[CreateAssetMenu(fileName = "New Inventory", menuName = "Items/Inventory")]
public class Inventory : MonoBehaviour
{
    //private readonly UnityEvent onInventoryItemsUpdated = null;
    //public GameEvent inventoryUpdated;

    public ItemContainer ItemContainer { get; } = new ItemContainer(24);
    [SerializeField] private Transform inventoryLocation;
    [SerializeField] private GameObject testItem1;
    [SerializeField] private GameObject testItem2;
    [SerializeField] private GameObject dropLocation;
    [SerializeField] private Hotbar hotbar;

    //[SerializeField] private ItemSlot testItemSlot2 = new ItemSlot();
    public void OnEnabled()
    {
        //Custom event system https://www.youtube.com/watch?v=iXNwWpG7EhM

        //Trying to not use a custom event solution https://www.youtube.com/watch?v=LnAJ4HQGR7I
    }
    private void Start()
    {
        //EventManager.StartListening(Events.HotbarUpdated, UpdateSlotIndexes);
    }

    public void OnDisable()
    {
        //Custom event system https://www.youtube.com/watch?v=iXNwWpG7EhM
        //ItemContainer.OnItemsUpdated += onInventoryItemsUpdated.Raise();
       //ItemContainer.OnItemsUpdated -= inventoryUpdated;
        //Trying to not use a custom event solution https://www.youtube.com/watch?v=LnAJ4HQGR7I
    }

    public void DoSomething() {
        Debug.Log("This is what happens when the inventory is enabled and listening for ItemContainer.OnItemsUpdated");
    }

    [ContextMenu("Test Add 1")]
    public void TestAdd()
    {
        //TODO SETUP ITEMSLOT WITH REAL ITEM
        //ItemContainer.OnItemsUpdated = inventoryUpdated;

        GameObject test1GameObject = Instantiate(testItem1, inventoryLocation);
        InventoryItem test1Item = test1GameObject.GetComponent<InventoryItem>();

        ItemSlot itemSlotWithItem = new ItemSlot(test1Item, 1);

        AddItem(itemSlotWithItem);

        //Needs to be added to the hot bar 
    }

    [ContextMenu("Test Add 2")]
    public void TestAdd2()
    {
        GameObject test2GameObject = Instantiate(testItem2, inventoryLocation);
        InventoryItem test2Item = test2GameObject.GetComponent<InventoryItem>();

        ItemSlot itemSlotWithItem = new ItemSlot(test2Item, 1);

        AddItem(itemSlotWithItem);
    }

    //Called by Input system on pickup
    public void AddItem(InventoryItem item)
    {
        ItemSlot itemSlotWithItem = new ItemSlot(item, 1);
        itemSlotWithItem = ItemContainer.AddItem(itemSlotWithItem);

        int slotIndex = ItemContainer.GetIndexBySlot(itemSlotWithItem);
        item.transform.position = inventoryLocation.position;

        //HotbarItem d = new HotbarItem();
        hotbar.Add(itemSlotWithItem, slotIndex);
    }

    //Called by Input system on pickup
    public void AddItem(ItemSlot itemSlotWithItem)
    {
        itemSlotWithItem = ItemContainer.AddItem(itemSlotWithItem);
        itemSlotWithItem.item.gameObject.transform.position = inventoryLocation.position;

        //Somehow find out where that item has gon

        int slotIndex = ItemContainer.GetIndexBySlot(itemSlotWithItem);


        hotbar.Add(itemSlotWithItem, slotIndex);

    }



    //Maybe replace with item dropper
    public void DropItem(InventorySlot thisSlot)
    {
        Vector3 dropLocationPos = dropLocation.transform.position;
        Vector3 dropLocationDirection = dropLocation.transform.forward;
        Quaternion dropLocationRotation = dropLocation.transform.rotation;
        float spawnDistance = 2;  
        Vector3 dropPostion = dropLocationPos + dropLocationDirection * spawnDistance;

        thisSlot.ItemSlot.item.transform.position = dropPostion;
        //thisSlot.ItemSlot.items.transform.position = dropLocation.position;  
        hotbar.Remove(thisSlot);

        //foreach (InventoryItem item in thisSlot.ItemSlot.items)
        //{
        //    item.transform.position = dropPostion;
        //}
    }

    //Maybe replace with item dropper
    public void DropItem(HotbarSlot thisSlot)
    {
        Vector3 dropLocationPos = dropLocation.transform.position;
        Vector3 dropLocationDirection = dropLocation.transform.forward;
        Quaternion dropLocationRotation = dropLocation.transform.rotation;
        float spawnDistance = 2;
        Vector3 dropPostion = dropLocationPos + dropLocationDirection * spawnDistance;

        //Get Item by slotindex
        Debug.Log("Trying to drop items with inventory Index " + thisSlot.name);
        //ItemSlot _itemSlot = ItemContainer.GetSlotByIndex(thisSlot.ReferenceSlotIndex);
        //ItemContainer.RemoveItem(thisSlot.SlotItem);
        hotbar.Remove(thisSlot);

        //_itemSlot.item.transform.position = dropPostion;
        //thisSlot.Slot.items.transform.position = dropLocation.position;  

        //foreach (InventoryItem item in _itemSlot.items)
        //{
        //    item.transform.position = dropPostion;
        //}
    }

    //public void UpdateSlotIndexes(Dictionary<string, object> eventParams)
    //{
    //    int indexOne = (int) eventParams["firstSlotIndex"];
    //    int indexTwo = (int) eventParams["secondSlotIndex"];
    //    bool merge = false;

    //    if(eventParams.ContainsKey("merge"))
    //    {
    //        merge = true;
    //    }

    //    //If you are merging you only need to check for the first slot
    //    foreach (HotbarSlot hotbarSlot in hotbar.hotbarSlots)
    //    {
    //        if (merge)
    //        {
    //            if (hotbarSlot.ReferenceSlotIndex == indexOne)
    //            {
    //                hotbarSlot.ReferenceSlotIndex = indexTwo;
    //            }
    //        }
    //        else
    //        {
    //            //move hotbarSlot index to equal indexTwo
    //            if(hotbarSlot.ReferenceSlotIndex == indexOne)
    //            {
    //                hotbarSlot.ReferenceSlotIndex = indexTwo;
    //            }
    //            else if (hotbarSlot.ReferenceSlotIndex == indexTwo)
    //            {
    //                hotbarSlot.ReferenceSlotIndex = indexOne;
    //            }
    //        }

    //    }

    //}

}

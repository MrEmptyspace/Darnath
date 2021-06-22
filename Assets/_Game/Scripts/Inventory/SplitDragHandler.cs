using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SplitDragHandler : ItemDragHandler
{
    private Inventory inventory;
    [SerializeField] GraphicRaycaster gr;
    [SerializeField] Inventory inventoryRef;
    public override void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        //canvasGroup.blocksRaycasts = false;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left || eventData.button == PointerEventData.InputButton.Right)
        {
            //Code to be place in a MonoBehaviour with a GraphicRaycaster component
            //GraphicRaycaster gr = this.GetComponent<GraphicRaycaster>();
            //Create the PointerEventData with null for the EventSystem
            PointerEventData ped = new PointerEventData(null);
            //Set required parameters, in this case, mouse position
            ped.position = Input.mousePosition;
            //Create list to receive all results
            List<RaycastResult> results = new List<RaycastResult>();
            //Raycast it
            gr.Raycast(ped, results);

            foreach (RaycastResult uiHit in results)
            {
                if(uiHit.gameObject.name == "InventorySlot")
                {
                    InventorySlot slot = uiHit.gameObject.GetComponent<InventorySlot>();
                    Inventory inventory = slot.Inventory;
                    inventory.Merge(slot.SlotIndex);
                }
            }
            if(results.Count == 1)
            {

                inventoryRef.DropItem(this.GetComponentInParent<SplitHandler>());
            }
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left || eventData.button == PointerEventData.InputButton.Right)
        {
            //transform.SetParent(originalParent);
            // transform.localPosition = Vector3.zero;
            //canvasGroup.blocksRaycasts = true;
        }

        //if (eventData.hovered.Count == 0 && eventData.button == PointerEventData.InputButton.Left)
        //{
        //    if (ItemSlotUI != null)
        //    {
        //        if (inventory != null)
        //        {

        //        }
        //        else
        //        {
        //            InventorySlot thisSlot = ItemSlotUI as InventorySlot;
        //            inventory = thisSlot.Inventory;
        //        }
        //    }
        //}
    }
}

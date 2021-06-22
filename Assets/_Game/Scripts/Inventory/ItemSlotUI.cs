using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class ItemSlotUI : MonoBehaviour, IDropHandler
{
    [SerializeField] public Image itemIconImage = null;

    public int SlotIndex { get; set; }

    public abstract HotbarItem SlotItem{get; set;}

    //private void OnEnable() => UpdateSlotUI(null);

    protected virtual void Start()
    {
        SlotIndex = transform.GetSiblingIndex();
    }

    public abstract void OnDrop(PointerEventData eventData);

   //public abstract void UpdateSlotUI();
    public abstract void UpdateSlotUI(Dictionary<string, object> message);

    protected virtual void EnableSlotUI(bool enable)
    {
        itemIconImage.enabled = enable;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IPointerDownHandler, IDragHandler,IPointerUpHandler,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] protected ItemSlotUI itemSlotUI = null;

    private CanvasGroup canvasGroup = null;
    private Transform originalParent = null;
    private bool isHovering = false;

    public ItemSlotUI ItemSlotUI => itemSlotUI;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnDisable()
    {
        if (isHovering)
        {
            //INVOKE EVENT
            EventManager.TriggerEvent(EventManager.Events.onMouseStartHoverTooltip);
            isHovering = false;
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            //INVOKE EVENT
            EventManager.TriggerEvent(EventManager.Events.onMouseEndHoverTooltip);
            originalParent = transform.parent;

            transform.SetParent(transform.parent.parent);

            canvasGroup.blocksRaycasts = false;
        }
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            transform.position = Input.mousePosition;
        }
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            transform.SetParent(originalParent);
            transform.localPosition = Vector3.zero;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Dictionary<string, object> message = new Dictionary<string, object>{{ "infoItem", ItemSlotUI.SlotItem }};
        EventManager.TriggerEvent(EventManager.Events.onMouseStartHoverTooltip, message);
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Dictionary<string, object> message = new Dictionary<string, object> { { "infoItem", ItemSlotUI.SlotItem } };
        EventManager.TriggerEvent(EventManager.Events.onMouseEndHoverTooltip, message);

        isHovering = false;
    }
}

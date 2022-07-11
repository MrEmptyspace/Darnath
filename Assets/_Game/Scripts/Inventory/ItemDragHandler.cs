using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using MCEvents;

public class ItemDragHandler : MonoBehaviour, IPointerDownHandler, IDragHandler,IPointerUpHandler,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] protected ItemSlotUI itemSlotUI = null;

    public CanvasGroup canvasGroup = null;
    public Transform originalParent = null;
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
            //EventManagerV2.TriggerEvent(Events.onMouseStartHoverTooltip);
            isHovering = false;
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            //EventManagerV2.TriggerEvent(Events.onMouseEndHoverTooltip);
            originalParent = transform.parent;

            transform.SetParent(transform.parent.parent);

            canvasGroup.blocksRaycasts = false;
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

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            transform.position = Input.mousePosition;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Dictionary<string, object> message = new Dictionary<string, object>{{ "infoItem", ItemSlotUI.SlotItem }};
        //EventManagerV2.TriggerEvent(Events.onMouseStartHoverTooltip, message);
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Dictionary<string, object> message = new Dictionary<string, object> { { "infoItem", ItemSlotUI.SlotItem } };
        //EventManagerV2.TriggerEvent(Events.onMouseEndHoverTooltip, message);

        isHovering = false;
    }
}

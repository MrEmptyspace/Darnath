using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MCEvents;

public class HoverInfoPopup : MonoBehaviour
{
    [SerializeField] private GameObject popupCanvasObject = null;
    [SerializeField] private RectTransform popupObject = null;
    [SerializeField] private TextMeshProUGUI infoText = null;
    [SerializeField] private Vector3 offset = new Vector3(0f,50f,0f);
    [SerializeField] private float padding = 25f;

    private Canvas popupCanvas = null;

    private void Start()
    {
        popupCanvas = popupCanvasObject.GetComponent<Canvas>();
        //EventManagertartListening(Events.onMouseStartHoverTooltip, DisplayInfo);
        //EventManagertartListening(Events.onMouseEndHoverTooltip, HideInfo);
    }

    private void Update()
    {
        FollowCursor();
    }

    private void FollowCursor()
    {
        if (!popupCanvasObject.activeSelf){ return; }

        Vector3 newPos = Input.mousePosition + offset;
        newPos.z = 0f;

        //HandlePadding.

        float rightEdgeToScreenEdgeDistance = Screen.width - (newPos.x + popupObject.rect.width * popupCanvas.scaleFactor / 2) - padding;
        if(rightEdgeToScreenEdgeDistance < 0)
        {
            newPos.x += rightEdgeToScreenEdgeDistance;
        }

        float leftEdgeToScreenEdgeDistance = 0 - (newPos.x + popupObject.rect.width * popupCanvas.scaleFactor / 2) + padding;
        if (leftEdgeToScreenEdgeDistance > 0)
        {
            newPos.x += leftEdgeToScreenEdgeDistance;
        }

        float topEdgeToScreenEdgeDistance = Screen.height - (newPos.y + popupObject.rect.height * popupCanvas.scaleFactor ) - padding;
        if (topEdgeToScreenEdgeDistance < 0)
        {
            newPos.y += topEdgeToScreenEdgeDistance;
        }

        popupObject.transform.position = newPos;
    }

    public void DisplayInfo(Dictionary<string, object> message = null)
    {
        if(message == null) { return; }
        if (!message.ContainsKey("infoItem") || (HotbarItem)message["infoItem"] == null)
        {
            return;
        }
        HotbarItem infoItem = (HotbarItem)message["infoItem"];

        StringBuilder builder = new StringBuilder();

        builder.Append("<size=35>").Append(infoItem.ColouredName).Append("</size>\n");
        builder.Append(infoItem.GetInfoDisplayText());

        infoText.text = builder.ToString();

        popupCanvasObject.SetActive(true);

        LayoutRebuilder.ForceRebuildLayoutImmediate(popupObject);
    }

    public void HideInfo(Dictionary<string, object> message = null) => popupCanvasObject.SetActive(false);

}

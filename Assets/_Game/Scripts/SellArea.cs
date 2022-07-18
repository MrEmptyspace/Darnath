using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MCEvents;

public class SellArea : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider collider)
    {
        //transform.parent.GetComponent<ParentScript>().CollisionDetected(this);
        if (collider.gameObject.TryGetComponent(out InventoryItem itemToBeSold))
        {
            EventManager.TriggerEvent(MCEventTag.SellItem,itemToBeSold.SellPrice);
        }

        //Need to do any cleanup an delete the crop
    }
}

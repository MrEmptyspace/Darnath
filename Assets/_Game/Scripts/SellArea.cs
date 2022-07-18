using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MCEvents;

public class SellArea : MonoBehaviour
{
    public List<GameObject> sellablesInArea { get; set; } = new List<GameObject>();

    void OnTriggerEnter(Collider collider)
    {
        // //transform.parent.GetComponent<ParentScript>().CollisionDetected(this);
        // if (collider.gameObject.TryGetComponent(out InventoryItem itemToBeSold))
        // {
        //     EventManager.TriggerEvent(MCEventTag.SellItem, itemToBeSold.SellPrice);

        // }

        // if (collider.gameObject.TryGetComponentInChildren(out ISellable itemToBeSold))
        // {
        //     sellablesInArea.Add(itemToBeSold);
        // }

        var tempMonoArray = collider.gameObject.GetComponents<MonoBehaviour>();
 
        foreach (var monoBehaviour in tempMonoArray)
        {
          var tempSellable = monoBehaviour as ISellable;
 
            if (tempSellable != null)
            {
                sellablesInArea.Add(monoBehaviour.gameObject);
            }
        }  

        //Need to do any cleanup an delete the crop
    }

    public void ResetSellArea(){
        sellablesInArea.Clear();
    }

}

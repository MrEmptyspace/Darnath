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
        Debug.Log("On Table collider name = " +collider.name);

        var tempMonoArray = collider.gameObject.GetComponents<MonoBehaviour>();
 
        foreach (var monoBehaviour in tempMonoArray)
        {
          var tempSellable = monoBehaviour as ISellable;
 
            if (tempSellable != null)
            {
                Debug.Log("Adding monoBehaviour.gameObject to list = " + monoBehaviour.gameObject.name);
                sellablesInArea.Add(monoBehaviour.gameObject);
            }
        }  

        //Need to do any cleanup an delete the crop
    }

    public void ResetSellArea(){

        for (int i = 0; i < sellablesInArea.Count; i++)
        {
            Destroy(sellablesInArea[i].gameObject);
        }
        sellablesInArea.Clear();
    }

}

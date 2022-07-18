using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour , ISellable
{
    [SerializeField]
    public int sellPrice;

    [SerializeField]
    public string itemName;

    public float GetBasePrice(){
        return sellPrice;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class InventoryItem : HotbarItem
{
    public string itemName;
    public string itemDescription;
    public Dictionary<string, string> itemProps = new Dictionary<string, string>();
    [SerializeField] [Min(0)] private int sellPrice = 1;
    [SerializeField] [Min(1)] private int maxStack = 1;
    public int SellPrice { get => sellPrice; }
    public int MaxStack { get => maxStack; }
   
    void Start()
    {
        //Change item tag to Item to detect when we look at it
        gameObject.tag = "Item";
    }

    public override string ColouredName
    {
        get
        {
            return "<color=green>" + Name + "</color>";
        }
    }


    public override string GetInfoDisplayText()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("<color=white><size=18>").Append("Description: ").Append(" </size></color>\n");
        builder.Append("<color=white><size=16>").Append(itemDescription).Append("</size></color>\n");

        builder.Append("<color=blue><size=18>").Append("Sell Price: ").Append(" </size></color>");
        builder.Append("<color=blue><size=18>").Append(sellPrice).Append("</size></color>\n");

        return builder.ToString();
    }


    public void PickItem(Transform transform)
    {
        ///Destroy(gameObject);
        this.transform.position = transform.position;
    }

    public void Init(string name)
    {
        //Inistiate base on an object

        base.Name = name;
        //Icon = Utils.LoadTexture(name, "png","Sprites/");
        this.itemName = name;
        //this.value = valueSADASD;
        //LOAD ITEM DATA
        ////TAKE THE NAME OF THE ITEM WITHOUT THE NUMBER SUFFIX AND CALL THE ITEM JSON and convert it to an item
        //itemPreview = Utils.LoadTexture(name, "png");
        //value = (int) Utils.LoadItemData(name);
        Debug.Log("Need to implement creating image for inventory");
    }

    public override bool Equals(object obj)
    {
        var item = obj as InventoryItem;

        if (item == null) return false;
        if (itemName != item.itemName) return false;
        if (itemDescription != item.itemDescription) return false;
        //if (EqualityComparer<Dictionary<string, string>>.Default.Equals(itemProps, item.itemProps)) return false;
        if (sellPrice != item.sellPrice) return false;
        if (maxStack != item.maxStack) return false;
        if (SellPrice != item.SellPrice) return false;
        if (MaxStack != item.MaxStack) return false;
        if (ColouredName != item.ColouredName) return false;
        ////base.Equals(obj) &&

        return true;
    }
}

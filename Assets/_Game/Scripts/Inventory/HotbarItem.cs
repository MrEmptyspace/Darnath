using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HotbarItem : MonoBehaviour
{
    [Header("Basic Info")]
    [SerializeField] private new string name = "New hotbar item name";
    [SerializeField] private Sprite icon = null;

    //public string Name => name;
    public abstract string ColouredName { get; }
    public Sprite Icon { get => icon; set => icon = value; }
    public string Name { get => name; set => name = value; }

    //public Sprite Icon => icon;

    public abstract string GetInfoDisplayText();

}

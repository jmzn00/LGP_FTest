using System.Collections.Generic;
using UnityEngine;

public enum ItemAction { Inspect, Equip, Use, Drop}

[CreateAssetMenu(menuName = "Inventory/Inventory Item", fileName = "NewInventoryItem")]
public class InventoryItem : ScriptableObject
{
    public string name;
    public string description;
    public Sprite sprite;

    public bool inspectable;
    public bool equippable;
    public bool usable;
    public bool droppable;

    public IEnumerable<ItemAction> GetActions() 
    {
        if(inspectable) yield return ItemAction.Inspect;
        if(equippable) yield return ItemAction.Equip;
        if(usable) yield return ItemAction.Use;
        if(droppable) yield return ItemAction.Drop;
    }
}

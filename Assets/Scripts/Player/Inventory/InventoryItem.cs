using System.Collections.Generic;
using UnityEngine;

public enum ItemAction { Inspect, Equip, Use, Drop}

[CreateAssetMenu(menuName = "Inventory/Inventory Item", fileName = "NewInventoryItem")]
public class InventoryItem : ScriptableObject
{
    [Header("Presentation")]
    public string displayName;
    public string description;
    public bool displayDescriptionInUI = true;
    public Sprite sprite;

    [Header("World")]
    public GameObject worldPrefab;

    [Header("Stacking")]
    [Min(1)] public int maxStack = 1;
    public bool Stackable => maxStack > 1;

    [Header("Actions (menu)")]
    [SerializeField] private List<ItemAction> _actions = new List<ItemAction>();

    [Header("Optional per-action overrides")]
    public ItemActionSO overrideInspect;
    public ItemActionSO overrideEquip;
    public ItemActionSO overrideUse;
    public ItemActionSO overrideDrop;

    public ItemActionSO GetOverride(ItemAction kind) => kind switch
    {
        ItemAction.Inspect => overrideInspect,
        ItemAction.Equip => overrideEquip,
        ItemAction.Use => overrideUse,
        ItemAction.Drop => overrideDrop,
        _ => null
    };

    private void OnValidate()
    {
        if (maxStack < 1) maxStack = 1;        
    }
    public IEnumerable<ItemAction> Actions => _actions;
}

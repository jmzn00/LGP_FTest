using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DB/Item Database")]
public class ItemDatabase : ScriptableObject
{
    [SerializeField] private InventoryItem[] inventoryItems;
    private Dictionary<string, InventoryItem> byName;

    private void OnEnable() => Rebuild();

    
    private void Rebuild() 
    {
        byName = new Dictionary<string, InventoryItem>(StringComparer.OrdinalIgnoreCase);
        foreach (var a in inventoryItems)
        {
            var key = a.displayName;
            if (!string.IsNullOrEmpty(key))
                byName[key.ToLower()] = a;
        }
    }
    public InventoryItem GetItemByName(string name) 
    {
        if (string.IsNullOrEmpty(name) || byName == null) return null;
        return byName.TryGetValue(name.ToLower(), out var def) ? def : null;
    }
}

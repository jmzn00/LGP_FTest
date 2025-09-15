using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DB/Armor Database")]
public class ArmorDatabase : ScriptableObject
{
    [SerializeField] private ArmorDefinition[] allArmor;
    private Dictionary<string, ArmorDefinition> byName;

    private void OnEnable() => Rebuild();

    private void Rebuild()
    {
        byName = new Dictionary<string, ArmorDefinition>(StringComparer.OrdinalIgnoreCase);
        foreach (var a in allArmor)
        {
            var key = a.rule.displayName;
            if (!string.IsNullOrEmpty(key))
                byName[key] = a;
        }
    }

    public ArmorDefinition GetArmorByName(string name)
    {
        if (string.IsNullOrEmpty(name) || byName == null) return null;
        return byName.TryGetValue(name, out var def) ? def : null;
    }
}

using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Combat/Armor/ Armor Definition")]
public class ArmorDefinition : ScriptableObject
{
    [System.Serializable]
    public struct Rule 
    {
        public string displayName;

        public ArmorSlot slot;
        public float damageMultiplier;
        public float flatReduction;
        public bool immuneToMelee;
        public bool immuneToAll;

        public bool destroyable;
        public float armorHealth;

        public string vfxTagOverride;
        public string sfxTagOverride;

        public Color tempColor;
    }

    [Header("Default Rule")]
    public Rule rule;
}

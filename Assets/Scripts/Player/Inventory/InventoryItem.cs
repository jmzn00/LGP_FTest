using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Inventory Item", fileName = "NewInventoryItem")]
public class InventoryItem : ScriptableObject
{
    public string name;
    public string description;
    public Sprite sprite;
    public GameObject visuals;
}

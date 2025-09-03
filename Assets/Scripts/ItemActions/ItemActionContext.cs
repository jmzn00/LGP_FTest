
using UnityEngine;

public struct ItemActionContext
{
    public GameObject user;
    public Vector3 worldPos;
    public PlayerInventory inventory;
    public int slotIndex;
    public InventoryItem itemDef;
}

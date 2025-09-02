using UnityEngine;

public class I_Medkit : Interactable
{
    [SerializeField] private InventoryItem _item;
    private void Awake()
    {
        _item.visuals = gameObject;
    }

    public override void Interact(PlayerInteraction interaction)
    {
        if(interaction != null) 
        {
            PlayerInventory inventory = interaction.gameObject.GetComponent<PlayerInventory>();
            if(inventory != null) 
            {
                inventory.AddInventoryItem(_item);
            }
        }
    }
}

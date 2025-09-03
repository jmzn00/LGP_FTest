using UnityEngine;

public class I_Key : Interactable
{
    [SerializeField] private InventoryItem _item;

    public override void Interact(PlayerInteraction interaction)
    {
        if(interaction != null) 
        {
            PlayerInventory inventory = interaction.gameObject.GetComponent<PlayerInventory>();
            if(inventory != null) 
            {
                if (inventory.TryAdd(_item))
                    Destroy(gameObject); // TEMP ADD POOLING
            }
        }
    }
    public override void Use()
    {
        base.Use();
    }
}

using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class I_Backpack : Interactable
{
    [SerializeField] private int slots = 5;
    public override void Interact(PlayerInteraction interaction)
    {
        PlayerInventory inventory = interaction.gameObject.GetComponent<PlayerInventory>();
        if (inventory != null)
        {
            inventory.AddInventorySlots(slots);
            Destroy(this.gameObject);
        }
    }
}

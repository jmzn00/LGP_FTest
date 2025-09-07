using TMPro;
using UnityEngine;

public class I_Note : Interactable
{
    [SerializeField] private InventoryItem _item;
    [SerializeField] private TMP_Text noteText;
    [SerializeField] private string noteString;

    private void Start()
    {
        Debug.Log($"Note String: {noteString}");
        noteText.text = _item.description;
    }
    public override void Interact(PlayerInteraction interaction)
    {
        if (interaction != null)
        {
            PlayerInventory inventory = interaction.gameObject.GetComponent<PlayerInventory>();
            if (inventory != null)
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

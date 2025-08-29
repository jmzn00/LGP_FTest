using UnityEngine;

public class InteractableCubePickup : Interactable
{
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public override void Interact(PlayerInteraction interaction)
    {
        if(interaction == null) 
        {
            rb.isKinematic = false;
            transform.SetParent(null);
        }
        else 
        {
            rb.isKinematic = true;
            transform.SetParent(interaction.InteractionPivot);
        }
        
    }
}

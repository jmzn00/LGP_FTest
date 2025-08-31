using UnityEngine;

public class InteractableCubePickup : Interactable
{
    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (InteractableDatabse.Instance != null)
        {
            InteractableDatabse.Instance.AddInteractable(this);
        }
        else
        {
            Debug.LogWarning("InteractableDatabase is NULL");
        }
    }
    private void OnDestroy()
    {
        if (InteractableDatabse.Instance != null)
        {
            InteractableDatabse.Instance.RemoveInteractable(this);
        }
        else
        {
            Debug.LogWarning("InteractableDatabase is NULL");
        }
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

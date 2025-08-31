using Unity.VisualScripting;
using UnityEngine;

public class InteractableCube : Interactable
{
    private InspectableObject inspectableObj;
    private void Start()
    {
        if (InteractableDatabse.Instance != null)
        {
            InteractableDatabse.Instance.AddInteractable(this);
        }
        else
        {
            Debug.LogWarning("InteractableDatabase is NULL");
        }
        inspectableObj = GetComponent<InspectableObject>();
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
        interaction.SetInspecting(inspectableObj);
    }
}

using Unity.VisualScripting;
using UnityEngine;

public class InteractableCube : Interactable
{
    private InspectableObject inspectableObj;
    private void Awake()
    {
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

    private void Start()
    {
        inspectableObj = GetComponent<InspectableObject>();    
    }
    public override void Interact(PlayerInteraction interaction)
    {
        interaction.SetInspecting(inspectableObj);
    }
}

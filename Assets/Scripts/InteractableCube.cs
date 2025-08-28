using Unity.VisualScripting;
using UnityEngine;

public class InteractableCube : Interactable
{
    private InspectableObject inspectableObj;

    private void Start()
    {
        inspectableObj = GetComponent<InspectableObject>();    
    }
    public override void Interact(PlayerInteraction interaction)
    {
        interaction.SetInspecting(inspectableObj);
    }
}

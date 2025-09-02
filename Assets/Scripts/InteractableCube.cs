public class InteractableCube : Interactable
{
    private InspectableObject inspectableObj;   
    public override void Interact(PlayerInteraction interaction)
    {
        interaction.SetInspecting(inspectableObj);
    }
}

using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Actions/Default Inspect")]
public class InspectDefaultAction : ItemActionSO
{
    public override ItemAction Kind => ItemAction.Inspect;

    public override bool CanExecute(in ItemActionContext ctx)
    {
        return ctx.itemDef.worldPrefab != null;
    }
    public override void Execute(in ItemActionContext ctx)
    {
        InspectUI inspectUI = ctx.inventory.gameObject.GetComponent<InspectUI>();
        if (inspectUI) 
        {
            inspectUI.Inspect(ctx.itemDef);
        }
    }

}

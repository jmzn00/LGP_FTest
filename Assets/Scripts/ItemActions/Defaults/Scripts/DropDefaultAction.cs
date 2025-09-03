using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Actions/Default Drop")]
public class DropDefaultAction : ItemActionSO
{
    public override ItemAction Kind => ItemAction.Drop;

    public override bool CanExecute(in ItemActionContext ctx)
        => ctx.itemDef.worldPrefab != null;

    public override void Execute(in ItemActionContext ctx) 
    {
        if(ctx.itemDef.worldPrefab)
            Object.Instantiate(ctx.itemDef.worldPrefab, ctx.worldPos + Vector3.up, Quaternion.identity);

        ctx.inventory.RemoveAt(ctx.slotIndex);
    }
}

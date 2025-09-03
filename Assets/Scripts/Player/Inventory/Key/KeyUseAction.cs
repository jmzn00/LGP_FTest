using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Actions/KeyUseAction")]
public class KeyUseAction : ItemActionSO
{
    [SerializeField] private int keyId;
    public override ItemAction Kind => ItemAction.Use;
    public override bool CanExecute(in ItemActionContext ctx)
    {
        return base.CanExecute(ctx);
    }
    public override void Execute(in ItemActionContext ctx)
    {
        LockInteraction lockInteraction = ctx.inventory.gameObject.GetComponent<LockInteraction>();
        if (lockInteraction) 
        {
            if (lockInteraction.TryOpenLock(keyId)) 
            {
                ctx.inventory.RemoveAt(ctx.slotIndex);
            }
        }   

    }
}

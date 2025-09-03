using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Actions/Default Use")]
public class UseDefaultAction : ItemActionSO
{
    public override ItemAction Kind => ItemAction.Use;

    public override bool CanExecute(in ItemActionContext ctx)
    {
        return true;
    }

    public override void Execute(in ItemActionContext ctx)
    {
        throw new System.NotImplementedException();
    }
}

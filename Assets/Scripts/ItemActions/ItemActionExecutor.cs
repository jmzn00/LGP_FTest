using UnityEngine;

public class ItemActionExecutor : MonoBehaviour
{
    [SerializeField] private ActionDefaultsSO defaults;

    public void Execute(ItemAction action, in ItemActionContext ctx) 
    {
        var so = ctx.itemDef.GetOverride(action) ?? defaults.GetDefault(action);
        if(so == null) { Debug.LogWarning($"No action asset for {action}"); return; }
        if(!so.CanExecute(ctx)) { Debug.LogWarning($"Cannot execute {action}"); return; }
        so.Execute(ctx);
    }
}

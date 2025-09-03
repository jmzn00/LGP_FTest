using UnityEngine;

public abstract class ItemActionSO : ScriptableObject
{
    public abstract ItemAction Kind { get; }
    public virtual bool CanExecute(in ItemActionContext ctx) => true;
    public abstract void Execute(in ItemActionContext ctx);
}

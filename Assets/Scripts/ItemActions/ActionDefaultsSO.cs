using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Action Defaults")]
public class ActionDefaultsSO : ScriptableObject
{
    public ItemActionSO defaultInspect;
    public ItemActionSO defaultEquip;
    public ItemActionSO defaultUse;
    public ItemActionSO defaultDrop;

    public ItemActionSO GetDefault(ItemAction kind) => kind switch
    {
        ItemAction.Inspect => defaultInspect,
        ItemAction.Equip => defaultEquip,
        ItemAction.Use => defaultUse,
        ItemAction.Drop => defaultDrop,
        _ => null
    };    
}

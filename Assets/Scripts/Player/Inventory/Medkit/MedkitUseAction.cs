using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Actions/Medkit Use")]
public class MedkitUseAction : ItemActionSO
{
    public override ItemAction Kind => ItemAction.Use;
    [SerializeField] private int _healAmount = 25;

    private PlayerHealth playerHealth;
    public override bool CanExecute(in ItemActionContext ctx)
    {
        playerHealth = ctx.inventory.gameObject.GetComponent<PlayerHealth>();
        if(playerHealth)
            return true;

        Debug.LogError("PlayerHealth component not found on player.");
        return false;
    }

    public override void Execute(in ItemActionContext ctx)
    {
        if (playerHealth) 
        {
            if(playerHealth.CanHeal(_healAmount))
                ctx.inventory.RemoveAt(ctx.slotIndex);
        }
        
    }
}

using UnityEngine;

public class I_Lock : Interactable
{
    [SerializeField] private int lockId;
    [SerializeField] private Activatable activatable;
    public override void Interact(PlayerInteraction interaction)
    {
        LockInteraction lockInteraction = interaction.gameObject.GetComponent<LockInteraction>();
        if (lockInteraction) 
        {
            lockInteraction.FocusLock(this, lockId);
        }
        else
            Debug.LogError("No LockInteraction component found on player");
    }
    public override void Use()
    {
        Debug.Log($"Lock {lockId} was opened" );
        activatable?.Activate();
        gameObject.SetActive(false);
    }
}

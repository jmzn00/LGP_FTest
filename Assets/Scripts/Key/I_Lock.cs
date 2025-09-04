using UnityEngine;

public class I_Lock : Interactable
{
    [SerializeField] private int lockId;
    [SerializeField] private Activatable activatable;
    [SerializeField] private Animator lockAnimator;
    private PlayerInteraction _interaction;
   
    public override void Interact(PlayerInteraction interaction)
    {
        _interaction = interaction;
        interaction.gameObject.GetComponent<LockInteraction>()?.FocusLock(this, lockId);
    }
    public override void Use()
    {
        if (!_interaction)
        {
            Debug.LogError("No PlayerInteraction component found on player");
            return;
        }
        LockInteraction lockInteraction = _interaction.gameObject.GetComponent<LockInteraction>();
        CameraController cameraController = _interaction.gameObject.GetComponent<CameraController>();
        if (lockInteraction && cameraController)
        {
            lockInteraction.FocusLock(this, lockId);
        }
        else
        {
            Debug.LogError("No LockInteraction || CameraController component found on player");
            return;
        }
        
    }

    public override void PlayAnimation(InteractableAnimationType type)
    {
        switch (type)
        {
            case InteractableAnimationType.Unlock:
                lockAnimator.SetTrigger("OpenLock");
                break;
            case InteractableAnimationType.Lock:

                break;
        }
    }
    public override void Used()
    {
        activatable?.Activate();
        gameObject.SetActive(false);
    }
}

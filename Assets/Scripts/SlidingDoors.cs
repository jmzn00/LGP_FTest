using UnityEngine;

public class SlidingDoors : Activatable
{
    [SerializeField] private Animator animator;
    [SerializeField] private Activatable activateOnActivate;
    private void Awake()
    {
        Deactivate();
    }
    public override void SetStatus(bool value, GameObject activator)
    {
        base.SetStatus(value, activator);
    }
    public override void Activate()
    {
        
    }
    public override void Deactivate()
    {
        
    }
    
    private void Update()
    {
        
    }
}

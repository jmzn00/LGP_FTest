using UnityEngine;

public class SlidingDoors : Activatable
{
    [SerializeField] private Animator animator;
    private void Awake()
    {
        //animator = GetComponent<Animator>();
    }
    public override void SetStatus(bool value, GameObject activator)
    {
        base.SetStatus(value, activator);
    }
    public override void Activate()
    {
        animator.Play("Door_Open", 0, 0);
    }
    public override void Deactivate()
    {
        animator.Play("Door_Close", 0, 0);
    }
}

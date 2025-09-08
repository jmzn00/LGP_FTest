using System.Data.Common;
using UnityEngine;

public class A_Door : Activatable
{

    [SerializeField] private Transform doorPivot;
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openSpeed = 1f;
    private DoorState currentState;

    private void Awake()
    {
        ChangeState(DoorState.Closed);
    }
    public override void Activate()
    {
        base.Activate();
        ChangeState(DoorState.Unlocked);
    }
    public override void Deactivate()
    {
        base.Deactivate();
        ChangeState(DoorState.Locked);

    }

    private void Update()
    {
        if (currentState == DoorState.Closed || currentState == DoorState.Open) return;

        switch (currentState) 
        {
            case DoorState.Locked:
                OnDoorStateLocked();
                break;
            case DoorState.Unlocked:
                OnDoorStateUnlocked();
                break;
            case DoorState.Open:
                
                break;
            case DoorState.Closed:

                break;
        }
    }

    private void OnDoorStateUnlocked() 
    {
        RotateTo(openAngle);
    }
    private void OnDoorStateLocked() 
    {
        RotateTo(0f);
    }

    private void RotateTo(float y) 
    {
        Quaternion targetRot = Quaternion.Euler(0f, y, 0f);
        Quaternion startRot = doorPivot.localRotation;
        float totalAngle = Quaternion.Angle(startRot, targetRot);
        float degPerSec = (openSpeed <= 0f) ? totalAngle : totalAngle / openSpeed;

        doorPivot.localRotation = Quaternion.RotateTowards(doorPivot.localRotation, targetRot, degPerSec * Time.deltaTime);
        if (Quaternion.Angle(doorPivot.localRotation, targetRot) <= 0.01f)
        {
            if(currentState == DoorState.Closed)
                ChangeState(DoorState.Open);
            else if(currentState == DoorState.Open)
                ChangeState(DoorState.Closed);
        }
    }
    private void ChangeState(DoorState state)
    {
        switch (state) 
        {
            case DoorState.Locked:
                currentState = DoorState.Locked;
                break;
            case DoorState.Unlocked:
                currentState = DoorState.Unlocked;
                break;
        }
    }
}


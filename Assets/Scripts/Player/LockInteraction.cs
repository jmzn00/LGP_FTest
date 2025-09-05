using System.Runtime.CompilerServices;
using UnityEngine;

public class LockInteraction : MonoBehaviour
{
    private InventoryUI _inventoryUi;
    private CameraController _cameraController;
    private Interactable _currentLock;
    private int _lockId;

    private void Awake()
    {
        _inventoryUi = GetComponent<InventoryUI>();
        _cameraController = GetComponent<CameraController>();
    }
    public void FocusLock(Interactable currentLock, int lockId) 
    {
        _inventoryUi?.ToggleInventory(true);

        _currentLock = currentLock;
        _lockId = lockId;
    }
    public void UnFocusLock() 
    {
        _currentLock = null;
        _lockId = -1;
    }
    public bool TryOpenLock(int id) 
    {
        if (id == _lockId) 
        {
            _cameraController.SetCameraFocusInteractable(_currentLock);
            _currentLock.PlayAnimation(InteractableAnimationType.Unlock);
            Debug.Log($"Lock {_lockId} Unlocked with key {id}");
            return true;
        }
        Debug.Log($"Wrong Key {id} for {_lockId}");
        return false;
    }   
    public void OnAnimationFinished(Interactable item) 
    {
        if (item) 
        {
            item.Used();
        }
        else 
        {
            Debug.LogError($"item is NULL cannot call Used();", this);
            return;
        }
    }
}

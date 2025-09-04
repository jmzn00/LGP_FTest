using UnityEngine;

public class LockInteraction : MonoBehaviour
{
    private InventoryUI _inventoryUi;
    private Interactable _currentLock;
    private int _lockId;

    private void Awake()
    {
        _inventoryUi = GetComponent<InventoryUI>();
    }
    public void FocusLock(Interactable curentLock, int lockId) 
    {
        _inventoryUi?.ToggleInventory(true);

        _currentLock = curentLock;
        _lockId = lockId;
    }
    public void UnFocusLock() 
    {
        _currentLock = null;
        _lockId = -1;
    }
    public bool TryOpenLock(int id) 
    {
        if(id == _lockId) 
        {
            _currentLock.Use();
            return true;
        }
        return false;
    }   
}

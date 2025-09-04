using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private List<InventoryItem> _inventoryItems;
    public List<InventoryItem> InventoryItems => _inventoryItems;

    private InventoryUI _inventoryUI;

    [SerializeField] private Transform inventoryHolster;

    [SerializeField] private int _inventorySlots = 5;
    public int InventorySlots 
    {
        get => _inventorySlots;
        set 
        {
            if(_inventorySlots != value) 
            {
                OnInventorySlotsChanged(_inventorySlots, value);
                _inventorySlots = value;
            }
        }
    }

    private void Awake()
    {
        _inventoryUI = GetComponent<InventoryUI>();
    }

    private void Start()
    {
        _inventoryItems = new List<InventoryItem>();
        SubscribeInputs();
        OnInventorySlotsChanged(_inventorySlots, _inventorySlots);
        OnInventoryItemsChanged(_inventoryItems);
    }
    

    private void SubscribeInputs() 
    {
        if(InputManager.Instance == null) 
        {
            Debug.LogError("InputManager Instance is NULL");
            return;
        }
        InputManager.Instance.Actions.UI.OpenInventory.performed += ctx => CheckInventoryItems();
        //InputManager.Instance.Actions.UI.OpenInventory.canceled += ctx => CheckInventoryItems();

    }
    private void OnInventorySlotsChanged(int oldValue, int newValue) 
    {
        _inventoryUI.OnInventorySlotsChanged(oldValue, newValue);
    }
    private void OnInventoryItemsChanged(List<InventoryItem> items) 
    {
        _inventoryUI.OnInventoryItemsChanged(items);
    }
    public bool TryAdd(InventoryItem item) 
    {
        if(_inventoryItems.Count >= _inventorySlots) 
        {
            return false;
        }
        else 
        {
            _inventoryItems.Add(item);
            OnInventoryItemsChanged(_inventoryItems);
            return true;
        }

    }
    public void RemoveAt(int index) 
    {
        _inventoryItems.RemoveAt(index);
        OnInventoryItemsChanged(_inventoryItems);
    }
    public void RemoveInventoryItem(InventoryItem item) 
    {
        _inventoryItems.Remove(item);
        OnInventoryItemsChanged(_inventoryItems);
    }

    bool inventoryToggle = false;
    public void CheckInventoryItems() 
    {
        inventoryToggle = !inventoryToggle;
        _inventoryUI.ToggleInventory(inventoryToggle);
    }

}

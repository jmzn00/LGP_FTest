using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private List<InventoryItem> _inventoryItems = new();
    public List<InventoryItem> InventoryItems => _inventoryItems;

    private PlayerUiManager _playerUi;

    [SerializeField] private Transform inventoryHolster;

    [SerializeField] private int _inventorySlots = 5;
    private int _usedSlots = 0;
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
        _playerUi = GetComponent<PlayerUiManager>();
    }

    private void Start()
    {
        SubscribeInputs();
        OnInventorySlotsChanged(_inventorySlots, _inventorySlots);
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
        _playerUi.OnInventorySlotsChanged(oldValue, newValue);
    }
    private void OnInventoryItemsChanged(List<InventoryItem> items) 
    {
        _playerUi.OnInventoryItemsChanged(items);
    }
    public void AddInventoryItem(InventoryItem item) 
    {
        if(_usedSlots <= _inventorySlots) 
        {
            _inventoryItems.Add(item);

            if (item.visuals)
                item.visuals.SetActive(false);

            OnInventoryItemsChanged(_inventoryItems);
            _usedSlots++;
        }
        else 
        {
            Debug.Log("Inventory is full");
        }
    }
    public void RemoveInventoryItem(InventoryItem item) 
    {
        _inventoryItems.Remove(item);
        item.visuals.SetActive(true);
        OnInventoryItemsChanged(_inventoryItems);
        _usedSlots--;
    }

    public void CheckInventoryItems() 
    {
        _playerUi.ToggleInventory(_inventoryItems);
    }
}

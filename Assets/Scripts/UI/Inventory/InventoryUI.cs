using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private Transform inventorySlotTransform;

    private List<InventoryItem> _inventoryItems = new();

    bool inventoryToggle = false;
    public bool InventoryOpen { get; private set; } = false;
    public void ToggleInventory(List<InventoryItem> items)
    {
        InventoryOpen = !InventoryOpen;

        inventoryPanel.SetActive(InventoryOpen);
        InputManager.Instance?.TogglePlayerInputs(!InventoryOpen);
    }

    private readonly List<InventorySlotView> slotViews = new();
    public void OnInventorySlotsChanged(int oldValue, int newValue)
    {
        // Destroy old
        for (int i = 0; i < slotViews.Count; i++)
            if (slotViews[i]) Destroy(slotViews[i].gameObject);
        slotViews.Clear();

        // Create exactly newValue slots
        for (int i = 0; i < newValue; i++)
        {
            var go = Instantiate(inventorySlotPrefab, inventorySlotTransform);
            var view = go.GetComponent<InventorySlotView>();
            if (!view)
            {
                Debug.LogError("inventorySlotPrefab must include InventorySlotView");
                continue;
            }

            // Wire callbacks once
            view.Init(i, OnSlotClicked, OnSlotHover);
            view.Bind(null); // start empty

            slotViews.Add(view);
        }

        // Bind whatever items we currently have
        UpdateInventoryBindings();
    }
    public void OnInventoryItemsChanged(List<InventoryItem> items)
    {
        _inventoryItems = items ?? new List<InventoryItem>();
        UpdateInventoryBindings();
    }

    // ---- CLICK / HOVER ----

    private void OnSlotClicked(int index, InventoryItem item, PointerEventData evt)
    {
        if (item == null) return;

        // Example: left-click opens a context menu; right-click quick action; double-click use
        // if (evt.button == PointerEventData.InputButton.Right) { /* quick drop/equip */ return; }
        // if (evt.clickCount >= 2) { /* quick use/equip */ return; }                      
        // contextMenu.Show(evt.position, item, action => HandleAction(action, index));

        if (evt.button == PointerEventData.InputButton.Left)
        {
            ToggleItemMenu(evt.position, item, index);
        }

    }

    [Header("ItemMenu")]
    [SerializeField] private ItemMenu _itemMenu;
    [SerializeField] private UnityEngine.UI.Button useButton;
    [SerializeField] private UnityEngine.UI.Button inspectButton;
    [SerializeField] private UnityEngine.UI.Button equipButton;
    [SerializeField] private UnityEngine.UI.Button dropButton;

    private Dictionary<ItemAction, UnityEngine.UI.Button> _buttonMap;

    private PlayerInventory _playerInventory;
    private ItemActionExecutor _actionExecutor;
    private void Awake()
    {
        _buttonMap = new Dictionary<ItemAction, UnityEngine.UI.Button>()
        {
            { ItemAction.Use, useButton },
            { ItemAction.Inspect, inspectButton },
            { ItemAction.Equip, equipButton },
            { ItemAction.Drop, dropButton }
        };
        HideAllButtons();
        if(_itemMenu) _itemMenu.gameObject.SetActive(false);

        _playerInventory = GetComponent<PlayerInventory>();
        _actionExecutor = GetComponent<ItemActionExecutor>();
    }
    private void HideAllButtons() 
    {
        foreach (var b in _buttonMap.Values) 
        {
            b.gameObject.SetActive(false);
            b.onClick.RemoveAllListeners();
        }
    }
    private void ShowAllButtonsFor(InventoryItem item, Action<ItemAction> onClick) 
    {
        HideAllButtons();
        foreach (var action in item.Actions) 
        {
            if (!_buttonMap.TryGetValue(action, out var btn)) continue;

            btn.gameObject.SetActive(true);
            btn.onClick.AddListener(() =>
            {
                onClick?.Invoke(action);
                _itemMenu.gameObject.SetActive(false);
            });
        }
    }
    public void ToggleItemMenu(Vector3 pos, InventoryItem item, int index)
    {
        if(!_itemMenu.gameObject.activeInHierarchy)
            _itemMenu.gameObject.SetActive(true);

        _itemMenu.transform.position = pos;

        ShowAllButtonsFor(item, action => HandleAction(action, index, item));
    }
    private void HandleAction(ItemAction action , int index, InventoryItem item) 
    {
        var ctx = new ItemActionContext()
        {
            user = gameObject,
            worldPos = gameObject.transform.position,
            inventory = _playerInventory,
            itemDef = item,
            slotIndex = index
        };
        _actionExecutor.Execute(action, ctx);
        UpdateInventoryBindings();
    }
    private void UpdateInventoryBindings()
    {
        if (slotViews.Count == 0 || slotViews.Count < _inventoryItems.Count) return;

        for (int i = 0; i < slotViews.Count; i++)
        {
            var item = (i < _inventoryItems.Count) ? _inventoryItems[i] : null;
            slotViews[i].Bind(item);
        }
    }

    private void OnSlotHover(int index, InventoryItem item, bool isOver)
    {
        // Optional: show/hide tooltip
        // if (isOver && item) tooltip.Show(item.displayName, item.description);
        // else tooltip.Hide();
    }
}

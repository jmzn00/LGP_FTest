using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private PlayerUiManager _playerUiManager;

    [Header("Inventory")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject inspectPanel;
    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private Transform inventorySlotTransform;

    private PlayerInventory _playerInventory;
    private ItemActionExecutor _actionExecutor;
    private List<InventoryItem> _inventoryItems = new();
    private readonly List<InventorySlotView> slotViews = new();
    public bool InventoryOpen { get; private set; } = false;

    [Header("ItemMenu")]
    [SerializeField] private ItemMenu _itemMenu;
    [SerializeField] private UnityEngine.UI.Button useButton;
    [SerializeField] private UnityEngine.UI.Button inspectButton;
    [SerializeField] private UnityEngine.UI.Button equipButton;
    [SerializeField] private UnityEngine.UI.Button dropButton;
    [Space]
    [SerializeField] private Color buttonSelectedColor = Color.yellow;

    private Dictionary<ItemAction, UnityEngine.UI.Button> _buttonMap;

    [Header("Interaction")]
    [SerializeField] private RawImage grabImage;

    [Header("Controller Navigation")]
    [SerializeField] private GridLayoutGroup grid;
    private int _selectedIndex = 0;
    private Vector2 _prevNavRaw = Vector2.zero;

    [Header("Audio")]
    [SerializeField] private AudioSource uiAudioSource;
    [SerializeField] private AudioClip slotHoverSound;
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
        if (_itemMenu) _itemMenu.gameObject.SetActive(false);

        _playerInventory = GetComponent<PlayerInventory>();
        _actionExecutor = GetComponent<ItemActionExecutor>();
        _playerUiManager = GetComponent<PlayerUiManager>();


    }
    private void Start()
    {
        SubscribeInputs();
    }
    private void SubscribeInputs()
    {
        if (InputManager.Instance == null)
        {
            Debug.LogError("InputManager Instance is NULL");
            return;
        }
        InputManager.Instance.Actions.UI.Cancel.performed += ctx => OnCancel();
        InputManager.Instance.Actions.UI.Navigate.performed += ctx => OnNavigate(ctx.ReadValue<Vector2>());
        InputManager.Instance.Actions.UI.OpenInventory.performed += ctx => ToggleInventory(true);
        InputManager.Instance.Actions.UI.Submit.performed += ctx => OnSelect();

    }

    #region Controller Navigation
    private void OnSelect() 
    {
        if (slotViews.Count == 0 || !InventoryOpen || _inventoryItems.Count == 0) return;

        if (_itemMenu.gameObject.activeInHierarchy) 
        {
            prevButton?.onClick.Invoke();
            return;
        }

        var view = slotViews[_selectedIndex];

        if(view.CurrentItem != null)
            ToggleItemMenu(view.transform.position, view.CurrentItem, _selectedIndex);
    }
    private void OnNavigate(Vector2 val) 
    {
        if (!InventoryOpen || slotViews.Count == 0) return;

        const float dead = 0.5f;

        int dx = 0, dy = 0;

        if (Mathf.Abs(val.x) > Mathf.Abs(val.y))
        {
            if (val.x > dead) dx = 1;
            if (val.x < -dead) dx = -1;
        }
        else
        {
            if (val.y > dead) dy = 1; ;
            if (val.y < -dead) dy = -1;
        }

        if (dx != 0 || dy != 0)
        {
            if (_itemMenu.gameObject.activeInHierarchy) 
            {
                TryMoveItemMenu(dy); // Item menu navigation
            }
            else 
            {
                TryMoveInventory(dx, dy); // Inventory navigation
            }
            
        }
    }

    private int _menuSelectedIndex = 0;
    private UnityEngine.UI.Button prevButton = null;
    private void TryMoveItemMenu(int dy) 
    {
        if (!_itemMenu) return;

        UnityEngine.UI.Button[] activeButtons = _itemMenu.GetActiveButtons();
        int count = activeButtons.Length;
        if (count == 0) return;

        _menuSelectedIndex -= dy;

        if(_menuSelectedIndex >= count)
            _menuSelectedIndex = 0;
        if(_menuSelectedIndex < 0)
            _menuSelectedIndex = count - 1;

        if (prevButton) 
        {
            Image image = prevButton.GetComponent<Image>();
            if (image) image.color = Color.white;
        }

        prevButton = activeButtons[_menuSelectedIndex];
        Image img = prevButton.GetComponent<Image>();
        if (img) img.color = buttonSelectedColor;
    }
    private void TryMoveInventory(int dx, int dy)
    {
        int cols = grid.constraintCount;
        int slots = slotViews.Count;

        int row = _selectedIndex / cols;
        int col = _selectedIndex % cols;

        if (dx != 0)
            col += dx;
        if (dy != 0)
            row -= dy;

        // wrap 
        if (col < 0) col = cols - 1;
        if (col >= cols) col = 0;

        int rows = Mathf.CeilToInt((float)slots / cols);

        // wrap vertically
        if (row < 0) row = rows - 1;
        if (row >= rows) row = 0;

        int newIndex = row * cols + col;
        if (newIndex >= slots)
        {
            while (col > 0 && (row * cols + col) >= slots) col--;
            newIndex = row * cols + col;

            if (newIndex >= slots) return;
        }

        if (newIndex == _selectedIndex) return;

        ApplySelection(newIndex);
    }
    private void ApplySelection(int newIndex)
    {
        if (_selectedIndex >= 0 && _selectedIndex < slotViews.Count)
            slotViews[_selectedIndex].SetSelected(false);

        _selectedIndex = newIndex;
        if (_selectedIndex >= 0 && _selectedIndex < slotViews.Count)
        {
            var view = slotViews[_selectedIndex];
            view.SetSelected(true);
        }
    }
    public void OnCancel()
    {
        if (inspectPanel.activeInHierarchy)
            inspectPanel.SetActive(false);
        else if (_itemMenu.gameObject.activeInHierarchy)
            _itemMenu.gameObject.SetActive(false);
        else if (InventoryOpen)
            ToggleInventory(false);
    }
    #endregion


    public void ToggleInteraction(bool value)
    {
        if (value)
        {
            if (!grabImage.gameObject.activeInHierarchy)
            {
                grabImage.gameObject.SetActive(true);
                _playerUiManager?.ShowTooltip("Press 'E' to interact", true);
            }

        }
        else
        {
            if (grabImage.gameObject.activeInHierarchy)
            {
                grabImage.gameObject.SetActive(false);
                _itemMenu.gameObject.SetActive(false);
                _playerUiManager?.ShowTooltip("", false);
            }

        }
    }    
    public void ToggleInventory(bool value)
    {
        InventoryOpen = value;

        inventoryPanel.SetActive(InventoryOpen);
        InputManager.Instance?.TogglePlayerInputs(!InventoryOpen);
    }
    public void OnInventorySlotsChanged(int oldValue, int newValue)
    {
        for (int i = 0; i < slotViews.Count; i++)
            if (slotViews[i]) Destroy(slotViews[i].gameObject);
        slotViews.Clear();

        for (int i = 0; i < newValue; i++)
        {
            var go = Instantiate(inventorySlotPrefab, inventorySlotTransform);
            var view = go.GetComponent<InventorySlotView>();
            if (!view)
            {
                Debug.LogError("inventorySlotPrefab must include InventorySlotView");
                continue;
            }

            view.Init(i, OnSlotClicked, OnSlotHover);
            view.Bind(null);

            slotViews.Add(view);
        }
        UpdateInventoryBindings();
    }
    public void OnInventoryItemsChanged(List<InventoryItem> items)
    {
        _inventoryItems = items ?? new List<InventoryItem>();
        UpdateInventoryBindings();        
    }

    public void OnInventoryItemAdded() 
    {
        int index = _inventoryItems.Count - 1;

        ToggleInventory(true);
        HandleAction(ItemAction.Inspect, index, _inventoryItems[index]);
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
        if (!_itemMenu.gameObject.activeInHierarchy) 
        {
            _itemMenu.gameObject.SetActive(true);
            if (prevButton) // ??? for resetting the selected color for controller navigation ???
            {
                prevButton.gameObject.GetComponent<Image>().color = Color.white;
                _menuSelectedIndex = 0;
                prevButton = null;
            }
            
        }
        

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
        if (isOver) 
        {
            uiAudioSource?.PlayOneShot(slotHoverSound);
        }
        
    }
}

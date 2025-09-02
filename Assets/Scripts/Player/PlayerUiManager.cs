using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class PlayerUiManager : MonoBehaviour
{
    [SerializeField] private TMP_Text staminaText;
    [SerializeField] private RawImage grabImage;

    [Header("Inventory")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private Transform inventorySlotTransform;

    private List<InventoryItem> _inventoryItems = new();

    public enum UiUpdate { Stamina }
    
    public void UpdateUI(UiUpdate toUpd, int amount) 
    {
        switch (toUpd) 
        {
            case UiUpdate.Stamina:
                staminaText.text = amount.ToString();
                break;
        }        
    }
    public void ToggleInteraction(bool value) 
    {
        if (value) 
        {
            if (!grabImage.gameObject.activeInHierarchy) 
            {
                grabImage.gameObject.SetActive(true);
            }
            
        }
        else
        {
            if (grabImage.gameObject.activeInHierarchy) 
            {
                grabImage.gameObject.SetActive(false);
            }
            
        }
    }

    bool inventoryToggle = false;
    public bool InventoryOpen { get; private set; } = false;
    public void ToggleInventory(List<InventoryItem> items) 
    {
        InventoryOpen = !InventoryOpen;

        inventoryPanel.SetActive(InventoryOpen);
    }

    private readonly List<InventorySlotView> slotViews = new();
    public void OnInventorySlotsChanged(int oldValue, int newValue)
    {
        Debug.Log("Slots Changed" + newValue);
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

        if(evt.button == PointerEventData.InputButton.Left) 
        {
            ToggleItemMenu(evt.position, item);
        }

    }
    [SerializeField] private GameObject itemMenuPanel;
    private InventoryItem _currentItemMenu;

    public void ToggleItemMenu(Vector3 pos, InventoryItem item) 
    {
        ItemMenu menu = itemMenuPanel.GetComponent<ItemMenu>();
        if (!menu || !item)
        {
            Debug.LogError("ItemMenu Or Item Not Found"); 
            return;
        }

        if (!itemMenuPanel.activeInHierarchy)
            itemMenuPanel.SetActive(true);

        itemMenuPanel.transform.position = pos;

        UnityEngine.UI.Button useButton = menu.UseButton;
        useButton.gameObject.SetActive(false);
        UnityEngine.UI.Button inspectButton = menu.InspectButton;
        inspectButton.gameObject.SetActive(false);
        UnityEngine.UI.Button equipButton = menu.EquipButton;
        equipButton.gameObject.SetActive(false);
        UnityEngine.UI.Button dropButton = menu.DropButton;
        dropButton.gameObject.SetActive(false);

        if (item.usable) 
        {
            useButton.gameObject.SetActive(true);    
        }
        if (item.inspectable) 
        {
            inspectButton.gameObject.SetActive(true);
        }
        if (item.equippable) 
        {
            equipButton.gameObject.SetActive(true);
        }
        if (item.droppable) 
        {
            dropButton.gameObject.SetActive(true);
        }


        
    }
    private void UpdateInventoryBindings()
    {
        if (slotViews.Count == 0) return;

        Debug.Log($"Slot Views {slotViews.Count}");
        Debug.Log($"Inventory Items {_inventoryItems.Count}");

        for (int i = 0; i < _inventoryItems.Count; i++) 
        {
            var item = _inventoryItems[i];
            slotViews[i].Bind(item);
        }
        /*
        for (int i = 0; i < slotViews.Count; i++)
        {
            var item = (i < _inventoryItems.Count) ? _inventoryItems[i] : null;
            slotViews[i].Bind(item);
        }
        */
    }

    private void OnSlotHover(int index, InventoryItem item, bool isOver)
    {
        // Optional: show/hide tooltip
        // if (isOver && item) tooltip.Show(item.displayName, item.description);
        // else tooltip.Hide();
    }


} 

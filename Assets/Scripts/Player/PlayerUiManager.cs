using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUiManager : MonoBehaviour
{
    [SerializeField] private TMP_Text staminaText;
    [SerializeField] private RawImage grabImage;

    [Header("Inventory")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private Transform inventorySlotTransform;

    private List<GameObject> inventorySlotVisuals = new();
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
            if(!grabImage.gameObject.activeInHierarchy)
                grabImage.gameObject.SetActive(true);
        }
        else
        {
            if (grabImage.gameObject.activeInHierarchy)
                grabImage.gameObject.SetActive(false);
        }
    }

    bool inventoryToggle = false;
    public void ToggleInventory(List<InventoryItem> items) 
    {
        inventoryToggle = !inventoryToggle;
        
        inventoryPanel.SetActive(inventoryToggle);

        if (inventoryPanel.activeInHierarchy) 
        {
            
        }

    }

    public void OnInventorySlotsChanged(int oldValue, int newValue) 
    {
        for (int i = 0; i < inventorySlotVisuals.Count; i++) 
        {
            if (inventorySlotVisuals[i])
                Destroy(inventorySlotVisuals[i]);
        }
        inventorySlotVisuals.Clear();

        for (int i = 0; i <= newValue; i++) 
        {
            GameObject slot = Instantiate(inventorySlotPrefab, inventorySlotTransform);
            inventorySlotVisuals.Add(slot);
        }
    }
    public void OnInventoryItemsChanged(List<InventoryItem> items) 
    {
        _inventoryItems = items;

        UpdateInventorySprites();
    }

    private void UpdateInventorySprites() 
    {
        for(int i = 0; i < _inventoryItems.Count; i++) 
        {
            Image image = inventorySlotVisuals[i].GetComponent<Image>();
            if (image) 
            {
                image.sprite = _inventoryItems[i].sprite;
            }
        }
    }
} 

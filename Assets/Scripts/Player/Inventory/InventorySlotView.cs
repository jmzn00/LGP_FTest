using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotView : MonoBehaviour,
    IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    [SerializeField] private Image icon;                 // assign in prefab (falls back to child)
    [SerializeField] private Image selectionHighlight;   // optional

    [Header("Options")]
    [SerializeField] private bool preserveAspect = true;

    public int Index { get; private set; } = -1;
    public InventoryItem CurrentItem { get; private set; }

    private Action<int, InventoryItem, PointerEventData> _onClick;
    private Action<int, InventoryItem, bool> _onHoverChanged;

    private void Awake()
    {
        if (!icon) icon = GetComponentInChildren<Image>(true);
        if (icon) icon.preserveAspect = preserveAspect;
        SetSelected(false);     
    }

    public void Init(
        int index,
        Action<int, InventoryItem, PointerEventData> onClick,
        Action<int, InventoryItem, bool> onHoverChanged = null)
    {
        Index = index;
        _onClick = onClick;
        _onHoverChanged = onHoverChanged;
    }

    public void Bind(InventoryItem item)
    {
        CurrentItem = item;

        if (!icon) return;

        if (item && item.sprite)
        {
            icon.sprite = item.sprite;
            icon.enabled = true;
        }
        else
        {
            ClearIcon();
        }
    }

    public void Unbind()
    {
        CurrentItem = null;
        ClearIcon();
        SetSelected(false);
        // Keep callbacks from Init so re-binding later still works.
    }

    public void Clear() => Unbind();

    private void ClearIcon()
    {
        if (!icon) return;
        icon.sprite = null;
        icon.enabled = false;
    }

    public void SetSelected(bool selected)
    {
        if (selectionHighlight) selectionHighlight.enabled = selected;
    }

    // --- Events ---

    public void OnPointerClick(PointerEventData eventData)
    {
        // If you want empty-slot clicks to do something later, remove this guard.
        if (!CurrentItem) return;
        _onClick?.Invoke(Index, CurrentItem, eventData);        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _onHoverChanged?.Invoke(Index, CurrentItem, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _onHoverChanged?.Invoke(Index, CurrentItem, false);
    }

    private void OnDisable()
    {
        SetSelected(false);
    }
}

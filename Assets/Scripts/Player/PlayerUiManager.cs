using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class PlayerUiManager : MonoBehaviour
{
    [SerializeField] private TMP_Text staminaText;
    [SerializeField] private RawImage grabImage;

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
} 

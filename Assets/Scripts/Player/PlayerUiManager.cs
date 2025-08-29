using TMPro;
using UnityEngine;

public class PlayerUiManager : MonoBehaviour
{
    [SerializeField] private TMP_Text staminaText;

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
}

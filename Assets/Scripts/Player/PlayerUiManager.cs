using TMPro;
using UnityEngine;

public class PlayerUiManager : MonoBehaviour
{
    [SerializeField] private TMP_Text staminaText;

    public void UpdateStaminaValue(int amount) 
    {
        staminaText.text = amount.ToString();
    }
}

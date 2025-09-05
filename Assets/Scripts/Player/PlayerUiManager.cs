using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerUiManager : MonoBehaviour
{
    #region ToolTips
    [SerializeField] private Image toolTipImage;
    [SerializeField] private TMP_Text toolTipText;
    [SerializeField] private float toolTipDuration = 5f;
    public void ShowTooltip(string message, bool value) 
    {
        if (!value) 
        {
            toolTipImage.gameObject.SetActive(false);
            return;
        }

        toolTipImage.gameObject.SetActive(true);
        toolTipText.text = message;
    }
    #endregion
}

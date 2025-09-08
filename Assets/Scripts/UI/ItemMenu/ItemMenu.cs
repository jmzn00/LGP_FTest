using UnityEngine;

public class ItemMenu : MonoBehaviour
{
    public UnityEngine.UI.Button UseButton;
    public UnityEngine.UI.Button InspectButton;
    public UnityEngine.UI.Button EquipButton;
    public UnityEngine.UI.Button DropButton;

    [SerializeField] private UnityEngine.UI.Button[] _buttons;
    public UnityEngine.UI.Button[] Buttons => _buttons;

    public int GetActiveButtonsCount() 
    {
        int count = 0;
        for (int i = 0; i < _buttons.Length; i++) 
        {
            if (_buttons[i].gameObject.activeInHierarchy) 
            {
                count++;
            }
        }
        return count;
    }
    public UnityEngine.UI.Button[] GetActiveButtons() 
    {
        UnityEngine.UI.Button[] activeButtons = new UnityEngine.UI.Button[GetActiveButtonsCount()];

        int index = 0;
        for (int i = 0; i < _buttons.Length; i++) 
        {
            if (_buttons[i].gameObject.activeInHierarchy) 
            {
                activeButtons[index] = _buttons[i];
                index++;
            }
        }
        return activeButtons;
    }
}

using UnityEngine;

public class InputManager : MonoBehaviour
{
    private InputSystem_Actions actions = null;

    public InputSystem_Actions Actions => actions;

    public static InputManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null) 
        {
            Debug.LogWarning("Multiple instances of InputManager");
            Destroy(this.gameObject);
        }
        
        Instance = this;

        if(actions == null)
            actions = new InputSystem_Actions();
        
        actions.Enable();
    }

    public void TogglePlayerInputs(bool value) 
    {
        if(value)
            actions.Player.Enable();
        else
            actions.Player.Disable();
    }
    public void ToggleUiInputs(bool value) 
    {
        if(value)
            actions.UI.Enable();
        else
            actions.UI.Disable();
    }

    private void OnDisable()
    {
        actions.Disable();
    }
}

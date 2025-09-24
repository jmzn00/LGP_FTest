using UnityEngine;

[DefaultExecutionOrder(-100)] // run early
public class InputManager : MonoBehaviour
{
    private InputSystem_Actions actions = null;

    public InputSystem_Actions Actions => actions;   

    private void Awake()
    {
        GameServices.Input = this;

        actions ??= new InputSystem_Actions();        
    }
    private void OnEnable()
    {
        TogglePlayerInputs(true);
        ToggleUiInputs(true);
    }
    private void OnDisable()
    {
        actions.Disable();
        if (GameServices.Input == this) GameServices.Input = null;

    }

    public void TogglePlayerInputs(bool value) 
    {
        if (value) 
        {
            actions.Player.Enable();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else 
        {
            actions.Player.Disable();
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
                
    }
    public void ToggleUiInputs(bool value) 
    {
        if(value)
            actions.UI.Enable();
        else
            actions.UI.Disable();
    }    
}

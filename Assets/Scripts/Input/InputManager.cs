using UnityEngine;
using UnityEngine.Rendering.UI;

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

    private void OnDisable()
    {
        actions.Disable();
    }
}

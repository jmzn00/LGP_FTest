using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 5f;
    [SerializeField] private Transform interactionPivot;
    [SerializeField] LayerMask ignoreLayers;

    private Interactable currentInteractable = null;
    private MovementController _playerMovementController;
    private CameraController _cameraController;
    public Transform InteractionPivot => interactionPivot;
    bool isInspecting = false;
    

    private void Start()
    {
        SubscribeInputs();
        _playerMovementController = GetComponent<MovementController>();
        _cameraController = GetComponent<CameraController>();
    }
    private void SubscribeInputs() 
    {
        if(InputManager.Instance == null) 
        {
            Debug.LogError("InputManager is NULL");
            return;
        }

        InputManager.Instance.Actions.Player.Interact.performed += ctx => Interact();
        InputManager.Instance.Actions.Player.Interact.canceled += ctx => Interact();
        
    }
    private void Interact() 
    {
        isInspecting = !isInspecting;

        if (!isInspecting)
        {
            SetInspecting(null);
            currentInteractable.Interact(null);
            return;
        }
        Ray ray = _cameraController.MainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, interactionDistance, ~ignoreLayers))
        {
            Interactable interactable = hitInfo.collider.gameObject.GetComponent<Interactable>();
            if (interactable == null)
            {
                Debug.Log("No Interactable Item Found");
                return;
            }
            else
            {
                currentInteractable = interactable;
                currentInteractable.Interact(this);
            }
        }
    }

    public void SetInspecting(InspectableObject inspectable) 
    {
        _cameraController.SetInspectableObj(inspectable);
    }

    private void Update()
    {
        interactionPivot.transform.rotation = Quaternion.Euler(_playerMovementController.InputRot.x, transform.eulerAngles.y, 0);
    }
}

using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Transform interactionPivot;
    [SerializeField] LayerMask ignoreLayers;

    private MovementController _playerMovementController;
    private CameraController _cameraController;
    public Transform InteractionPivot => interactionPivot;

    private void Start()
    {
        _playerMovementController = GetComponent<MovementController>();
        _cameraController = GetComponent<CameraController>();
    }

    private void Update()
    {
        interactionPivot.transform.rotation = Quaternion.Euler(_playerMovementController.InputRot.x, transform.eulerAngles.y, 0);
    }

    public void SetInspecting(InspectableObject inspectable) 
    {
        _cameraController.SetInspectableObj(inspectable);
    }    
}

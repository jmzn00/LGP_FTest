using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private InspectableObject _inspectableObject;
    private MovementController _movementController;
    private InventoryUI _inventoryUI;

    [SerializeField] private float inspectOrbitDistance = 2.5f;
    [SerializeField] private float inspectOrbitHeight = 1.5f;
    [SerializeField] private float inspectOrbitSensitivity = 2f;

    [SerializeField] private Camera inspectCamera;
    private Camera _camera;

    public Camera MainCamera => _camera;

    private float inspectOribitAngle = 0f;


    private void Awake()
    {
        _camera = Camera.main;
        _movementController = GetComponent<MovementController>();
        _inventoryUI = GetComponent<InventoryUI>();
    }

    private void Start()
    {
        if(InputManager.Instance == null) 
        {
            Debug.LogError("InputManager is NULL");
            return;
        }
        InputManager.Instance.Actions.Player.Zoom.performed += ctx => InspectableZoom(ctx.ReadValue<float>());
    }
    private void Update()
    {
        if (isFocusing && focusTarget)
        {
            Vector3 target = focusTarget.transform.position + focusTarget.transform.forward;

            // Smooth position
            _camera.transform.position = Vector3.Lerp(
                _camera.transform.position,
                target,
                focusSmoothTime * Time.deltaTime
            );

            // Smooth rotation to look at target
            Vector3 direction = (focusTarget.transform.position + focusPositionOffset) - _camera.transform.position;
            _camera.transform.rotation = Quaternion.LookRotation(direction);

            if(Vector3.Distance(_camera.transform.position, target) < 0.1f) 
            {
                focusTarget.Used();
                SetCameraFocusInteractable(null);
            }
        }        
    }

    private bool isFocusing = false;
    private Interactable focusTarget;
    [SerializeField] private Vector3 focusPositionOffset = new Vector3(0f, 0f, 1f);
    [SerializeField] private float focusSmoothTime = 1f;
    public void SetCameraFocusInteractable(Interactable interactable) 
    {
        if (interactable) 
        {
            isFocusing = true;
            focusTarget = interactable;
            _inventoryUI.ToggleInventory(false);
            _movementController.SetCameraOverride(true);
        } 
        else 
        {
            isFocusing = false;
            focusTarget = null;
            _movementController.SetCameraOverride(false);
        }
    }

    public void SetCameraPosition(Vector3 target) 
    {
        _camera.transform.position = target;
    }
    public void SetCameraRotation(Quaternion rotation) 
    {
        _camera.transform.rotation = rotation;
    }
    public void SetCameraLookAt(Vector3 target) 
    {
        _camera.transform.LookAt(target);
    }
    public void SetInspectableObj(InspectableObject obj)
    {
        if(obj == null) 
        {
            _movementController.SetCameraOverride(false);
            inspectCamera.gameObject.SetActive(false);
            _camera.cullingMask = -1; // everything
        }
        else 
        {
            _inspectableObject = obj;
            _movementController.SetCameraOverride(true);
            inspectCamera.gameObject.SetActive(true);
            _camera.cullingMask = 0; // nothing
        }            
    }

    private void HandleObjectInspection() 
    {
        if (_inspectableObject != null)
        {
            inspectOribitAngle += _movementController.LookInput.x * inspectOrbitSensitivity;

            float radians = inspectOribitAngle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(
                Mathf.Sin(radians) * inspectOrbitDistance,
                inspectOrbitHeight,
                Mathf.Cos(radians) * inspectOrbitDistance
                );

            Vector3 cameraPos = _inspectableObject.transform.position + offset;
            Vector3 lookAt = _inspectableObject.transform.position + Vector3.up * inspectOrbitHeight;

            inspectCamera.transform.position = cameraPos;
            inspectCamera.transform.LookAt(lookAt);
        }
    }
    private void InspectableZoom(float value) 
    {
        if (_inspectableObject == null)
            return;

        inspectOrbitDistance += value;
    }
}

using UnityEngine;

public class CameraController : MonoBehaviour
{
    private InspectableObject _inspectableObject;
    private MovementController _movementController;
    
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

    private void Update()
    {
        HandleObjectInspection();
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

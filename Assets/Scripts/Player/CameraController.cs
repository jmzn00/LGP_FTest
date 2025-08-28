using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _camera;
    private InspectableObject _inspectableObject;

    private MovementController _movementController;

    private float inspectOribitAngle = 0f;
    [SerializeField] private float inspectOrbitDistance = 2.5f;
    [SerializeField] private float inspectOrbitHeight = 1.5f;
    [SerializeField] private float inspectOrbitSensitivity = 2f;



    private void Awake()
    {
        _camera = Camera.main;
        _movementController = GetComponent<MovementController>();
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
        }
        else 
        {
            _inspectableObject = obj;
            _movementController.SetCameraOverride(true);
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

            SetCameraPosition(cameraPos);
            SetCameraLookAt(lookAt);
        }
    }



}

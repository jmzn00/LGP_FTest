using TMPro;
using UnityEngine;

public class ItemDetection : MonoBehaviour
{
    [SerializeField] private float visionRange = 5f;
    [SerializeField] private float visionAngleDeg = 20f;
    [SerializeField] private float interactionDistance = 5f;
    [SerializeField] private LayerMask detectionMask;
    [SerializeField] private LayerMask interactableLayer;

    private CameraController _cameraController;
    private PlayerUiManager _playerUiManager;
    private PlayerInteraction _playerInteraction;

    private readonly Collider[] _overlap = new Collider[64];
    private readonly RaycastHit[] _rayHits = new RaycastHit[8];

    private float _cosAngle;
    private float _rangeSqr;
    private Interactable _current;

    [SerializeField] private float detectionHz = 10f;
    private float _accum;

    private void Start()
    {
        _cameraController = GetComponent<CameraController>();
        _playerUiManager = GetComponent<PlayerUiManager>();
        _playerInteraction = GetComponent<PlayerInteraction>();



        RecomputeCache();
        SubscribeInputs();
    }
    private void Update()
    {
        _accum += Time.deltaTime;
        float step = 1f / Mathf.Max(1f, detectionHz);
        if(_accum >= step) 
        {
            _accum -= step;
           
            DetectItemsCone();
        }
    }
    private bool isInspecting = false;
    private void Interact(Interactable interactable) 
    {

            if (_current.Moveable) 
        {
            isInspecting = !isInspecting;
            if (isInspecting) 
            {
                _current.Interact(_playerInteraction);
                return;
            }
            else 
            {
                _current.Interact(null);
                return;
            }
        }
        else 
        {
            interactable.Interact(_playerInteraction);
        }
        
    }
    public void SetInspecting(InspectableObject inspectable)
    {        
     _cameraController.SetInspectableObj(inspectable);               
    }
    private void SubscribeInputs() 
    {
        if(InputManager.Instance == null) 
        {
            Debug.LogError("InputManager Instance is NULL");
            return;
        }

        InputManager.Instance.Actions.Player.Interact.performed += ctx =>
        {
            if (_current != null)
            {
                Interact(_current);
            }
            else
                Debug.Log("_current is NULL");
        };
        InputManager.Instance.Actions.Player.Interact.canceled += ctx =>
        {
            if (_current != null)
            {
                Interact(_current);
            }
        };
    }
    private void DetectItemsCone() 
    {
        Vector3 camPos = _cameraController.MainCamera.transform.position;
        Vector3 camFwd = _cameraController.MainCamera.transform.forward;

        int count = Physics.OverlapSphereNonAlloc(camPos, visionRange, _overlap, interactableLayer, QueryTriggerInteraction.Ignore);

        Interactable best = null;
        float bestScore = -1f;

        for (int i = 0; i < count; i++) 
        {
            Collider col = _overlap[i];
            if (!col) continue;

            Interactable interactable = col.GetComponent<Interactable>(); // Maybe getColliderInParent?
            if (!interactable) continue;

            Vector3 to = col.bounds.center - camPos;
            float sqrDist = to.sqrMagnitude;
            if (sqrDist > _rangeSqr) continue;

            Vector3 dir = to.normalized;
            float dot = Vector3.Dot(dir, camFwd);
            if (dot < _cosAngle) continue;

            float maxDist = Mathf.Min(interactionDistance, Mathf.Sqrt(sqrDist));
            int hitCount = Physics.RaycastNonAlloc(camPos, dir, _rayHits, maxDist, detectionMask, QueryTriggerInteraction.Ignore);

            Debug.DrawRay(camPos, dir * maxDist, Color.yellow, 1f);

            bool visible = false;
            for(int h = 0; h < hitCount; h++) 
            {
                var hit = _rayHits[h];
                if (!hit.collider) continue;

                Interactable hitInteractable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    visible = (hitInteractable == interactable);
                    break;
                }
                else 
                {
                    visible = false;
                    break;
                }
            }

            if (!visible) continue;

            float dist = Mathf.Sqrt(sqrDist);
            float score = dot + 0.1f * (1f - Mathf.Clamp01(dist / visionRange));
        
            if(score > bestScore) 
            {
                bestScore = score;
                best = interactable;
            }
        }

        if(best != _current) 
        {
            _current = best;
            _playerUiManager.ToggleInteraction(_current != null);
        }
    }

    public void RecomputeCache()
    {
        _cosAngle = Mathf.Cos(visionAngleDeg * Mathf.Deg2Rad);
        _rangeSqr = visionRange * visionRange;
    }
}

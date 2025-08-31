using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    private List<Interactable> interactables = new();
    private HashSet<Interactable> currentlyVisibleTargets = new HashSet<Interactable>();

    [SerializeField] private Vector3 positionOffset = new Vector3(0, 1.75f, 0);
    [SerializeField] private float visionConeRange = 10f;
    [SerializeField] private float visionConeAngle = 90f;
    private float cosVisionAngle;
    private bool lastHitWasTarget;
    private Vector3 lastHitPoint;

    private PlayerUiManager playerUi;

    bool interactionPending = false;

    private void Awake()
    {
        cosVisionAngle = Mathf.Cos(visionConeAngle * Mathf.Deg2Rad);
        playerUi = GetComponent<PlayerUiManager>();
    }


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
        DetectItems();
    }
    [SerializeField] private LayerMask DetectionMask;
    private void DetectItems() 
    {
        if(InteractableDatabse.Instance == null) { return; }
        interactables = InteractableDatabse.Instance.GetInteractables();

        var visibleThisFrame = new HashSet<Interactable>();
        Interactable closestTarget = null;
        float closestDistanceSqrt = float.MaxValue;
        Vector3 closestHitPoint = Vector3.zero;

        for (int i = 0; i < interactables.Count; i++) 
        {
            var potentialTarget = interactables[i];

            Vector3 targetPos = potentialTarget.transform.position;
            Vector3 vectorToTarget = targetPos - transform.position;
            float sqrtDistance = vectorToTarget.sqrMagnitude;

            if(sqrtDistance > ((visionConeRange) * (visionConeRange))) 
            {
                if (currentlyVisibleTargets.Contains(potentialTarget)) 
                {
                    //playerUi.ToggleInteraction(false);
                    //lost sight
                }
                continue;
            }
            
            if (Vector3.Dot(vectorToTarget.normalized, transform.forward) < cosVisionAngle) 
            {
                if (currentlyVisibleTargets.Contains(potentialTarget)) 
                {
                    //playerUi.ToggleInteraction(false);
                    //lost sight
                }
                playerUi.ToggleInteraction(false);
                continue;
                
            }
            else 
            {
                playerUi.ToggleInteraction(true);
            }
            
            /*
            RaycastHit hitInfo;
            if(Physics.Raycast(transform.position + positionOffset, vectorToTarget.normalized, out hitInfo, visionConeRange, DetectionMask)) // detectionMask
            {
                Debug.DrawRay(transform.position + positionOffset, vectorToTarget.normalized * visionConeRange, Color.yellow, 0.1f);

                bool canSeeTarget = hitInfo.collider.transform.root.gameObject == potentialTarget.gameObject;
                if (canSeeTarget)
                {
                    visibleThisFrame.Add(potentialTarget);

                    if(sqrtDistance < closestDistanceSqrt) 
                    {
                        closestDistanceSqrt = sqrtDistance;
                        closestTarget = potentialTarget;
                        closestHitPoint = hitInfo.point;

                        playerUi.ToggleInteraction(true);
                    }
                }
                else 
                {
                    if (currentlyVisibleTargets.Contains(potentialTarget)) 
                    {
                        //Lost Sight
                        playerUi.ToggleInteraction(false);
                    }
                }
            }
            else 
            {
                if (currentlyVisibleTargets.Contains(potentialTarget)) 
                {
                    //Lost Sight
                    playerUi.ToggleInteraction(false);
                }
            }
            */
            
            
        }
        /*
        if(closestTarget != null) 
        {
           lastHitPoint = closestHitPoint;
           lastHitWasTarget = true;
        }
        else 
        {
            playerUi.ToggleInteraction(false);
            lastHitWasTarget = false;
        }

        currentlyVisibleTargets = visibleThisFrame;
            */
    }
    
}

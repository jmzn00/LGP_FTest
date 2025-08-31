using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDatabse : MonoBehaviour
{
    public static InteractableDatabse Instance { get; private set; }
    private List<Interactable> interactables = new();

    private void Awake()
    {
        if(Instance != null) 
        {
            Debug.LogWarning("Duplicate InteractableDatabase Instance", this);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void AddInteractable(Interactable item) 
    {
        if (!interactables.Contains(item)) 
        {
            interactables.Add(item);
        }
    }

    public void RemoveInteractable(Interactable item) 
    {
        if (interactables.Contains(item)) 
        {
            interactables.Remove(item);
        }
    } 

    public List<Interactable> GetInteractables() 
    {
        return interactables;
    }
}

using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private bool _moveable = false;
    public bool Moveable => _moveable;
    public virtual void Interact(PlayerInteraction interaction) 
    {
        Debug.Log($"Interacting with {this.gameObject}");
    }
    public virtual void Use() 
    {
        Debug.Log($"Using {gameObject.name}");
    }
    public virtual void Used() 
    {
        Debug.Log($"{gameObject.name} was used");
    }
    public virtual void PlayAnimation(InteractableAnimationType type) 
    {
        Debug.Log($"Playing animation {type} on {gameObject.name}");
    }

}

using UnityEngine;

public class Interactable : MonoBehaviour
{
    public virtual void Interact(PlayerInteraction interaction) 
    {
        Debug.Log($"Interacting with {this.gameObject}");
    }
}

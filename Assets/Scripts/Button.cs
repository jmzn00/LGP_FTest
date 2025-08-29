using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private bool toggle = false;
    [SerializeField] private Activatable activatable = null;

    private bool isActivated = false;
    private Animator _buttonAnimator;
    private List<GameObject> activators = new();

    private void Awake()
    {
        _buttonAnimator = GetComponent<Animator>();
        SetStatus(false);
    }

    private void SetStatus(bool active) 
    {
        if (active) 
        {
            _buttonAnimator.Play("Button_Down", 0, 0);
            if(activatable != null)
                activatable.SetStatus(true, this.gameObject);
        }
        else 
        {
            _buttonAnimator.Play("Button_Up", 0, 0);
            if(activatable != null)
                activatable.SetStatus(false, this.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Interactable")) 
        {
            if (toggle && isActivated)
                return;

            if (!activators.Contains(other.gameObject)) 
            {
                activators.Add(other.gameObject);
            }
            if (!isActivated) 
            {
                isActivated = true;
                SetStatus(true);
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (activators.Contains(other.gameObject))
        {
            activators.Remove(other.gameObject);
        }
        if (!toggle && activators.Count == 0) 
            {
                isActivated = false;
                SetStatus(false);
            }
    }

    


}

using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Activatable : MonoBehaviour
{
    private List<GameObject> activators = new();
    public virtual void SetStatus(bool value, GameObject activator) 
    {
        
        switch (value) 
        {
            case true:
                if (!activators.Contains(activator))
                {
                    bool wasEmpty = activators.Count == 0;
                    activators.Add(activator);
                    if (wasEmpty)
                        Activate();
                }
                break;
            case false:
                if (activators.Contains(activator))
                {
                    activators.Remove(activator);
                }
                if(activators.Count == 0)
                    Deactivate();
                break;
        }

    }
    public virtual void Activate() 
    {
        Debug.Log($"{this.gameObject} has been activated");
    }
    public virtual void Deactivate() 
    {
        Debug.Log($"{this.gameObject} has been deactivated");
    }
}

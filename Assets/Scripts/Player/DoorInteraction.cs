using UnityEngine;

public class LockInteraction : MonoBehaviour
{

    public bool TryOpenLock(int id) 
    {
        Debug.Log($"{id} was Opened");
        return true;
    }   
}

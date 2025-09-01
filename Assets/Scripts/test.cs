using UnityEngine;

public class test : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("Hello");
    }
    void Start()
    {
        Debug.Log("World");
        Debug.LogWarning("Warning");
        Debug.LogError("Error");

        print("Print World");
    }
}

using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ShaderModifyTrigger : MonoBehaviour
{
    [SerializeField] private Material mat;
    private List<string> propNames = new();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            
            
        }
    }
}

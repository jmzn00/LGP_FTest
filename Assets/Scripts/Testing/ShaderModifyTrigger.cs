using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal.ShaderGraph;
using UnityEngine;
using UnityEngine.Rendering;

public class ShaderModifyTrigger : MonoBehaviour
{
    [SerializeField] private Material mat;
    [SerializeField] private float speed = 2f;


    private float t = 0f;
    bool set = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            set = true;
        }
    }
    private void Start()
    {
        mat.SetFloat("_Range", 0f);
    }
    private void Update()
    {
        if (!set) return;

        t -= Time.deltaTime * speed;
        if(t < -2f) set = false;

        mat.SetFloat("_Range", t);
    }

}

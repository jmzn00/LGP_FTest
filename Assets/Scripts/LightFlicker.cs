using System.Collections;
using UnityEngine;

public class LightFlicker : Activatable
{
    private Light _light;
    [SerializeField] private float _minIntensity = 0.1f;
    [SerializeField] private float _maxIntensity = 3f;
    [SerializeField] private float _flickerSpeed = 0.1f;
    private bool flicker;
    private void Awake()
    {
        _light = GetComponent<Light>();
    }
    public override void Activate()
    {
        flicker = true;
        StartCoroutine(Flicker());
    }
    private void OnDisable()
    {
        flicker = false;
        StopAllCoroutines();
    }

    private IEnumerator Flicker() 
    {
        while (flicker) 
        {
            yield return new WaitForSeconds(_flickerSpeed);
            _light.intensity = Random.Range(_minIntensity, _maxIntensity);
        }        
    }
}

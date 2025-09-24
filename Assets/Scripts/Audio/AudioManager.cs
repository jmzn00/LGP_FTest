using UnityEngine;

[DefaultExecutionOrder(-100)]
public class AudioManager : MonoBehaviour
{
    private void Awake()
    {
        GameServices.Audio = this;
    }
    private void OnDisable()
    {
        if(GameServices.Audio = this) GameServices.Audio = null;
    }
}

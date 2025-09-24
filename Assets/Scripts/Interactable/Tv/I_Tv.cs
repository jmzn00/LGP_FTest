using UnityEngine;
using UnityEngine.Video;

public class I_Tv : Interactable
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VideoClip mainClip;
    private bool tvPlaying = false;

    private void Awake()
    {
        videoPlayer.Stop();
        videoPlayer.clip = mainClip;
    }
    public override void Interact(PlayerInteraction interaction)
    {
        if (!tvPlaying) 
        {            
            videoPlayer.Play();
            tvPlaying = true;
        }
        else 
        {
            videoPlayer.Stop();
            tvPlaying = false;
        }
    }
}

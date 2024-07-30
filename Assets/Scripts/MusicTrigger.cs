using UnityEngine;
using RPGM.Core;
using RPGM.Gameplay;

public class MusicTrigger : MonoBehaviour
{
    public AudioClip newClip;  // The new music clip to play

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<MC_Controller>() != null)
        {
            var musicController = FindObjectOfType<MusicController>();
            if (musicController != null)
            {
                musicController.CrossFade(newClip);
            }
        }
    }
}

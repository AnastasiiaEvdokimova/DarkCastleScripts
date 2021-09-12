using UnityEngine;

public class MusicManager : MonoBehaviour
{
   public AudioClip newMusic;
   private void  Start()
    {
        AudioSource go = GameObject.FindWithTag("MenuMusic").GetComponent<AudioSource>(); 
        if (go.clip != newMusic)
        {
            go.clip = newMusic;
            go.Play();
        }
    }
}

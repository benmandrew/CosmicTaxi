using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMusicManager : MonoBehaviour
{
    private static MainMenuMusicManager instance = null;
    private AudioSource bgMusic; 

    void Awake() {
        if (instance == null)
          { 
               instance = this;
               DontDestroyOnLoad(gameObject);
               return;
          }
          if (instance == this) return; 
          Destroy(gameObject);
    }

    void Start() {
        bgMusic = GetComponent<AudioSource>();
        // DontDestroyOnLoad(bgMusic);
    }

    public void ToggleMusic()
    {
        bgMusic.mute = !bgMusic.mute;
    }

    public bool getState()
    {
        return bgMusic.mute;
    }
}

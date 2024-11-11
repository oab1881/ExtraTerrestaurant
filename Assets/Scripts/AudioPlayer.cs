/*
 ==== Created by Jake Wardell 11/11/24 ====

Plays audio clips both sound effects and music

Attached: Will be attached to the AudioPlayer Gameobject

Changelog:
    -Created script: 11/11/24 : Jake
    
    
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    //For storing all sound effects
    //For storing all music clips
    [SerializeField]
    List<AudioClip> soundEffectClip= new List<AudioClip>();
    List<AudioClip> musicClips = new List<AudioClip>();

    //Stores reference to audio source
    [SerializeField]
    AudioSource audioSource;

    /// <summary>
    /// Plays a Sound effect by passing the name in
    /// </summary>
    /// <param name="name"></param>
    public void PlaySoundEffect(string name)
    {
        for(int i = 0; i < soundEffectClip.Count; i++)
        {
            if(name == soundEffectClip[i].name)
            {
                audioSource.PlayOneShot(soundEffectClip[i]);
                break;
            }
        }
    }

    public void PlayMusic(string name, bool loop)
    {
        /*
        for(int i = 0; i < musicClips.Count; i++)
        {
            if(name == musicClips[i].name)
            {
                audioSource.Play();
            }
        }
        */
    }
}

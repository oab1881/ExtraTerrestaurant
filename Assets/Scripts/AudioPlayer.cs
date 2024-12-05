/*
 ==== Created by Jake Wardell 11/11/24 ====

Plays audio clips both sound effects and music

Attached: Will be attached to the AudioPlayer Gameobject

Changelog:
    -Created script: 11/11/24 : Jake
    -Total revamp of the system : 12/03/24 : Jake
    -Made it so audioPlayer persists between scenes; Made music player version : 12/04/24
    
    
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    //For storing all sound effects
    //For storing all music clips
    [SerializeField]
    List<AudioClip> soundEffectClip= new List<AudioClip>();

    //Number of audio sources
    [SerializeField]
    int audioSourceCount;

    //Stores reference to audio sources
    [SerializeField]
    List<AudioSource> audioSources;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        //Creates the list then adds all the objects to it depending on how many should be added
        //Also adds each audio source as a compenent of the gameobject
        audioSources = new List<AudioSource>();
        for(int i =0; i < audioSourceCount; i++)
        {
            AudioSource temp = gameObject.AddComponent<AudioSource>();
            audioSources.Add(temp);
        }

    }

    /// <summary>
    /// Plays a Sound effect by passing the name in
    /// </summary>
    /// <param name="name">name of clip to play</param>
    /// <param name="index">index in the audio source list</param>
    public void PlaySoundEffect(string name, int index)
    {
       //We create a temp audioClip
       AudioClip temp = GetAudioClip(name);
        
        //Plays the sound once
        audioSources[index].PlayOneShot(temp);

    }

    public void playLongSound(string name, int index)
    {
        //Tests if the specified audioSource is already playing.
        //If it is we just return doing nothing
        if (audioSources[index].isPlaying)
        {
            return;
        }

        //Otherwise we create a temp audioClip
        AudioClip temp = GetAudioClip(name);

        

        //Sets the clip to temp
        //Then plays that clip
        audioSources[index].clip = temp;
        audioSources[index].Play();
    }

    /// <summary>
    /// Stops a sound from playing
    /// </summary>
    /// <param name="index">index in the list of audioSources</param>
    public void StopSound(int index)
    {
        audioSources[index].Stop();
    }

    public void PlayMusic(string name, int index)
    {
        audioSources[index].loop = true;
        playLongSound(name, index);
    }

    /// <summary>
    /// Created to avoid redudant code that finds the audio clip
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="System.Exception"></exception>
    private AudioClip GetAudioClip(string name)
    {
        //Goes through our list and tries finding our audio clip then sets it to temp
        for (int i = 0; i < soundEffectClip.Count; i++)
        {
            if (name == soundEffectClip[i].name)
            {
                return soundEffectClip[i];
            }
        }

        //If temp doesn't get set to anything ie the name was inncorrect doesn't exist
        throw new System.Exception("No vairable found " + name);
       
    }

    public void SetVolume(int index, float volume)
    {
        audioSources[index].volume = volume;
    }
}

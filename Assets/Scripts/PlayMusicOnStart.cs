/*
 ==== Created by Jake Wardell 12/04/24 ====

Starts the music when main menu loads up

Attached: Music Starter gameobject

Changelog:
    -Created script: 12/03/24 : Jake  
*/



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayMusicOnStart : MonoBehaviour
{

    // Start is called before the first frame update
    private void Start()
    {
        //Creates a temp and gets the audiomanager on music
        //Then sets volume pretty low cause music is a bit loud
        //Then starts the ambience on loop
        AudioPlayer temp = GameObject.Find("AudioManager(Music)").GetComponent<AudioPlayer>();
        temp.SetVolume(0, 0.03f);
        GameObject.Find("AudioManager(Music)").GetComponent<AudioPlayer>().PlayMusic("ambeince", 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDestroyer : MonoBehaviour
{
    public GameObject Sound;

    private void Start()
    {
        //"Sound" is passed between scenes
        //Createad this to destroy in scenes with own music
        Sound = GameObject.Find("Sound");

        Destroy(Sound);        
    }
}

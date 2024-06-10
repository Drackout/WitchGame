using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{

    //Audio
    [SerializeField] private AudioClip SoundClip;
    // Start is called before the first frame update

    public void AudioPlay()
    {
        //play soundfx
            SoundFXManager.instance.PlaySoundFXClip(SoundClip, transform, 1.0f);
    }
}
    

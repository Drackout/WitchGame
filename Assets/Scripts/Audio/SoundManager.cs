using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Slider sliderMusic;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            LoadVolume();
        }
        else
        {
            LoadVolume();
        }
    }

    public void VolumeChanger()
    {
        AudioListener.volume = sliderMusic.value;
        SaveVolume();
    }

    private void LoadVolume()
    {
        sliderMusic.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void SaveVolume()
    {
        PlayerPrefs.SetFloat("musicVolume", sliderMusic.value);
    }


}

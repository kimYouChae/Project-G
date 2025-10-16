using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundModel
{
    private AudioMixer audioMixer;

    public AudioMixer AudioMixer { get { return audioMixer; } }

    public SoundModel( AudioMixer mixer)
    {
        MasterVolume = 0f;
        SFXVolume = 0f;
        BGMVolume = 0f;

        this.audioMixer = mixer;
    }

    public float MasterVolume 
    {
        get => PlayerPrefs.GetFloat("Master");
        set => PlayerPrefs.SetFloat("Master", value);
    }

    public float SFXVolume
    {
        get => PlayerPrefs.GetFloat("SFX");
        set => PlayerPrefs.SetFloat("SFX", value);
    }

    public float BGMVolume
    {
        get => PlayerPrefs.GetFloat("BGM");
        set => PlayerPrefs.SetFloat("BGm", value);
    }
}

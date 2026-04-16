using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundVolume
{
    private const float baseVolume = 0f;

    public SoundVolume()
    {
    }

    public float MasterVolume 
    {
        // "Master"가 저장되 있으면 리턴, 아니면 base값
        get => PlayerPrefs.GetFloat("Master", baseVolume);
        set => PlayerPrefs.SetFloat("Master", value);
    }

    public float SFXVolume
    {
        // "SFX"가 저장되 있으면 리턴, 아니면 base값
        get => PlayerPrefs.GetFloat("SFX", baseVolume);
        set => PlayerPrefs.SetFloat("SFX", value);
    }

    public float BGMVolume
    {
        // "BGM"가 저장되 있으면 리턴, 아니면 base값
        get => PlayerPrefs.GetFloat("BGM", baseVolume);
        set => PlayerPrefs.SetFloat("BGM", value);
    }
}

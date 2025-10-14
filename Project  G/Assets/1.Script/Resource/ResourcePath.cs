using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Resource Data", menuName = "Scriptable Object/Resource Data")]
public class ResourcePath : ScriptableObject
{
    [Header("===Sprite===")]
    [SerializeField] private string defaultSpritePath;
    public string DefaultSpritePath => defaultSpritePath;

    [SerializeField] private string mapSpritePath;
    public string MapSpritePath => mapSpritePath;

    [Space]
    [Header("===Sound Asset===")]
    [SerializeField] private string soundPath;
    public string SoundPath => soundPath;

    [SerializeField] private string audioMixerPath;
    public string AudioMixerPath => audioMixerPath; 

    [SerializeField] private string sfxPath;
    public string SfxPath => sfxPath;

    [SerializeField] private string bgmPath;
    public string BgmPath => bgmPath;

    [Space]
    [Header("===Text Asset===")]
    [SerializeField] private string fallBackLocalizationData;
    public string FallBackLocalizationData => fallBackLocalizationData;

}

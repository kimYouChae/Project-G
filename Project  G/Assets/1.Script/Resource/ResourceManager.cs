using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ResourceManager : Singleton<ResourceManager>
{
    [SerializeField] private ResourceLoader resourceLoader;
    [SerializeField] private ResourcePath resourcePath;

    [Header("===Sprite===")]
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite[] mapSprite;
    [SerializeField] private Sprite[] characterSprite;

    [Header("===Audio===")]
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioClip[] sfxClip;
    [SerializeField] private AudioClip[] bgmClip;

    [Header("===Text Asset===")]
    [SerializeField] private TextAsset fallBackLocalizionTextfile;
    [SerializeField] private TextAsset[] fallBackChartTextfile;

    protected override void Singleton_Awake()
    {
        resourceLoader = new ResourceLoader();

        defaultSprite           = resourceLoader.RoadSprite(resourcePath.DefaultSpritePath + "/" + resourcePath.DefaultSpritePath);
        mapSprite               = resourceLoader.RoadSpriteAll(resourcePath.MapSpritePath);
        characterSprite         = resourceLoader.RoadSpriteAll(resourcePath.CharacterSpritePath);

        mixer                   = resourceLoader.RoadMixer(resourcePath.SoundPath + "/" + resourcePath.AudioMixerPath);
        sfxClip                 = resourceLoader.RoadClipAll(resourcePath.SoundPath + "/" + resourcePath.SfxPath);
        bgmClip                 = resourceLoader.RoadClipAll(resourcePath.SoundPath + "/" + resourcePath.BgmPath);

        fallBackLocalizionTextfile = resourceLoader.RoadFallBackLocalization(resourcePath.FallBackLocalizationData);
        fallBackChartTextfile = resourceLoader.RoadeFallBackChartData(resourcePath.FallBackChartData);
    }

    public Sprite GetDefaultSprite() => defaultSprite;

    public Sprite MapSprite(int idx) 
    {
        if (idx < 0 || idx >= mapSprite.Length)
            return defaultSprite;

        return mapSprite[idx];
    }

    public Sprite CharacterSprite(CharacterType type) 
    {
        int idx = (int)type;

        if (idx < 0 || idx >= characterSprite.Length)
            return defaultSprite;

        return characterSprite[idx];
    }

    public AudioMixer GetAudioMixer => mixer;
    public AudioClip[] GetSFXClip => sfxClip;
    public AudioClip[] GetBGMClip => bgmClip;

    public TextAsset FallBackLocalizationText => fallBackLocalizionTextfile;
    public TextAsset[] FallBackChartTextfile => fallBackChartTextfile;


}

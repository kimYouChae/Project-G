using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    [SerializeField] private ResourceLoader resourceLoader;
    [SerializeField] private ResourcePath resourcePath;

    [Header("===Sprite===")]
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite[] mapSprite;

    [Header("===AudioClip===")]
    [SerializeField] private AudioClip[] sfxClip;
    [SerializeField] private AudioClip[] bgmClip;

    protected override void Singleton_Awake()
    {
        resourceLoader = new ResourceLoader(resourcePath);

        defaultSprite = resourceLoader.RoadSprite(resourcePath.DefaultSpritePath + "/" + resourcePath.DefaultSpritePath);
        mapSprite = resourceLoader.RoadSpriteAll(resourcePath.MapSpritePath);

        sfxClip = resourceLoader.RoadClipAll(resourcePath.SoundPath + "/" + resourcePath.SfxPath);
        bgmClip = resourceLoader.RoadClipAll(resourcePath.SoundPath + "/" + resourcePath.BgmPath);
    }

    public Sprite MapSprite(int idx) 
    {
        if (idx < 0 || idx >= mapSprite.Length)
            return defaultSprite;

        return mapSprite[idx];
    }

    public AudioClip[] GetSFXClip => sfxClip;
    public AudioClip[] GetBGMClip => bgmClip;


}

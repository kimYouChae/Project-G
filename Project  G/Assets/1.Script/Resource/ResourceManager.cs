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

    protected override void Singleton_Awake()
    {
        resourceLoader = new ResourceLoader(resourcePath);

        defaultSprite = resourceLoader.RoadSprite(resourcePath.DefaultSpritePath + "/" + resourcePath.DefaultSpritePath);
        mapSprite = resourceLoader.RoadSpriteAll(resourcePath.MapSpritePath);
    }

    public Sprite MapSprite(int idx) 
    {
        if (idx < 0 || idx >= mapSprite.Length)
            return defaultSprite;

        return mapSprite[idx];
    }

    public AudioClip[] GetSFXClip => resourceLoader.RoadClipAll( resourcePath.SoundPath + "/" + resourcePath.SfxPath);
    public AudioClip[] GetBGMClip => resourceLoader.RoadClipAll(resourcePath.SoundPath + "/" + resourcePath.BgmPath);


}

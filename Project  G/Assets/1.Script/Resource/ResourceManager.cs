using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public static class ResourceLoaderGeneric 
{
    // Load
    public static T LoadAsset<T>(string path) where T : Object 
    {
        var entity = Resources.Load<T>(path);
        if(entity == null)
            return null;

        return entity;
    }

    // LoadeAll
    public static T[] LoadAssetAll<T>(string path) where T : Object 
    {
        var entity = Resources.LoadAll<T>(path);
        if (entity == null)
            return null;

        return entity; 
    }
}

public class ResourceManager : Singleton<ResourceManager>
{
    [SerializeField] private ResourcePath resourcePath;

    [Header("===Sprite===")]
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite[] mapSprite;
    [SerializeField] private Sprite[] characterSprite;

    [Header("===Text Asset===")]
    [SerializeField] private TextAsset fallBackLocalizionTextfile;
    [SerializeField] private TextAsset[] fallBackChartTextfile;

    protected override void Singleton_Awake()
    {
        SetUpDontDestroy();

        defaultSprite = ResourceLoaderGeneric.LoadAsset<Sprite>(resourcePath.GetPathEntity("DefaultSprite"));
        mapSprite = ResourceLoaderGeneric.LoadAssetAll<Sprite>(resourcePath.GetPathEntity("MapSprite"));
        characterSprite = ResourceLoaderGeneric.LoadAssetAll<Sprite>(resourcePath.GetPathEntity("CharacterSprite"));

        fallBackLocalizionTextfile = ResourceLoaderGeneric.LoadAsset<TextAsset>(resourcePath.GetPathEntity("FallBackLocalization"));
        fallBackChartTextfile = ResourceLoaderGeneric.LoadAssetAll<TextAsset>(resourcePath.GetPathEntity("FallBackChart"));
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

    public TextAsset FallBackLocalizationText => fallBackLocalizionTextfile;
    public TextAsset[] FallBackChartTextfile => fallBackChartTextfile;


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Language 
{
    private Dictionary<string, string> lauguageContainer;

    public Language() 
    {
        lauguageContainer = new Dictionary<string, string>();
    }
}

public enum LanguageType 
{
    English,
    Korean,
    Japanese,
    Chinese
}

public class LocalizationManager : Singleton<LocalizationManager>
{
    [SerializeField]
    private LanguageType lanType;
    [SerializeField]
    private Dictionary<LanguageType, Language> languages;

    // 인스펙터 보기용
    [SerializeField] private List<string> values;

    protected override void Singleton_Awake()
    {
        languages = new Dictionary<LanguageType, Language>();
    }
}

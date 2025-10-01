using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[System.Serializable]
public class Language 
{
    private Dictionary<string, string> lauguageContainer;

    public Language() 
    {
        lauguageContainer = new Dictionary<string, string>();
    }

    public void Add(string key, string value) 
    {
        if(!lauguageContainer.ContainsKey(key)) 
        {
            lauguageContainer.Add(key, value);
        }
    }

    public string Get(string key) 
    {
        if(lauguageContainer.ContainsKey(key))
        { return lauguageContainer[key]; }

        return string.Empty;
    }
}


public class LocalizationManager : Singleton<LocalizationManager>
{
    [SerializeField]
    private LanguageType lanType;
    [SerializeField]
    private Dictionary<LanguageType, Language> languages;

    private Action ChangeLanguageAction;

    protected override void Singleton_Awake()
    {
        SetLanguageType();

        languages = new Dictionary<LanguageType, Language>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V)) 
        {
            Debug.Log("v 눌림");
            ChangeLanguageType(LanguageType.English);
        }
    }

    public void AddLanguageDictionary(LanguageType type, string key, string value) 
    {
        if (!languages.ContainsKey(type))
        {
            languages.Add(type, new Language());
        }

        // type에 해당하는 language 클래스에 추가 
        languages[type].Add(key, value);
    }

    // 운영체제 언어 별로 Lang타입 정하기
    private void SetLanguageType() 
    {
        switch(Application.systemLanguage) 
        {
            case SystemLanguage.English:
                lanType = LanguageType.English; break;
            case SystemLanguage.Japanese:
                lanType = LanguageType.Japanese; break;
            case SystemLanguage.Korean:
                lanType = LanguageType.Korean; break;
            case SystemLanguage.Chinese: 
                lanType = LanguageType.Chinese; break;
            default:
                lanType = LanguageType.English; break;
        }
    }

    // 현재 lang 타입 정하기
    private void ChangeLanguageType(LanguageType type) 
    {
        lanType = type;

        ChangeLanguageAction?.Invoke();
    }

    // key에 맞는 문자열 return
    public string ReturnLolicalization(string key) 
    { 
        // language 타입은 현재 lang 타입
        if(languages.ContainsKey(lanType))
            return languages[lanType].Get(key);

        return string.Empty;
    }

    public void RegisterChangeLanguage(Action action) 
    {
        ChangeLanguageAction += action;
    }
}


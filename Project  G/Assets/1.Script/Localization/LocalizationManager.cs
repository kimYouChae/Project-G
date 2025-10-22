using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[System.Serializable]
public class Language 
{
    private Dictionary<string, string> languageContainer;

    public Language() 
    {
        languageContainer = new Dictionary<string, string>();
    }

    public void langAdd(string key, string value) 
    {
        if(languageContainer.ContainsKey(key))
            languageContainer[key] = value;  
        else 
            languageContainer.Add(key, value);
    }

    public string langGet(string key) 
    {
        if(languageContainer.ContainsKey(key))
        { return languageContainer[key]; }

        return string.Empty;
    }
}


public class LocalizationManager : Singleton<LocalizationManager>
{
    [SerializeField]
    private LanguageType currLanguateType;
    [SerializeField]
    private Dictionary<LanguageType, Language> languages;
    [SerializeField]
    private string[] mapNameLocalization;

    private Action<LanguageType> ChangeLanguageAction;

    protected override void Singleton_Awake()
    {
        SetLanguageType();

        languages = new Dictionary<LanguageType, Language>();

        // 맵 type 길이만큼 초기화
        mapNameLocalization = new string[ Extension.EnumCount<MapType>()];
        RegisterChangeLanguage(LocalizationMapNameList);

        // fallBack 로컬라이제이션 테이블 사용
        FallBackLocalization();
    }

    private void FallBackLocalization() 
    {
        // 1. 텍스트파일 가져오기
        TextAsset text = ResourceManager.Instance.FallBackLocalizationText;

        // 2. LitJson 파싱
        LitJson.JsonData jsonData = JsonMapper.ToObject(text.text);

        // 3. 클래스 생성
        LocalizationChart localChart = new LocalizationChart();
        localChart.IParseAndStore(jsonData["rows"]);
    }

    public void AddLanguageDictionary(LanguageType type, string key, string value) 
    {
        if (!languages.ContainsKey(type))
        {
            languages.Add(type, new Language());
        }

        // type에 해당하는 language 클래스에 추가 
        languages[type].langAdd(key, value);
    }

    // 운영체제 언어 별로 Lang타입 정하기
    private void SetLanguageType() 
    {
        switch(Application.systemLanguage) 
        {
            case SystemLanguage.English:
                currLanguateType = LanguageType.English; break;
            case SystemLanguage.Japanese:
                currLanguateType = LanguageType.Japanese; break;
            case SystemLanguage.Korean:
                currLanguateType = LanguageType.Korean; break;
            case SystemLanguage.Chinese: 
                currLanguateType = LanguageType.Chinese; break;
            default:
                currLanguateType = LanguageType.English; break;
        }
    }

    /// <summary>
    /// 언어 변경 시 실행할 메서드
    /// </summary>
    /// <param name="type">바꿀 언어 타입</param>
    public void ChangeLanguageType(LanguageType type) 
    {
        currLanguateType = type;

        ChangeLanguageAction?.Invoke(currLanguateType);
    }

    public void ChangeLanguageType() 
    {
        ChangeLanguageAction?.Invoke(currLanguateType);
    }

    // key에 맞는 문자열 return
    public string ReturnLocalizationString(LanguageType type , string key) 
    { 
        if(languages.ContainsKey(type))
            return languages[type].langGet(key);

        return string.Empty;
    }

    public string ReturnLocalizationString(string key) 
    {
        // language 타입은 현재 lang 타입
        if (languages.ContainsKey(currLanguateType))
            return languages[currLanguateType].langGet(key);

        return string.Empty;
    }

    public void RegisterChangeLanguage(Action<LanguageType> action) 
    {
        ChangeLanguageAction += action;
    }

    #region Map Name Localizatin
    // 현재 언어에 따라 mapName 배열의 값을 바꾸기
    private void LocalizationMapNameList(LanguageType type)
    {
        mapNameLocalization[(int)MapType.Forest] = ReturnLocalizationString(LocalizationKey.Map_Forest);
        mapNameLocalization[(int)MapType.GiganticTree] = ReturnLocalizationString(LocalizationKey.Map_GiganticTree);
        mapNameLocalization[(int)MapType.Market] = ReturnLocalizationString(LocalizationKey.Map_Market);
        mapNameLocalization[(int)MapType.Island] = ReturnLocalizationString(LocalizationKey.Map_Island);
        mapNameLocalization[(int)MapType.Hell] = ReturnLocalizationString(LocalizationKey.Map_Hell);
        mapNameLocalization[(int)MapType.IceVillage] = ReturnLocalizationString(LocalizationKey.Map_IceVillage);
    }

    public string MapNameReturn(MapType type) 
    {
        return mapNameLocalization[(int)type];
    }

    public string MapNameReturn(int index) 
    {
        return mapNameLocalization[index];
    }

    #endregion
}


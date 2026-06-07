using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
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

[Serializable]
public class LocalizationWrapper
{
    public List<LocalizationClass> rows;
}


public class LocalizationManager : Singleton<LocalizationManager>
{
    [SerializeField]
    private LanguageType currLanguateType;
    [SerializeField]
    private Dictionary<LanguageType, Language> languages;

    private Action<LanguageType> ChangeLanguageAction;

    protected override void Singleton_Awake()
    {
        SetUpDontDestroy();

        SetLanguageType();

        languages = new Dictionary<LanguageType, Language>();

        // fallBack 로컬라이제이션 테이블 사용
        FallBackLocalization();
    }

    private void Start()
    {
        // 씬 변경 시 로컬라이징 ( 게임 -> 로비 씬 돌아올 때 )
        PhotonSceneManager.Instance.RegisterAction(SceneType.Lobby, ChangeLanguageType);
    }

    private void FallBackLocalization() 
    {
        // 1. 텍스트파일 가져오기
        TextAsset text = ResourceManager.Instance.FallBackLocalizationText;

        // 2. LitJson 파싱
        string jsonData = text.text;

        // 3. 클래스 생성
        FallbackJsonParse(jsonData);
    }

    // fallback json 바탕 클래스 생성
    private void FallbackJsonParse(string json) 
    {
        LocalizationWrapper list = JsonConvert.DeserializeObject<LocalizationWrapper>(json);

        for (int i = 0; i < list.rows.Count; i++) 
        {
            LocalizationClass temp = list.rows[i];
            string key = temp.key;

            // 언어만큼 for 돌기 
            for (int j = 0; j < Extension.EnumCount<LanguageType>(); j++)
            {
                LanguageType currType = Extension.GetElement<LanguageType>(j);

                LocalizationManager.Instance.AddLanguageDictionary(currType, key, temp.TypeByString(currType));
            }
        }
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
            // case SystemLanguage.Korean:
            //    currLanguateType = LanguageType.Korean; break;
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

        if (currLanguateType == LanguageType.Korean)
            currLanguateType = LanguageType.English;

        Debug.Log($"[LocalizationManager] 언어 변경 메서드 실행, 언어타입 : {currLanguateType}");
        ChangeLanguageAction?.Invoke(currLanguateType);
    }

    public void ChangeLanguageType() 
    {
        if (currLanguateType == LanguageType.Korean)
            currLanguateType = LanguageType.English;

        Debug.Log($"[LocalizationManager] 언어 변경 메서드 실행, 언어타입 : {currLanguateType}");
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
    public string MapNameReturn(MapType type) 
    {
        return ReturnLocalizationString(GetMapKey(type));
    }

    public string GetMapKey(MapType type)
    {
        switch (type)
        {
            case MapType.Forest: return LocalizationKey.Map_Forest;
            case MapType.GiganticTree: return LocalizationKey.Map_GiganticTree;
            case MapType.Market: return LocalizationKey.Map_Market;
            case MapType.Island: return LocalizationKey.Map_Island;
            case MapType.Hell: return LocalizationKey.Map_Hell;
            case MapType.IceVillage: return LocalizationKey.Map_IceVillage;
            default: return LocalizationKey.Map_DataError;
        }
    }

    #endregion
}


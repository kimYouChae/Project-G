using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingPopUP : UIPopUP
{
    [SerializeField] Slider MasterSlider;
    [SerializeField] Slider BGMSlier;
    [SerializeField] Slider SFXSlider;

    [SerializeField] TMP_Dropdown languageDropDown;
    [SerializeField] int selectLanguage;
    [SerializeField] Button applyButton;

    private Action<SoundType, float> soundValueChangedAction;
    private Action<int> applyLanguageAction;

    public void RegisterSoundValue(Action<SoundType, float> action) 
    {
        soundValueChangedAction += action;
    }

    public void RegisterLanguageApply(Action<int> action) 
    {
        applyLanguageAction += action;    
    }

    void Start()
    {
        MasterSlider.onValueChanged.AddListener(value => { soundValueChangedAction?.Invoke(SoundType.Master, value); });
        BGMSlier.onValueChanged.AddListener(value => { soundValueChangedAction?.Invoke(SoundType.BGM, value); });
        SFXSlider.onValueChanged.AddListener(value => { soundValueChangedAction?.Invoke(SoundType.SFX, value) ; });

        languageDropDown.onValueChanged.AddListener( value => selectLanguage = value);
        applyButton.onClick.AddListener( () => applyLanguageAction?.Invoke(selectLanguage) );

        // 드롭다운 관리
        languageDropDown.ClearOptions();
        InitDropDown();
    }

    private void InitDropDown() 
    {
        List<string> optionList = new List<string>();

        for (int i = 0; i < Extension.EnumCount<LanguageType>(); i++) 
        {
            LanguageType type = Extension.GetElement<LanguageType>(i);
            optionList.Add(Define.languageNames[type]);
        }

        languageDropDown.AddOptions(optionList);
    }

}

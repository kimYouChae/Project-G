using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingPopUP : UIPopUP
{
    [SerializeField] Slider MasterSlider;
    [SerializeField] Slider BGMSlier;
    [SerializeField] Slider SFXSlider;

    [SerializeField] Dropdown languageDropDown;
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
    }

}

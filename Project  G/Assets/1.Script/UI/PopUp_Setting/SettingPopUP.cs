using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingPopUP : UIPopUP , ILocalizable
{
    [SerializeField] Slider MasterSlider;
    [SerializeField] Slider BGMSlier;
    [SerializeField] Slider SFXSlider;

    [SerializeField] TMP_Dropdown languageDropDown;
    [SerializeField] int selectLanguage;
    [SerializeField] Button applyButton;

    private Action<SoundType, float> soundValueChangedAction;
    private Action<int> applyLanguageAction;

    [Header("===Localize Text===")]
    [SerializeField] TextMeshProUGUI soundText;
    [SerializeField] TextMeshProUGUI masterText;
    [SerializeField] TextMeshProUGUI sfxText;
    [SerializeField] TextMeshProUGUI bgmText;
    [SerializeField] TextMeshProUGUI languageText;

    public void OpenSetting() 
    {
        // UI ON, 팝업 사운드 실행
        base.OpenPopUP();
    }

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

        // 로컬라이징 관리
        LocalizationManager.Instance.RegisterChangeLanguage(IUpdateLocalization);
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

    public void IUpdateLocalization(LanguageType type)
    {
        soundText.text = LocalizationManager.Instance.ReturnLocalizationString(type, LocalizationKey.Setting_Sound);
        masterText.text = LocalizationManager.Instance.ReturnLocalizationString(type, LocalizationKey.Setting_Master);
        sfxText.text = LocalizationManager.Instance.ReturnLocalizationString(type, LocalizationKey.Setting_SFX);
        bgmText.text = LocalizationManager.Instance.ReturnLocalizationString(type, LocalizationKey.Setting_BGM);
        languageText.text = LocalizationManager.Instance.ReturnLocalizationString(type, LocalizationKey.Setting_Language);
    }
}

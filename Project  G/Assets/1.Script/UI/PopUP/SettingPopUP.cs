using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingPopUP : UIPopUP
{
    [Header("===Sound===")]
    [SerializeField] Slider MasterSlider;
    [SerializeField] Slider BGMSlier;
    [SerializeField] Slider SFXSlider;

    [Header("===Localization===")]
    [SerializeField] TMP_Dropdown languageDropDown;
    [SerializeField] int selectLanguage;
    [SerializeField] Button applyButton;

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

        // 사운드 슬라이드 초기화 
        MasterSlider.value = SoundManager.Instance.GetVolumeByType(SoundType.Master);
        BGMSlier.value = SoundManager.Instance.GetVolumeByType(SoundType.BGM);
        SFXSlider.value = SoundManager.Instance.GetVolumeByType(SoundType.SFX);
    }

    private void OnDisable()
    {
        // 꺼질 때 
        // 사운드 볼륨 저장
        SoundManager.Instance.SaveSoundVolume(SoundType.Master, MasterSlider.value);
        SoundManager.Instance.SaveSoundVolume(SoundType.BGM, BGMSlier.value);
        SoundManager.Instance.SaveSoundVolume(SoundType.SFX, SFXSlider.value);
    }

    void Start()
    {
        MasterSlider.onValueChanged.AddListener(value => { ChangeVolume(SoundType.Master, value); });
        BGMSlier.onValueChanged.AddListener(value => { ChangeVolume(SoundType.BGM, value); });
        SFXSlider.onValueChanged.AddListener(value => { ChangeVolume(SoundType.SFX, value); });

        languageDropDown.onValueChanged.AddListener(value => selectLanguage = value);
        applyButton.onClick.AddListener(() => ChangeLanguage(selectLanguage));

        // 드롭다운 관리
        languageDropDown.ClearOptions();
        InitDropDown();

        // 로컬라이징 관리
        LocalizationManager.Instance.RegisterChangeLanguage(IUpdateLocalization);
    }

    // 드롭다운 초기화
    private void InitDropDown()
    {
        List<string> optionList = new List<string>();

        for (int i = 0; i < Extension.EnumCount<LanguageType>(); i++)
        {
            LanguageType type = Extension.GetElement<LanguageType>(i);

            if (type == LanguageType.Korean)
                continue;

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

    // 볼륨 변경 
    private void ChangeVolume(SoundType type, float volume)
    {
        SoundManager.Instance.ChangeVolumeByType(type, volume);
    }

    // 언어 변경 
    private void ChangeLanguage(int index)
    {
        LocalizationManager.Instance.ChangeLanguageType((LanguageType)index);
    }
}

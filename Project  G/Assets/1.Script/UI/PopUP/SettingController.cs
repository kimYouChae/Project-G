using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class SettingController 
{
    private SoundModel soundModel;
    private SettingPopUP settingPopup;  // view에 해당하는 부분 

    public SettingController(SoundModel soundModel , SettingPopUP settingPopup )
    {
        this.soundModel = soundModel;
        this.settingPopup = settingPopup;

        settingPopup.RegisterSoundValue(ChangeVolume);
    }
    private void ChangeVolume(SoundType type, float volume) 
    {
        soundModel.AudioMixer.SetFloat(type.ToString(), Mathf.Log10(Mathf.Max(0.001f, volume)) * 20);
    }
}

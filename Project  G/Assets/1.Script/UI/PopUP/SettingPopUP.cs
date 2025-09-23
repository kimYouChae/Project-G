using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SettingPopUP : UIPopUP
{
    public Slider MasterSlider;
    public Slider BGMSlier;
    public Slider SFXSlider;

    private Action<SoundType, float> soundValueChangedAction;

    public void RegisterSoundValue(Action<SoundType, float> action) 
    {
        soundValueChangedAction += action;
    }

    void Start()
    {
        MasterSlider.onValueChanged.AddListener(value => { soundValueChangedAction?.Invoke(SoundType.Master, value); });
        BGMSlier.onValueChanged.AddListener(value => { soundValueChangedAction?.Invoke(SoundType.BGM, value); });
        SFXSlider.onValueChanged.AddListener(value => { soundValueChangedAction?.Invoke(SoundType.SFX, value) ; });

    }

}

using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Collections;

public class SoundManager : Singleton<SoundManager>
{
    /// <summary>
    /// sfx, bgm 공통 오디오 믹서 관리
    /// </summary>
    
    [SerializeField]
    private Transform soundTrs; // 사운드오브젝트, 하위에 SFX / BGM 오브젝트 추가 예정

    [SerializeField] 
    private AudioMixer audioMixer;  // 오디오믹스
    [SerializeField]
    private AudioMixerGroup sfxMixerGroup;  // 오디오 믹서 안 sfx 그룹
    [SerializeField] 
    private AudioMixerGroup bgmMixerGroup;  // 오디오 믹서 안 bgm 그룹

    [SerializeField]
    private SoundVolume soundVolume;
    const string SFX_MIXER_GROUP = "SFX";
    const string BGM_MIXER_GROUP = "BGM";

    public AudioMixer AudioMixer { get => audioMixer; }

    protected override void Singleton_Awake()
    {
        // 오디오믹서, 사운드 볼륨 초기화
        audioMixer = ResourceManager.Instance.GetAudioMixer;
        soundVolume = new SoundVolume();

        if (audioMixer != null)
        {
            sfxMixerGroup = audioMixer.FindMatchingGroups(SFX_MIXER_GROUP)[0];
            bgmMixerGroup = audioMixer.FindMatchingGroups(BGM_MIXER_GROUP)[0];
        }

        StartCoroutine(Temp());
    }

    IEnumerator Temp() 
    {
        yield return null;

        // 플레이어프리팹에 저장되어 있는 볼룸으로 변경 필요
        ChangeVolumeByType(SoundType.Master, soundVolume.MasterVolume);
        ChangeVolumeByType(SoundType.BGM, soundVolume.BGMVolume);
        ChangeVolumeByType(SoundType.SFX, soundVolume.SFXVolume);
    }


    public void SettingAudioMixerOutput(AudioSource source, SoundType soundType) 
    {
        // bgm 오디오 믹서그룹 설정 
        if (soundType == SoundType.BGM)
        {
            source.outputAudioMixerGroup = bgmMixerGroup;
        }
        // sfx 오디오 믹서 그룹 설정
        else if(soundType == SoundType.SFX) 
        {
            source.outputAudioMixerGroup = sfxMixerGroup;
        }
    }

    public Transform InstanceSoundObject(string trsName) 
    {
        GameObject obj = new GameObject(trsName);
        obj.name = trsName;
        obj.transform.SetParent(soundTrs);

        return obj.transform;
    }

    public void ChangeVolumeByType(SoundType type, float volume) 
    {
        // 오디오 믹서의 볼륨 바꾸기 
        audioMixer.SetFloat(type.ToString(), Mathf.Log10(Mathf.Max(0.001f, volume)) * 20);
    }

    public float GetVolumeByType(SoundType type) 
    {
        switch (type) 
        {
            case SoundType.Master: return soundVolume.MasterVolume;
            case SoundType.BGM: return soundVolume.BGMVolume;
            case SoundType.SFX: return soundVolume.SFXVolume;
            default: return 0;
        }
    }

    public void SaveSoundVolume(SoundType type, float volume) 
    {
        Debug.Log($"[SoundManager] 사운드 {type}에 대한 볼륨 저장 : {volume}");

        switch (type)
        {
            case SoundType.Master:
                soundVolume.MasterVolume = volume;
                break;
            case SoundType.BGM: 
                soundVolume.BGMVolume = volume;
                break;
            case SoundType.SFX: 
                soundVolume.SFXVolume = volume;
                break;
        }
    }
}

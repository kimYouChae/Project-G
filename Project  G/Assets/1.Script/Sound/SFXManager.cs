using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    private const float SFX_RETURN_DELAY = 0.15f;

    [Header("===Transform===")]
    private Transform sfxTrs;       // 오디오 소스 담아둘 trs

    [Header("===Container===")]
    private Dictionary<SFXType, AudioClip> typeByClip;      // 타입별 오디오 클립 
    private Dictionary<SFXType, AudioSource> typeBySource;  // 타입별 오디오 소스 
    private AudioSource commmonAudioSource;     // 공용 오디오소스 

    [Header("===SFX Policy===")]
    private SFXPolicy sfxPolicy;    // sfx 타입별 실행정책

    private void Awake()
    {
        // 정책 생성 
        sfxPolicy = new SFXPolicy();

        // 오디오 클립 가져오기
        InitAudioClip();

        // 오디오 소스 세팅 
        InitAudioSource();
    }

    private void InitAudioClip()
    {
        try 
        {
            // 리소스manager에서 클립 가져오기 
            var clips = ResourceManager.Instance.GetSFXClip;

            // 클립이름은 enum의 type과 같아야 한다.
            for (int i = 0; i < clips.Length; i++)
            {
                string name = clips[i].name;
                // name에 해당하는 type
                SFXType type = Extension.StringToEnum<SFXType>(name);

                // 딕셔너리에 저장
                typeByClip.Add(type, clips[i]);
            }
        }
        catch(Exception e) { Debug.LogError(e); }

    }

    private void InitAudioSource() 
    {
        // SFX 오브젝트 하나 만들기 (여기에 오디오소스 추가예정)
        sfxTrs = SoundManager.Instance.InstanceSoundObject(nameof(SFXManager));

        // 정책따라 오디오소스 생성
        int length = Extension.EnumCount<SFXType>();
        for(int i = 0; i < length; i++) 
        {
            SFXType sType = (SFXType)i;
            PlayPolicy policy = sfxPolicy.GetPolicy(sType);

            // 개인 오디오소스가 필요하면
            if (policy.isIndividualSoundSource)
            {
                // 오디오소스 생성후 딕셔너리에 넣기
                AudioSource indiSource = sfxTrs.AddComponent<AudioSource>();
                typeBySource.Add(sType, indiSource);

                // 오디오 믹서 세팅 
                SettingAudioMixer(indiSource);
                continue;
            }

            // 공용 오디오소스이면 
            if (commmonAudioSource != null)
                continue;

            AudioSource source = sfxTrs.AddComponent<AudioSource>();
            commmonAudioSource = source;
            SettingAudioMixer(source);
        }
    }

    private void SettingAudioMixer(AudioSource source) 
    {
        // 믹서 세팅 

    }

    public void PlaySFX(SFXType type) 
    {
        //##TODO : 실행할 때 타입에 맞는 정책을 바탕으로 실행해야됨 

        // 1. 타입에 해당하는 오디오소스 받기
        AudioSource source = GetAudioSource(type);
        if (source == null)
        {
            Debug.LogError($"Failed to Get Audio Source by type {type}");
            return;
        }

        // 2. 실행 검사 
        if (source.isPlaying)
        {
            // 실행된지 일정시간 이하이면 return
            if (source.time <= SFX_RETURN_DELAY)
                return;

            // 일정시간 넘었으면 stop
            source.Stop();
        }

        // 3. 실행 
        source.Play();
    }

    // type에 해당하는 오디오소스 return
    private AudioSource GetAudioSource(SFXType type)
    {
        if (typeBySource.ContainsKey(type))
            return typeBySource[type];

        return null;
    }
}

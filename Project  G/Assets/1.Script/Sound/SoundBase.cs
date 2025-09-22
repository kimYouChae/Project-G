using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// SFX, BGM 타입을 가질 수 있음
/// </summary>
/// <typeparam name="T"></typeparam>
public class SoundBase<T> : MonoBehaviour
    where T : Enum
{
    [SerializeField] Transform sourceTransform; // SFX, BGM을 실행시킬 trs
    [SerializeField] private AudioMixer mixerGroup;

    protected Dictionary<T, AudioSource> typeBySource;
    protected Dictionary<T, AudioClip> typeByClip;

    private async void Awake()
    {
        typeBySource = new Dictionary<T, AudioSource>();
        typeByClip = new Dictionary<T, AudioClip>();

        await InitAudioClip();
        await InitAudioSource();
    }

    // 초기화 - ReSource에서 가져오기 - 하위에서 구현 
    protected virtual async Task InitAudioClip() 
    {
        //Debug.Log($"1.{GetType().Name} 오디오 소스 초기화중");

    }

    private async Task InitAudioSource() 
    {
        Transform trs = new GameObject(GetType().Name).GetComponent<Transform>();
        trs.parent = this.transform;

        sourceTransform = trs;

        //Debug.Log($"2. {GetType().Name}오디오소스 초기화중");
        // clip만큼 오디오 소스 추가하기 
        foreach (var temp in typeByClip) 
        {
            AudioSource source = trs.AddComponent<AudioSource>();

            source.clip = temp.Value;

            T type = Extension.StringToEnum<T>(temp.Value.name);
            typeBySource.Add(type, source);
        }
    }

    // type에 해당하는 오디오소스 return
    protected AudioSource GetAudioSource(T type) 
    { 
        if(typeBySource.ContainsKey(type))
            return typeBySource[type];

        return null;
    }
}

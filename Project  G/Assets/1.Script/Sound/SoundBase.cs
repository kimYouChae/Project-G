using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// SFX, BGM 타입을 가질 수 있음
/// </summary>
/// <typeparam name="T"></typeparam>
public class SoundBase<T> : MonoBehaviour
{
    [SerializeField] private int intPoolSize;

    [SerializeField] Transform sourceTransform; // SFX, BGM을 실행시킬 trs
    [SerializeField] private AudioMixer mixerGroup;

    protected Dictionary<T, AudioSource> typeBySource;
    protected Dictionary<T, AudioClip> typeByClip;

    private void Awake()
    {
        typeBySource = new Dictionary<T, AudioSource>();
        typeByClip = new Dictionary<T, AudioClip>();

        InitAudioClip();
    }

    // 초기화 - ReSource에서 가져오기 - 하위에서 구현 
    protected virtual void InitAudioClip() {}

    private void InitAudioSource() 
    { 
        
    }
}

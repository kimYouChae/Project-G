using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    [Header("===BGM Setting===")]
    [SerializeField] private AudioSource nowAudioSource;    // 현재 오디오소스
    [SerializeField] BGMType currBGMType;                   // 현재 bgm 타입

    [Header("===Container===")]
    protected Dictionary<BGMType, AudioSource> typeBySource;
    protected Dictionary<BGMType, AudioClip> typeByClip;

    // 페이드인 , 아웃 코루틴
    private Coroutine fadeInCorutine;
    private Coroutine fadeOutCorutine;

    private const float DEFAULT_FADE_TIME = 0.5f;
    private const bool DEFAULT_LOOP = true;

    protected async Task InitAudioClip()
    {
        try
        {
            // 리소스manager에서 클립 가져오기 
            var clips = ResourceManager.Instance.GetBGMClip;

            // 클립이름은 enum의 type과 같아야 한다.
            for (int i = 0; i < clips.Length; i++)
            {
                string name = clips[i].name;
                // name에 해당하는 type
                BGMType type = Extension.StringToEnum<BGMType>(name);

                // 딕셔너리에 저장
                typeByClip.Add(type, clips[i]);
            }
        }
        catch (Exception e) { Debug.LogError(e); }
    }

    public void PlayBGM(BGMType bgmType)
    {

        // 0. 현재 실행하고 있는 사운드가 있으면 페이드아웃
        if (nowAudioSource != null && nowAudioSource.clip != null)
        {
            if (fadeOutCorutine != null)
                StopCoroutine(fadeOutCorutine);

            fadeOutCorutine = StartCoroutine(FadeOut(nowAudioSource, DEFAULT_FADE_TIME));
        }

        // 1-1. 세팅
        nowAudioSource.loop = DEFAULT_LOOP;
        nowAudioSource.spatialBlend = 0f; // 2D 사운드

        // 2. 바꿀 bgm 페이드인
        if (fadeInCorutine != null)
        {
            StopCoroutine(fadeInCorutine);
        }
        fadeInCorutine = StartCoroutine(FadeIn(nowAudioSource, DEFAULT_FADE_TIME));
    }

    IEnumerator FadeIn(AudioSource audioSource, float fadeTime)
    {
        // 일정시간 대기 (전 브금이 완전히 꺼질때까지 대기)
        if (fadeTime > 0)
            yield return new WaitForSeconds(fadeTime * 0.5f);

        // 페이드인 
        // 소리 : 0부터 1까지
        audioSource.volume = 0;

        // 실행 후 fadeIn
        audioSource.Play();

        float startVolume = 0;
        float targetVolume = 1f;
        float currTime = 0;

        while (true)
        {
            if (currTime >= fadeTime)
                break;

            // 진행시간 +
            currTime += Time.deltaTime;

            // 소리조정 
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currTime / fadeTime);

            // 한프레임 대기 
            yield return null;
        }

        // 정확한 볼륨으로 설정
        audioSource.volume = targetVolume;
        // 코루틴이 끝나면 null
        fadeInCorutine = null;
    }

    IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
    {
        // 실행하고 있는 오디오가 없으면
        if (!audioSource.isPlaying)
            yield break;

        // 페이드아웃
        // 소리 : 1부터 0까지 
        float startVolume = audioSource.volume;
        float targetVolume = 0f;
        float currTime = 0;

        while (true)
        {
            if (currTime >= fadeTime)
                break;

            // 진행시간 +
            currTime += Time.deltaTime;

            // 소리조정 
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currTime / fadeTime);

            // 한프레임 대기 
            yield return null;
        }

        // 페이드 아웃이 완료되면 오디오 재생을 중지
        audioSource.Stop();

        // 코루틴이 끝나면 null
        fadeOutCorutine = null;
    }

}

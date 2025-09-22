using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SFXManager : SoundBase<SFXType>
{
    private const float SFX_RETURN_DELAY = 0.15f;

    protected override async Task InitAudioClip()
    {
        await base.InitAudioClip();

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

    public void PlaySFX(SFXType type) 
    {
        // 1. 타입에 해당하는 오디오소스 받기
        AudioSource source = base.GetAudioSource(type);
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
}

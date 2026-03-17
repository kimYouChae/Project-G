using UnityEngine;
using UnityEngine.Audio;

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

    const string SFX_MIXER_GROUP = "SFX";
    const string BGM_MIXER_GROUP = "BGM";

    protected override void Singleton_Awake()
    {
        // 오디오 믹서 가져오기 
        audioMixer = ResourceManager.Instance.GetAudioMixer;
        if (audioMixer != null)
        {
            sfxMixerGroup = audioMixer.FindMatchingGroups(SFX_MIXER_GROUP)[0];
            bgmMixerGroup = audioMixer.FindMatchingGroups(BGM_MIXER_GROUP)[0];
        }
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
}

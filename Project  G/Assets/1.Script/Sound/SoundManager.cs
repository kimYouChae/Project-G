using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    protected override void Singleton_Awake()
    {

    }

    /*
    private async Task InitAudioSource()
    {
        Transform trs = new GameObject(GetType().Name).GetComponent<Transform>();
        trs.parent = this.transform;

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
    */

}

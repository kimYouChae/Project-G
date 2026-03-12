using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField]
    private Transform soundTrs; // 사운드오브젝트, 하위에 SFX / BGM 오브젝트 추가 예정

    protected override void Singleton_Awake()
    {

    }

    public Transform InstanceSoundObject(string trsName) 
    {
        GameObject obj = new GameObject(trsName);
        obj.name = trsName;
        obj.transform.SetParent(soundTrs);

        return obj.transform;
    }
}

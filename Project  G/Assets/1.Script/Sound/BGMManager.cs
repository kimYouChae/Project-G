using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : SoundBase<BGMType>
{
    protected override void InitAudioClip()
    {
        var clips = ResourceManager.Instance.GetBGMClip;

        // 클립이름은 enum의 type과 같아야 한다.
        for(int i = 0; i < clips.Length; i++) 
        {
            string name = clips[i].name;
            // name에 해당하는 type
            BGMType type = Extension.StringToEnum<BGMType>(name);

            // 딕셔너리에 저장
            typeByClip.Add(type, clips[i]);
        }
    }
}

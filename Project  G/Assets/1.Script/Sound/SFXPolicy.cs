using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayPolicy 
{
    // 개인 Sound Source 를 사용하는지 
    // or 공통 Sound Source 사용 ( play on shot 방식 )
    public bool isIndividualSoundSource;

    // 동기화 되는지
    // or 로컬에서만 실행되는지 ( 비동기 )
    public bool isSync;

    public PlayPolicy(bool isindivi, bool isSyn)
    {
        this.isIndividualSoundSource = isindivi;
        this.isSync = isSyn;
    }
}

public class SFXPolicy 
{
    /// <summary>
    /// SFX 규칙에 대해서 정리
    /// 1. 개인 soundSource를 사용하는지 
    /// 2. 동기화 되는지
    /// </summary>
    
    private Dictionary<SFXType, PlayPolicy> sfxPolicyDic;

    public SFXPolicy() 
    {
        sfxPolicyDic = new Dictionary<SFXType, PlayPolicy>();

        // UI
        sfxPolicyDic.Add(SFXType.UIClick, new PlayPolicy(false, false));
        sfxPolicyDic.Add(SFXType.UIBack, new PlayPolicy(false, false));
        sfxPolicyDic.Add(SFXType.UIPopup, new PlayPolicy(false, false));
        sfxPolicyDic.Add(SFXType.UIWarning, new PlayPolicy(false, false));

        // InGame
        sfxPolicyDic.Add(SFXType.Countdown, new PlayPolicy(false, false));
        sfxPolicyDic.Add(SFXType.GameStart, new PlayPolicy(false, false));

        // Character
        sfxPolicyDic.Add(SFXType.CharacterFootstep, new PlayPolicy(true, false));
        sfxPolicyDic.Add(SFXType.CharacterDeath, new PlayPolicy(false, true));
        sfxPolicyDic.Add(SFXType.ShieldBlock, new PlayPolicy(false, false));
        sfxPolicyDic.Add(SFXType.ScoreCoin, new PlayPolicy(false, false));
        sfxPolicyDic.Add(SFXType.InvincibleSkill, new PlayPolicy(false, false));

        // Spawner
        sfxPolicyDic.Add(SFXType.BulletSpawnerShot, new PlayPolicy(false, false));
        sfxPolicyDic.Add(SFXType.FourBulletObjPut, new PlayPolicy(false, false));
        sfxPolicyDic.Add(SFXType.MissileSpawnerShot, new PlayPolicy(false, false));
        sfxPolicyDic.Add(SFXType.MissileFly, new PlayPolicy(true, false));
        sfxPolicyDic.Add(SFXType.MissileExplosion, new PlayPolicy(false, false));
        sfxPolicyDic.Add(SFXType.LaserCharge, new PlayPolicy(true, true));
        sfxPolicyDic.Add(SFXType.LaserFire, new PlayPolicy(true, true));
    }

    public PlayPolicy GetPolicy(SFXType type)
    {
        if (sfxPolicyDic.TryGetValue(type, out PlayPolicy policy))
        {
            return policy;
        }

        Debug.LogWarning($"SFXPolicy not found for {type}");
        return null;
    }
}

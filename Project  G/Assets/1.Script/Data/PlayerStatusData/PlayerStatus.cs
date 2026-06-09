using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[System.Serializable]
public class PlayerStatusData
{
    // characterType,moveSpeed,scoreAmount,scoreCooltime,shieldAngle,colorChangeInterval
    [SerializeField] private CharacterType characterType;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float scoreAmount;
    [SerializeField] private float scoreCooltime;
    [SerializeField] private float shieldAngle;
    [SerializeField] private float colorChangeInterval;

    #region 맴버가 있는 생성자

    public PlayerStatusData(CharacterType characterType, float moveSpeed, float scoreAmount, float scoreCooltime, float shieldAngle, float colorChangeInterval)
    {
        this.characterType = characterType;
        this.moveSpeed = moveSpeed;
        this.scoreAmount = scoreAmount;
        this.scoreCooltime = scoreCooltime;
        this.shieldAngle = shieldAngle;
        this.colorChangeInterval = colorChangeInterval;
    }
    #endregion

    public CharacterType CharacterType { get => characterType;  }
    public float MoveSpeed { get => moveSpeed;  }
    public float ScoreAmount { get => scoreAmount; }
    public float ScoreCooltime { get => scoreCooltime; }
    public float ShieldAngle { get => shieldAngle; }
    public float ColorChangeInterval { get => colorChangeInterval; }
}

public static class PlayerStatus
{
    private static Dictionary<CharacterType, PlayerStatusData> keyValuePairs;

    public static void SetPlayerStatusData(PlayerStatusData data)
    {
        // 딕셔너리에 타입별 config 데이터 저장 
        if (keyValuePairs == null)
            keyValuePairs = new Dictionary<CharacterType, PlayerStatusData>();

        if(!keyValuePairs.ContainsKey(data.CharacterType))
        {
            keyValuePairs.Add(data.CharacterType, data);
            // Debug.Log($"[PlayerStatus] 캐릭터 타입 : {data.CharacterType} / 속도 : {data.MoveSpeed}");
        }
    }

    public static PlayerStatusData GetPlayerData(CharacterType type) 
    {
        if (keyValuePairs.ContainsKey(type))
            return keyValuePairs[type];

        return null;
    }

    public static float GetPlayerSpeed() 
    {
        if (keyValuePairs.ContainsKey(CharacterType.BasicCharacter))
            return keyValuePairs[CharacterType.BasicCharacter].MoveSpeed;

        return 1;
    }
}

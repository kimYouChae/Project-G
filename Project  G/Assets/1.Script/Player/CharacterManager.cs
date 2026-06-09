using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    [SerializeField] private string charaterName;
    [SerializeField] private CharacterType characterType;
    [SerializeField] private string characterToolTip;
    [SerializeField] private AchieveType achieveType;      // 달성해야할 도전과제 타입

    #region 멤버가 있는 생성자
    public CharacterData(string charaterName, CharacterType characterType, string characterToolTip, AchieveType achieveType)
    {
        this.charaterName = charaterName;
        this.characterType = characterType;
        this.characterToolTip = characterToolTip;
        this.achieveType = achieveType;
    }
    #endregion

    public string CharaterName { get => charaterName; }
    public string CharacterToolTip { get => characterToolTip;}
    public AchieveType AchieveType { get => achieveType;}
    public CharacterType CharacterType { get => characterType;}
}

public class CharacterManager : Singleton<CharacterManager>
{
    [SerializeField]
    private List<CharacterData> characterData;

    private Dictionary<CharacterType, CharacterData> typeByCharacterData;

    public List<CharacterData> CharacterData { get => characterData;}

    protected override void Singleton_Awake()
    {
        characterData = new List<CharacterData>();
        typeByCharacterData = new Dictionary<CharacterType, CharacterData>();
    }

    public void AddtoCharacterContainer(CharacterData data) 
    {
        characterData.Add(data);
        if (!typeByCharacterData.ContainsKey(data.CharacterType)) 
        {
            typeByCharacterData.Add(data.CharacterType, data);
        }
    }

    public CharacterData TypeByCharacterData(CharacterType type) 
    {
        return typeByCharacterData[type];
    }
}

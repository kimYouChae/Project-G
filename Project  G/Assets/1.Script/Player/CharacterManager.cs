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

    public CharacterData(string name, CharacterType type, string tooptip , AchieveType aType)
    {
        this.charaterName = name;
        this.characterType = type;
        this.characterToolTip = tooptip;
        this.achieveType = aType;
    }

    public string CharaterName { get => charaterName; }
    public string CharacterToolTip { get => characterToolTip; set => characterToolTip = value; }
    public AchieveType AchieveType { get => achieveType; set => achieveType = value; }
    public CharacterType CharacterType { get => characterType; set => characterType = value; }
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

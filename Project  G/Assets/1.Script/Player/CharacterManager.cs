using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    [SerializeField] private string charaterName;
    [SerializeField] private CharacterType characterType;
    [SerializeField] private string characterToolTip;
    [SerializeField] private AchiveType achiveType;      // 달성해야할 도전과제 타입 

    public CharacterData(string name, CharacterType type, string tooptip , AchiveType aType) 
    {
        this.charaterName = name;
        this.characterType = type;
        this.characterToolTip = tooptip;
        this.achiveType = aType;
    }

    public string CharaterName { get => charaterName; }
    public string CharacterToolTip { get => characterToolTip; set => characterToolTip = value; }
    public AchiveType AchiveType { get => achiveType; set => achiveType = value; }
    public CharacterType CharacterType { get => characterType; set => characterType = value; }
}

public class CharacterManager : Singleton<CharacterManager>
{
    [SerializeField]
    private List<CharacterData> characterData;

    private Dictionary<CharacterType, CharacterData> typeByCharacterData;

    [Header("===현재 선택 캐릭터 타입===")]
    private CharacterType characterType;

    public List<CharacterData> CharacterData { get => characterData;}

    protected override void Singleton_Awake()
    {

    }

    private void Start()
    {
        // 임시 데이터 
        characterData = new List<CharacterData>();
        typeByCharacterData = new Dictionary<CharacterType, CharacterData>();

        CharacterData ch1 = new CharacterData("캐릭터1", CharacterType.BasicCharacter, "임시:베이직캐릭터입니다", AchiveType.None);
        CharacterData ch2 = new CharacterData("캐릭터2", CharacterType.ShieldCharacter, "임시:쉴드 캐릭터", AchiveType.Stage_Forest);
        CharacterData ch3 = new CharacterData("캐릭터3", CharacterType.ScoreCharacter, "임시:점수 캐릭터", AchiveType.Stage_GiganticTree);
        CharacterData ch4 = new CharacterData("캐릭터4", CharacterType.InvincibleCharacter, "임시:1회 무적 캐릭터", AchiveType.Stage_Island);

        characterData.Add(ch1);
        characterData.Add(ch2);
        characterData.Add(ch3);
        characterData.Add(ch4);

        typeByCharacterData.Add(CharacterType.BasicCharacter, ch1);
        typeByCharacterData.Add(CharacterType.ShieldCharacter, ch2);
        typeByCharacterData.Add(CharacterType.ScoreCharacter, ch3);
        typeByCharacterData.Add(CharacterType.InvincibleCharacter, ch4);

    }

    public void InitCharacterSelectIndex(CharacterType type) 
    {
        this.characterType = type;
    }

    public CharacterData TypeByCharacterData(CharacterType type) 
    {
        return typeByCharacterData[type];
    }
}

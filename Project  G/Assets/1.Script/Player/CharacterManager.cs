using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData
{
    private string charaterName;
    private CharacterType characterType;
    private string characterToolTip;
    private AchiveType achiveType;      // 달성해야할 도전과제 타입 

    public CharacterData(string name, CharacterType type, string tooptip , AchiveType aType) 
    {
        this.charaterName = name;
        this.characterType = type;
        this.characterToolTip = tooptip;
        this.achiveType = aType;
    }

    public string CharaterName { get => charaterName; }
}

public class CharacterManager : Singleton<CharacterManager>
{
    [SerializeField]
    private List<CharacterData> characterData;

    public List<CharacterData> CharacterData { get => characterData;}

    protected override void Singleton_Awake()
    {

    }

    private void Start()
    {
        // 임시 데이터 
        characterData = new List<CharacterData>();

        CharacterData ch1 = new CharacterData("캐릭터1", CharacterType.BasicCharacter, "임시:베이직캐릭터입니다", AchiveType.None);
        CharacterData ch2 = new CharacterData("캐릭터2", CharacterType.ShieldCharacter, "임시:쉴드 캐릭터", AchiveType.Stage_Forest);
        CharacterData ch3 = new CharacterData("캐릭터3", CharacterType.ScoreCharacter, "임시:점수 캐릭터", AchiveType.Stage_GiganticTree);
        CharacterData ch4 = new CharacterData("캐릭터4", CharacterType.InvincibleCharacter, "임시:1회 무적 캐릭터", AchiveType.Stage_Island);

        characterData.Add(ch1);
        characterData.Add(ch2);
        characterData.Add(ch3);
        characterData.Add(ch4);
    }
}

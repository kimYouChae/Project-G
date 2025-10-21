using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectPopUP : UIPopUP
{
    /// <summary>
    /// 스크립트 내용이 적어서 MVC로 나누지 않음 ! 
    /// </summary>

    [Header("===CharacterSelectPopUP===")]
    [SerializeField] GameObject characterPrefab;
    [SerializeField] GameObject contenct;   // 스크롤뷰 콘텐츠
    [SerializeField] List<GameObject> characterObjs;

    [Space]
    [SerializeField] CharacterType selectCharacterType;

    [Header("===Datail===")]
    [SerializeField] TextMeshProUGUI characterTitle;
    [SerializeField] TextMeshProUGUI characterToolTip;
    [SerializeField] TextMeshProUGUI cantSelectText;    // 미션 수행 전 텍스트
    [SerializeField] Button selectButton;      // 미션 수행 후 선택 버튼

    private void Start()
    {
        selectButton.onClick.AddListener(() => SelectCharacterButton());
    }

    // 켤 때 초기화
    public void InitCharacterView()
    {
        // 디테일 창 - 기본 캐릭터로 초기화
        UpdateDetailUi(CharacterType.BasicCharacter);

        if (characterObjs.Count > 0) { return; }

        for (int i = 0; i < CharacterManager.Instance.CharacterData.Count; i++)
        {
            GameObject ch = Instantiate(characterPrefab);
            ch.transform.SetParent(contenct.transform, false);

            // 캐릭터 오브젝트 세팅 (데이터 순서대로)
            CharacterData characterData = CharacterManager.Instance.CharacterData[i];
            ch.GetComponent<CharacterObject>().Init(characterData.CharaterName, characterData.CharacterType, SelectCharacterObj);
        }
    }

    private void SelectCharacterObj(CharacterType type)
    {
        Debug.Log($"{type} 캐릭터 선택  ");

        selectCharacterType = type;

        // 디테일 창에 캐릭터 정보 표시
        UpdateDetailUi(type);
    }

    private void UpdateDetailUi(CharacterType type)
    {
        CharacterData data = CharacterManager.Instance.TypeByCharacterData(type);

        characterTitle.text = data.CharaterName;
        characterToolTip.text = data.CharacterToolTip;

        // 달성여부
        bool isAchive = true;
        Achievement achiv = null;
        if (data.AchiveType != AchiveType.None) 
        {
            achiv = AchievementsManager.Instance.GetAchiveByType(data.AchiveType);
            isAchive = achiv.IIsComplete();
        }

        if (isAchive)
        {
            // 달성 시 -> 캐릭터 선택 버튼 ON
            cantSelectText.gameObject.SetActive(false);
            selectButton.gameObject.SetActive(true);
        }
        else
        {
            // 미달성시 -> 캐릭터 선택 버튼 Off, progress 텍스트 띄우기
            cantSelectText.gameObject.SetActive(true);
            selectButton.gameObject.SetActive(false);
            cantSelectText.text = achiv.IProgressText();
        }
    }

    private void SelectCharacterButton() 
    {
        CharacterManager.Instance.InitCharacterSelectIndex(selectCharacterType);
    }
}

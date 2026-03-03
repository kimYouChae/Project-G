using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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
    [SerializeField] List<CharacterObject> characterObjs;

    [Space]
    [SerializeField] CharacterType selectCharacterType;

    [Header("===Datail===")]
    [SerializeField] TextMeshProUGUI characterTitle;
    [SerializeField] TextMeshProUGUI characterToolTip;
    [SerializeField] Image characterImage;
    [SerializeField] TextMeshProUGUI cantSelectText;    // 미션 수행 전 텍스트
    [SerializeField] Button selectButton;      // 미션 수행 후 선택 버튼
    [SerializeField] TextMeshProUGUI selectButtonText;  // 선택 버튼 텍스트 

    [Header("===Localization===")]
    [SerializeField] TextMeshProUGUI characterPopUpTitle;

    private void Start()
    {
        selectButton.onClick.AddListener(() => SelectCharacterButton());
    }

    private void InstantiateCharacterObj() 
    {
        for (int i = 0; i < CharacterManager.Instance.CharacterData.Count; i++)
        {
            GameObject ch = Instantiate(characterPrefab);
            ch.transform.SetParent(contenct.transform, false);
            characterObjs.Add(ch.GetComponent<CharacterObject>());
        }
    }

    // 켤 때 초기화
    public void InitCharacterView(bool hasOpend)
    {
        // 디테일 창 - 기본 캐릭터로 초기화
        UpdateDetailUi(CharacterType.BasicCharacter);

        // 타이틀 로컬라이징
        characterPopUpTitle.text = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.Character);

        // 도전과제 정보 가져오기
        StartCoroutine(GetInfo(hasOpend));
    }

    private IEnumerator GetInfo(bool hasOpend)
    {
        List<AchiveProgressResponse> achivelist;

        // 한번도 UI를 안켰을 때만 
        if (!hasOpend)
        {
            // API 호출 필요 
            yield return StartCoroutine(
                GameServices.Instance.UserProgressService.GetAchivementService(UserDataManager.Instance.SteamID));
        }

        // GamdService의 도전과제모델에서 가져오기 
        achivelist = GameServices.Instance.AchiveProgressModel.GetBestScoreInfo();

        // UI 업데이트 
        UpdateCharaterPopup(achivelist);
    }

    private void UpdateCharaterPopup(List<AchiveProgressResponse> achives) 
    {
        // 리스트에 없으면 -> 1회 오픈, 새로 생성
        /*
        if (characterObjs.Count <= 0)
        {
            InstantiateCharacterObj();
        }

        // 로컬라이징 업데이트 
        for (int i = 0; i < CharacterManager.Instance.CharacterData.Count; i++)
        {
            // 캐릭터 오브젝트 세팅 (데이터 순서대로)
            CharacterData characterData = CharacterManager.Instance.CharacterData[i];
            CharacterType characterType = characterData.CharacterType;
            characterObjs[i].Init(LocalizationManager.Instance.ReturnLocalizationString(characterType.ToString() + "_Name"),
                characterType,
                ResourceManager.Instance.CharacterSprite(characterType),
                SelectCharacterObj);
        }
        */
    }

    private void SelectCharacterObj(CharacterType type)
    {
        // Debug.Log($"{type} 캐릭터 선택  ");

        selectCharacterType = type;

        // 디테일 창에 캐릭터 정보 표시
        UpdateDetailUi(type);
    }

    private void UpdateDetailUi(CharacterType type)
    {
        CharacterData data = CharacterManager.Instance.TypeByCharacterData(type);

        characterTitle.text = LocalizationManager.Instance.ReturnLocalizationString(type.ToString() + "_Name");
        characterToolTip.text = LocalizationManager.Instance.ReturnLocalizationString(type.ToString() + "_ToolTip");
        characterImage.sprite = ResourceManager.Instance.CharacterSprite(data.CharacterType);

        // 달성여부
        bool isAchive = true;
        // Achievement achiv = null;
        if (data.AchiveType != AchiveType.None) 
        {
            // achiv = AchievementsManager.Instance.GetAchiveByType(data.AchiveType);
            // isAchive = achiv.IIsComplete();
        }

        if (isAchive)
        {
            // 달성 시 -> 캐릭터 선택 버튼 ON
            cantSelectText.gameObject.SetActive(false);
            selectButton.gameObject.SetActive(true);
            selectButtonText.text = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.Select);
        }
        else
        {
            // 미달성시 -> 캐릭터 선택 버튼 Off, progress 텍스트 띄우기
            cantSelectText.gameObject.SetActive(true);
            selectButton.gameObject.SetActive(false);
            // cantSelectText.text = achiv.IProgressText();
        }
    }

    private void SelectCharacterButton() 
    {
        // UserDataManager에 저장하기 
        UserDataManager.Instance.CharacterType = selectCharacterType;
    }
}

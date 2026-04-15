using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] Image characterImage;          // 캐릭터 이미지
    [SerializeField] TextMeshProUGUI characterTitle;    // 캐릭터 이름
    [SerializeField] TextMeshProUGUI characterToolTip;  // 캐릭터 툴팁
    [SerializeField] TextMeshProUGUI cantSelectText;    // 미션 수행 전 텍스트
    [SerializeField] Button selectButton;      // 미션 수행 후 선택 버튼
    [SerializeField] TextMeshProUGUI selectButtonText;  // 선택 버튼 텍스트 
    [SerializeField] TextMeshProUGUI currAchivementTitle;   // 도전과제 텍스트 

    [Header("===Localization===")]
    [SerializeField] TextMeshProUGUI characterPopUpTitle;

    private void Start()
    {
        selectButton.onClick.AddListener(() => SelectCharacterButton());
    }

    // 켤 때 초기화
    public void OpenCharacterView(bool hasOpend)
    {
        // UI ON, 팝업 사운드 실행
        base.OpenPopUP();

        // 로컬라이징
        // 버튼-선택 로컬라이징 : 그냥 팝업 켤 때 1회 하면 될듯 ? 껏다켯다만 하니까 
        characterPopUpTitle.text = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.Character);
        selectButtonText.text = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.Select);

        // 도전과제 정보 가져오기
        StartCoroutine(GetInfo(hasOpend));
    }

    private IEnumerator GetInfo(bool hasOpend)
    {
        // 한번도 UI를 안켰을 때만 
        if (!hasOpend)
        {
            // API 호출 필요 
            yield return StartCoroutine(
                GameServices.Instance.UserProgressService.GetAchivementService(UserDataManager.Instance.SteamID));
        }

        // UI 업데이트 
        UpdateCharaterPopup();

        // 디테일 창 - 기본 캐릭터로 초기화
        UpdateDetailUi(CharacterType.BasicCharacter);
    }

    private void UpdateCharaterPopup() 
    {
        var characterList = CharacterManager.Instance.CharacterData;

        // 리스트에 없으면 -> 1회 오픈, 새로 생성
        if (characterObjs.Count <= 0)
        {
            InstantiateCharacterObj(characterList.Count);
        }

        // 로컬라이징 업데이트 
        for (int i = 0; i < characterList.Count; i++)
        {
            CharacterData characterData = characterList[i];
            CharacterType characterType = characterData.CharacterType;

            characterObjs[i].Init(LocalizationManager.Instance.ReturnLocalizationString(characterType.ToString() + "_Name"),
                characterType,
                ResourceManager.Instance.CharacterSprite(characterType),
                SelectCharacterObj);
        }
        
    }

    private void InstantiateCharacterObj(int characterCnt)
    {
        for (int i = 0; i < characterCnt; i++)
        {
            GameObject ch = Instantiate(characterPrefab);
            ch.transform.SetParent(contenct.transform, false);
            characterObjs.Add(ch.GetComponent<CharacterObject>());
        }
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
        UpdatBaseDetailUI(data);


        // 기본 캐릭터는 도전과제 None
        if (data.AchiveType == AchiveType.None) 
        {
            SetUnLockUi();

            return;
        }

        // 캐릭터타입에 해당하는 , 해금해야하는 도전과제 가져오기 
        AchiveProgressResponse achiveResponse 
            = GameServices.Instance.AchiveProgressModel.GetAchiveProgress(data.AchiveType);


        // 클리어했으면
        if (achiveResponse.isClear)
        {
            SetUnLockUi();

            return;
        }

        // 미달성시
        // 타입에 해당하는 도전과제 데이터 가져오기
        StageAchive achiveData = AchievDataManager.Instance.GetAchiveByType(achiveResponse.AchiveType);

        SetLockUi();

        // 도전과제 타이틀 로컬라이징 
        // 후 도전과제 이름 + 진행사항 표시
        cantSelectText.text = LocalizationAchiveTitle(data.AchiveType) + "\n" + UserDataManager.Instance.ReturnUserStage(achiveData.MapType)
            + " / " + achiveData.AchiveStage;
    }

    private string LocalizationAchiveTitle(AchiveType type) 
    {
        string key = type.ToString() + "_Title";
        return LocalizationManager.Instance.ReturnLocalizationString(key);
    }

    private void SelectCharacterButton() 
    {
        // UserDataManager에 저장하기 
        UserDataManager.Instance.CharacterType = selectCharacterType;

    }

    private void UpdatBaseDetailUI(CharacterData data) 
    {
        // 캐릭터 이름, 툴팁 로컬라이징
        characterTitle.text = LocalizationManager.Instance.ReturnLocalizationString(data.CharacterType.ToString() + "_Name");
        characterToolTip.text = LocalizationManager.Instance.ReturnLocalizationString(data.CharacterType.ToString() + "_ToolTip");
        // 캐릭터 이미지 설정
        characterImage.sprite = ResourceManager.Instance.CharacterSprite(data.CharacterType);
    }

    private void SetLockUi() 
    {
        // 캐릭터 선택 버튼 Off, progress 텍스트 띄우기
        cantSelectText.gameObject.SetActive(true);
        selectButton.gameObject.SetActive(false);
    }

    private void SetUnLockUi() 
    {
        // 달성 시 -> 캐릭터 선택 버튼 ON
        cantSelectText.gameObject.SetActive(false);
        selectButton.gameObject.SetActive(true);
    }
}

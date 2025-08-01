using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class LobbyUIManager : MonoBehaviour
{
    [Space]
    [Header("===CharacterSelect===")]
    [SerializeField]
    private Button selectButton;
    [SerializeField]
    private GameObject characterUiPrefab;
    [SerializeField]
    private Transform characterContent;    // 생성된 프리팹 부모
    [SerializeField]
    private int currSelectCharacter;        // 선택한 캐릭터 인덱스 

    private void InitCharacterSelectUI()
    {
        CreateCharacter();

        // 시작 버튼 
        if (selectButton != null)
            selectButton.onClick.AddListener(() => 
            {
                // 캐릭터 세팅 
                UserDataManager.GetInstance().SetCharacterIndex(currSelectCharacter);
                // 캐릭터 생성
                UserDataManager.GetInstance().InsertToUserTable();
                // panel 전환
                ChangePanel(LobbyPanelType.CharacterSelect , LobbyPanelType.Lobby);
                // 로비UI 세팅
                SettingProfile();
            });
    }

    public void SettingCharacterIndex(int idx) 
    { 
        this.currSelectCharacter = idx;
    }

    // 동적으로 생성
    private void CreateCharacter() 
    {
        for(int i = 0; i < characterSprite.Length; i++) 
        {
            GameObject temp = Instantiate(characterUiPrefab);
            temp.transform.SetParent(characterContent);

            temp.GetComponent<Image>().sprite = characterSprite[i];

            temp.GetComponent<CharacterObject>().CharacterIndex = i;
        }
    }
}

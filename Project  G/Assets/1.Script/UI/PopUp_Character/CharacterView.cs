using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterView : MonoBehaviour
{
    [SerializeField] GameObject characterPrefab;
    [SerializeField] GameObject contenct;   // 스크롤뷰 콘텐츠
    [SerializeField] List<GameObject> characterObjs;

    [Header("===Datail===")]
    [SerializeField] TextMeshProUGUI characterTitle;
    [SerializeField] TextMeshProUGUI characterToolTip;
    [SerializeField] TextMeshProUGUI cantSelectText;    // 미션 수행 전 텍스트
    [SerializeField] TextMeshProUGUI selectButton;      // 미션 수행 후 선택 버튼

    // 켤 때 초기화
    public void InitCharacterView() 
    {
        if(characterObjs.Count > 0) { return; }

        for(int i = 0; i < CharacterManager.Instance.CharacterData.Count; i++) 
        {
            GameObject ch = Instantiate(characterPrefab);
            ch.transform.SetParent(contenct.transform, false);

            // 캐릭터 오브젝트 세팅
            ch.GetComponent<CharacterObject>().Init(CharacterManager.Instance.CharacterData[i].CharaterName , i);
        }
    }
}

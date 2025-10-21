using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterObject : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TextMeshProUGUI characterTitle;
    [SerializeField] CharacterType characterType;
    [SerializeField] Image characterImage;
    [SerializeField] Action<CharacterType> selectAction;       // 해당 오브젝트가 선택되었을 때 액션 

    public void Init(string name, CharacterType type, Sprite sprite, Action<CharacterType> action = null ) 
    {
        characterTitle.text = name;
        this.characterType = type;
        this.characterImage.sprite = sprite;
        selectAction = action;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        selectAction?.Invoke(characterType);
    }
}

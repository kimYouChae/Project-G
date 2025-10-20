using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterObject : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TextMeshProUGUI characterTitle;
    [SerializeField] int index;
    [SerializeField] Action<int> selectAction;       // 해당 오브젝트가 선택되었을 때 액션 

    public void Init(string name, int index, Action<int> action = null ) 
    {
        characterTitle.text = name;
        this.index = index;
        selectAction = action;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        selectAction?.Invoke(index);
    }
}

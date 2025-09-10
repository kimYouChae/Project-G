using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NickNameView : MonoBehaviour
{
    [Header("===NickNameUi===")]
    [SerializeField] TMP_InputField nickInputField;
    [SerializeField] Button enterNickNameButton;

    public Action<string> submitNickNameAction;

    private void Awake()
    {
        enterNickNameButton.onClick.AddListener(AddNickNameButton);
    }

    public void RegisterNickNameAction(Action<string> action) 
    {
        submitNickNameAction += action;
    }

    private void AddNickNameButton() 
    {
        submitNickNameAction?.Invoke( nickInputField.text );
    }

    public void InValueNickNamePopUp() 
    {
        // 닉네임 잘못된 팝업 띄우기
        Debug.Log("닉네임이 잘못되었음");
    }
}

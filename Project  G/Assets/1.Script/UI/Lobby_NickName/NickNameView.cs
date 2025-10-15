using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NickNameView : MonoBehaviour, ILocalizable
{
    [Header("===NickNameUi===")]
    [SerializeField] TMP_InputField nickInputField;
    [SerializeField] Button enterNickNameButton;

    [Header("===Localize Text===")]
    [SerializeField] TextMeshProUGUI inputFieldText;
    [SerializeField] TextMeshProUGUI inputButtonText;

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

    private void OnEnable()
    {
        LocalizationManager.Instance.RegisterChangeLanguage(IUpdateLocalization);
    }

    public void IUpdateLocalization(LanguageType type)
    {
        inputFieldText.text = LocalizationManager.Instance.ReturnLocalizationString(type, LocalizationKey.NickName_Input);
        inputButtonText.text = LocalizationManager.Instance.ReturnLocalizationString(type, LocalizationKey.Input);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomView : MonoBehaviour, ILocalizable
{
    [Header("===CreateUi===")]
    [SerializeField] TMP_InputField roomTitleField;
    [SerializeField] Image mapImage;
    [SerializeField] TextMeshProUGUI mapTitle;

    [Header("===Button===")]
    [SerializeField] Button passwordCopyButton;
    [SerializeField] Button createRoomButton;
    [SerializeField] Button rightButton;
    [SerializeField] Button leftButton;

    [SerializeField] Button backButton;     // 뒤로가기 버튼 

    [Header("===Localize Text===")]
    [SerializeField] TextMeshProUGUI roomNameText;
    [SerializeField] TextMeshProUGUI roomNameInputFieldText;
    [SerializeField] TextMeshProUGUI passwordText;
    [SerializeField] TextMeshProUGUI createText;

    private Action CopyPassWordAction;
    private Action<string> CreateRoomAction;
    private Action<int> RightArrowAction;       // +1
    private Action<int> LeftArrowAction;        // -1
    private Action backButtonAction;

    private void Awake()
    {
        passwordCopyButton.onClick.AddListener(() => CopyPassWordAction?.Invoke());
        createRoomButton.onClick.AddListener(() => CreateRoomAction?.Invoke(roomTitleField.text));
        rightButton.onClick.AddListener(() => RightArrowAction?.Invoke( 1));
        leftButton.onClick.AddListener(()=> LeftArrowAction?.Invoke( -1));
        backButton.onClick.AddListener(() => backButtonAction?.Invoke());
    }

    public void RegisterCopyPassWord(Action action) { CopyPassWordAction += action; }
    public void RegisterCreatRoom(Action<string> action) { CreateRoomAction += action; }
    public void RegisterRightArrow(Action<int> action) { RightArrowAction += action; }
    public void RegisterLeftArrow(Action< int> action) { LeftArrowAction += action; }
    public void RegisterBackButton(Action action) { backButtonAction += action; }


    public void ChangeMapImage(int index) 
    {
        mapImage.sprite = ResourceManager.Instance.MapSprite(index);
        mapTitle.text = LocalizationManager.Instance.MapNameReturn(index);
    }

    private void OnEnable()
    {
        LocalizationManager.Instance.RegisterChangeLanguage(IUpdateLocalization);
    }

    public void IUpdateLocalization(LanguageType type)
    {
        roomNameText.text = LocalizationManager.Instance.ReturnLocalizationString(type, LocalizationKey.CreateRoom_RoomName);
        roomNameInputFieldText.text = LocalizationManager.Instance.ReturnLocalizationString(type, LocalizationKey.CreateRoom_RoomName);
        passwordText.text = LocalizationManager.Instance.ReturnLocalizationString(type, LocalizationKey.CreateRoom_Password);
        createText.text = LocalizationManager.Instance.ReturnLocalizationString(type, LocalizationKey.Lobby_CreateRoom);
    }
}

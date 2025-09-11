using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomView : MonoBehaviour
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

    [SerializeField] private Sprite[] mapSprite;

    private Action CopyPassWordAction;
    private Action<string> CreateRoomAction;
    private Action<int,int> RightArrowAction;       // 스프라이트 list 길이, +1
    private Action<int,int> LeftArrowAction;        // 스프라이트 list 길이, -1

    private void Awake()
    {
        passwordCopyButton.onClick.AddListener(() => CopyPassWordAction?.Invoke());
        createRoomButton.onClick.AddListener(() => CreateRoomAction?.Invoke(roomTitleField.text));
        rightButton.onClick.AddListener(() => RightArrowAction?.Invoke(mapSprite.Length, 1));
        leftButton.onClick.AddListener(()=> LeftArrowAction?.Invoke(mapSprite.Length, -1));
    }

    public void RegisterCopyPassWord(Action action) { CopyPassWordAction += action; }
    public void RegisterCreatRoom(Action<string> action) { CreateRoomAction += action; }
    public void RegisterRightArrow(Action<int,int> action) { RightArrowAction += action; }
    public void RegisterLeftArrow(Action<int, int> action) { LeftArrowAction += action; }


    public void ChangeMapImage(int index) 
    {
        mapImage.sprite = mapSprite[index];
        mapTitle.text = Define.MapName[index];
    }
}

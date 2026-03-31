using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class FriendObject : MonoBehaviour
{
    [SerializeField] Image profileImage;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] int index;     // 몇번째 friend 오브젝트인지 
    [SerializeField] Button friendSelectButton; // "초대" 버튼
    [SerializeField] TextMeshProUGUI buttonText;    // 버튼의 "초대" 텍스트 

    private FriendPopUP parentPopup;

    private void Awake()
    {
        friendSelectButton.onClick.AddListener(SelectFriendObj);
    }

    public void SetParentPopup(FriendPopUP popup) 
    { 
        parentPopup = popup;
    }

    public void UpdateFriendObj(Texture2D texture, string name, int index) 
    {
        // 이미지 세팅
        Sprite sprite = Sprite.Create(
        texture,
        new Rect(0, 0, texture.width, texture.height),
        new Vector2(0.5f, 0.5f)
        );

        profileImage.sprite = sprite;

        // 닉네임, 인덱스 세팅
        nameText.text = name;
        this.index = index;
    }

    private void SelectFriendObj()
    {
        parentPopup.InviteFriendByIndex(index);
    }


}

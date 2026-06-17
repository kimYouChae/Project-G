using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoomInfoObject : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI roomTitle;
    [SerializeField] private int roomObjectIndex;
    [SerializeField] private Image selectRoomImage; // 방 선택 시 On Off 되는 이미지
    public RoomListView roomListView;

    public int RoomObjectIndex { set { roomObjectIndex = value; } }

    private void OnEnable()
    {
        OnOffSelectImage(false);
    }

    public void SetRoomName(string name) 
    {
        roomTitle.text = name;
    }

    // true면 On, false 이면 Off
    public void OnOffSelectImage(bool flag) 
    {
        selectRoomImage.gameObject.SetActive(flag);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        roomListView.NotifySelectRoomIndex(roomObjectIndex);
    }
}

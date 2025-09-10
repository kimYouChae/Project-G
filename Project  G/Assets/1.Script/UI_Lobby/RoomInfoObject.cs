using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoomInfoObject : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int roomObjectIndex;
    public RoomListView roomListView;

    public int RoomObjectIndex { set { roomObjectIndex = value; } }

    public void OnPointerClick(PointerEventData eventData)
    {
        roomListView.NotifySelectRoomIndex(roomObjectIndex);
    }
}

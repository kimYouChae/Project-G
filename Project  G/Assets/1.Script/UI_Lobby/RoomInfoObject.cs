using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoomInfoObject : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int roomObjectIndex;

    public int RoomObjectIndex { set { roomObjectIndex = value; } }

    public void OnPointerClick(PointerEventData eventData)
    {
        LobbyUIManager.GetInstance().SettingRoomIndex(roomObjectIndex);
    }
}

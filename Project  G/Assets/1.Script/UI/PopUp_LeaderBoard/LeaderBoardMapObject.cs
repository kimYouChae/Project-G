using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LeaderBoardMapObject : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    LeaderBoardPopUp popup;
    [SerializeField]
    private MapType mapType;
    [SerializeField]
    private Image grayInActiveImage;

    public void OnPointerClick(PointerEventData eventData)
    {
        popup.InitPopup(mapType);
    }

    public void OnOnffInActiveImage(bool flag) 
    {
        grayInActiveImage.gameObject.SetActive(flag);
    }
}

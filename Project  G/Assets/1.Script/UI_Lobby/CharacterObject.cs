using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterObject : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private int characterIndex;

    public int CharacterIndex { get => characterIndex; set => characterIndex = value; }

    public void OnPointerClick(PointerEventData eventData)
    {
        LobbyUIManager.GetInstance().SettingCharacterIndex(characterIndex);
    }
}

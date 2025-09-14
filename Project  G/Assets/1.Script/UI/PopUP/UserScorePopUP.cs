using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserScorePopUP : UIPopUP
{
    [Header("UserScorePopUP")]
    [SerializeField] TextMeshProUGUI[] mapNameText;
    [SerializeField] TextMeshProUGUI[] mapScoreText;

    [SerializeField] Button closeButton;

    private void Start()
    {
        closeButton.onClick.AddListener(() => 
        { 
            gameObject.SetActive(false);
            LobbyUIManager.GetInstance().OnOffPopUPPanel(false);
        });
    }

    public void IniUserScore(MapType type, float score) 
    {
        mapNameText[(int)type].text = Define.MapName[(int)type];
        mapScoreText[(int)type].text = score.ToString();
    }


}

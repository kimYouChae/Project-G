using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextPopUp : UIPopUP
{
    [SerializeField] TextMeshProUGUI popupText;

    public void UpdateText(string text) 
    {
        popupText.text = text;
    }
}

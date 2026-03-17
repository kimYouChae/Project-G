using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextPopUp : UIPopUP
{
    [SerializeField] TextMeshProUGUI popupText;

    // Text 팝업은 보통 경고문이기 때문에 OpenSFX를 Warning으로 설정
    protected override SFXType GetOpenSFXType() 
    {
        return SFXType.UIWarning;
    }

    public void UpdateText(string text) 
    {
        base.OpenPopUP();

        popupText.text = text;
    }
}

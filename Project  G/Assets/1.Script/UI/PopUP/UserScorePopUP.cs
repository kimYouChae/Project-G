using System;
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

    private void IniUserScore(MapType type, float score)
    {
        mapNameText[(int)type].text = Define.MapName[(int)type];
        mapScoreText[(int)type].text = score.ToString();
    }

    public void InitUserScorePopup() 
    {
        Array type = System.Enum.GetValues(typeof(MapType));

        for (int i = 0; i < type.Length; i++)
        {
            MapType mapType = (MapType)type.GetValue(i);

            if (mapType == MapType.None)
                return;

            float v = UserDataManager.Instance.UserData.MapTypeToScore[mapType];
            IniUserScore(mapType, v);
        }
    }
}

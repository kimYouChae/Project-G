using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginUi : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nickNameField;
    [SerializeField]
    private Button onOffModeSelectPanel;

    public string PlayerNickName() 
    {
        return nickNameField.text;
    }

}

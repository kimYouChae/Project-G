using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameLoadingUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI waitingText;

    private void Start()
    {
        waitingText.text = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.InGame_Loading);
    }
}

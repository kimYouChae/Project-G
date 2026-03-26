using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    [Space]
    [Header("===TitleUI===")]
    [SerializeField]
    private TextMeshProUGUI titleText;

    private void Start()
    {
        titleText.text = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.Server_Conneting);

        StartCoroutine(StartTitleLogic());
    }

    IEnumerator StartTitleLogic() 
    {
        while(true) 
        {
            if (SteamConnected.isSteamReady &&
                PunConnected.hasHandledInitialPhotonConnect)
            {
                // "클릭 시 실행" 텍스트로 변경 
                titleText.text = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.Enter_AnyKey);

                if (Input.anyKeyDown)
                {
                    LobbyUIManager.Instance.OnOffDarkPanel(true);

                    // panel 변경 
                    LobbyUIManager.Instance.ChangePanel(LobbyPanelType.Title, LobbyPanelType.Lobby);

                    // SFX 실행
                    SFXManager.Instance.PlaySFX(SFXType.UIClick);

                    // BGM 교체
                    BGMManager.Instance.PlayBGM(BGMType.Lobby);

                    yield break;
                }
            }

            yield return null;
        }
    }

}

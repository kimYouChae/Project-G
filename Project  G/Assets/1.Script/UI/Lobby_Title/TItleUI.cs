using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour, ILobbyPanelInitionlize
{
    [Space]
    [Header("===TitleUI===")]
    [SerializeField]
    private TextMeshProUGUI titleText;

    private void Start()
    {
        // 타이틀 로컬라이징
        titleText.text = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.Server_Conneting);
    }

    // LobbyUiManger의 ChangePanel가 실행되서
    // Title로 바뀔때 "만" 실행 
    // 게임씬 -> 로비씬 바뀔 때 켜지는거 방지
    public void IInitPanel()
    {
        StartCoroutine(StartTitleLogic());
    }

    IEnumerator StartTitleLogic() 
    {
        bool flag = false;

        while(true) 
        {
            if (SteamConnected.isSteamReady &&
                PunConnected.hasHandledInitialPhotonConnect)
            {
                // 한번만 실행될 수 있도록
                if (!flag) 
                {
                    // "클릭 시 실행" 텍스트로 변경 
                    titleText.text = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.Enter_AnyKey);
                    flag = true;
                }

                if (Input.anyKeyDown)
                {
                    LobbyUIManager.Instance.OnOffDarkPanel(true);

                    // 포톤의 로컬 player 데이터 세팅 
                    PunLobbyManager.Instance.SettingPhotonLocalPlayer();

                    // 애널리스틱 이벤트 실행
                    AnalyticsManager.SendSessionStart();

                    // panel 변경 
                    LobbyUIManager.Instance.ChangePanel(LobbyPanelType.Title, LobbyPanelType.Lobby);

                    // 로컬라이징 실행
                    LocalizationManager.Instance.ChangeLanguageType();

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

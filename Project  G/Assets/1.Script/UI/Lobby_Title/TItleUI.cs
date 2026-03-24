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

        // 서버 연결 시 액션 등록 
        PunLobbyManager.Instance.RegisterServerConnectAction(()=> 
            titleText.text = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.Enter_AnyKey));
        PunLobbyManager.Instance.RegisterServerConnectAction(() => StartCoroutine(test()));
    }

    IEnumerator test() 
    {
        while(true) 
        {
            if (Input.anyKeyDown) 
            {
                LobbyUIManager.Instance.OnOffDarkPanel(true);
                
                // 로그인 로직 시작 
                StartCoroutine(LoginUser());

                // panel 변경 
                LobbyUIManager.Instance.ChangePanel(LobbyPanelType.Title, LobbyPanelType.Lobby);

                // SFX 실행
                SFXManager.Instance.PlaySFX(SFXType.UIClick);

                // BGM 교체
                BGMManager.Instance.PlayBGM(BGMType.Lobby);

                yield break;
            }

            yield return null;
        }
    }

    private IEnumerator LoginUser() 
    {
        // 1. 스팀 로그인
        long steamID = SteamAPITest.Instance.GetSteamID();
        string nick = SteamAPITest.Instance.GetSteamNick();
        string cnr = SteamAPITest.Instance.GetCountry();

        // 2. 유저데이터 스크립트에 steam 관련 정보 저장
        UserDataManager.Instance.InsertUserInfo(steamID, nick, cnr);

        // 3. 로그인 API
        yield return StartCoroutine(
            GameServices.Instance.AuthService.AuthService(steamID, nick, cnr));

        // 4. 차트 불러오기 
        yield return GameServices.Instance.ChartLogic();
    }
}

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
                LoginUser();
                yield break;
            }

            yield return null;
        }
    }

    private void LoginUser() 
    {
        // 1. 스팀 로그인
        string steamID = SteamAPI.instance.GetSteamID();
        string nick = SteamAPI.instance.GetSteamNick();
        string cnr = SteamAPI.instance.GetCountry();

        // 2. 로그인 API
        GameServices.Instance.AuthService.AuthService(steamID, nick, cnr);

        // 3. 차트 불러오기 
        GameServices.Instance.ChartLogic();
    }
}

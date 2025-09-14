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

    const string isConnecting = "서버에 연결중....";
    const string isReady = "시작하려면 아무키나 누르세요";

    private void Start()
    {
        titleText.text = isConnecting;

        // 서버 연결 시 액션 등록 
        PunLobbyManager.Instance.RegisterServerConnectAction(()=> titleText.text = isReady);
        PunLobbyManager.Instance.RegisterServerConnectAction(() => StartCoroutine(test()));
    }

    IEnumerator test() 
    {
        while(true) 
        {
            if (Input.anyKeyDown) 
            {
                EnterByLocalData();
                yield break;
            }

            yield return null;
        }
    }

    private void EnterByLocalData() 
    {
        // 1. 게스트 로그인
        // 콜백으로 하는 이유 : 해당 메서드는 SendQueue방식인데 
        // 무조건 GuestLogin메서드가 끝난 후 실행되야하기때문에 
        // 콜백으로 넘겨서 명시적으로 실행시켜주기
        BackEndServerManager.Instance.GuestLogin( () => 
        {
            // 2. 닉네임 유무 결과
            NickCheckResultType result = BackEndServerManager.Instance.isHasNickName();
            switch (result)
            {
                case NickCheckResultType.NoPlayerInfo:
                    Debug.Log("PlayerInfo데이터가 NUll입니다");
                    return;

                // 2. 닉네임이 없으면 ? 
                case NickCheckResultType.NoNickname:
                    Debug.Log("닉네임이 없습니다. 닉네임을 설정하려 갑시다");
                    // 2-1. 닉네임 ui On
                    LobbyUIManager.Instance.ChangePanel(LobbyPanelType.Title, LobbyPanelType.NickName);
                    break;

                // 3. 닉네임 있으면 
                case NickCheckResultType.HasNickname:
                    Debug.Log("닉네임이 있습니다. 로비 panel로 갑니다");

                    // 뒤끝 테이블에 저장되어 있는 유저 정보 가져오기 
                    UserDataManager.Instance.GetUserDataInTable();

                    // 3-1. lobby Ui On
                    LobbyUIManager.Instance.ChangePanel(LobbyPanelType.Title, LobbyPanelType.Lobby);

                    break;
            }

            // 차트 불러오기 
            BackendChartManager.Instance.InitBackendChart();
        });


    }
}

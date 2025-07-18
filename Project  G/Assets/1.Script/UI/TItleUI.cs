using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class LobbyUIManager : MonoBehaviour
{
    [Space]
    [Header("===TitleUI===")]
    [SerializeField]
    private Button startButton;

    private void InitTitleUI() 
    {
        // 시작 버튼 
        if (startButton != null)
            startButton.onClick.AddListener(EnterByLocalData);
    }

    private void EnterByLocalData() 
    {
        // 1. 게스트 로그인
        // 콜백으로 하는 이유 : 해당 메서드는 SendQueue방식인데 
        // 무조건 GuestLogin메서드가 끝난 후 실행되야하기때문에 
        // 콜백으로 넘겨서 명시적으로 실행시켜주기
        BackEndServerManager.GetInstance().GuestLogin( () => 
        {
            // 2. 닉네임 유무 결과
            NickCheckResultType result = BackEndServerManager.GetInstance().isHasNickName();
            switch (result)
            {
                case NickCheckResultType.NoPlayerInfo:
                    Debug.Log("PlayerInfo데이터가 NUll입니다");
                    return;

                // 2. 닉네임이 없으면 ? 
                case NickCheckResultType.NoNickname:
                    Debug.Log("닉네임이 없습니다. 닉네임을 설정하려 갑시다");
                    // 2-1. 닉네임 ui On
                    ChangePanel(LobbyPanelType.Title, LobbyPanelType.NickName);
                    break;

                // 3. 닉네임 있으면 
                case NickCheckResultType.HasNickname:
                    Debug.Log("닉네임이 있습니다. 로비 panel로 갑니다");
                    // 3-1. lobby Ui On
                    ChangePanel(LobbyPanelType.Title, LobbyPanelType.Lobby);
                    break;
            }
        });


    }
}

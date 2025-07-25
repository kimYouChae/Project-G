using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class LobbyUIManager : MonoBehaviour
{
    [Space]
    [Header("===NickNameUi===")]
    [SerializeField] TMP_InputField nickInputField;
    [SerializeField] Button enterNickNameButton;

    private void InitNickNameUI() 
    {
        // 닉네임 입력 버튼
        if (enterNickNameButton != null)
            enterNickNameButton.onClick.AddListener(EnterNickName);
    }

    private void EnterNickName() 
    { 
        string nickName = nickInputField.text;

        if (nickName.Equals(string.Empty))
        {
            Debug.Log("닉네임은 빈 칸이면 안됩니다");
            return;
        }

        BackEndServerManager.GetInstance().UpdateNickName(nickName);

        // 포톤 닉네임 세팅 
        PunLobbyManager.GetInstance().SettingNickName(nickName);

        // 화면 전환
        ChangePanel(LobbyPanelType.NickName , LobbyPanelType.Lobby);
    }
}

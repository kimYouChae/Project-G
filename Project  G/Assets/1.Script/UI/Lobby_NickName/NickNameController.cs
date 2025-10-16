using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NickNameModel 
{
    public string NickName;

    public void SetNickName(string nick) 
    { 
        NickName = nick;
    }

    public bool isValid() 
    {
        return NickName.Equals(string.Empty);
    }
}

public class NickNameController : ILobbyPanelInitionlize
{
    private NickNameView nicknameView;
    private NickNameModel nicknameModel;

    public NickNameController(NickNameView nicknameView, NickNameModel nicknameModel)
    {
        this.nicknameView = nicknameView;
        this.nicknameModel = nicknameModel;

        nicknameView.RegisterNickNameAction(OnSubMitNickName);
    }

    public void IInitPanel()
    {
        
    }

    public void OnSubMitNickName(string inputNickName) 
    {
        // 닉네임 세팅
        nicknameModel.SetNickName(inputNickName);

        // 유효성 검사 
        if(nicknameModel.isValid()) 
        {
            Debug.Log("닉네임이 잘못되었음");
            // 팝업 띄우기 


            return;
        }

        // 뒤끝 테이블에 유저 정보 저장하기 
        UserDataManager.Instance.InsertToUserTable(inputNickName);

        // 닉네임 업데이트
        BackEndServerManager.Instance.UpdateNickName(inputNickName);

        // 포톤 닉네임 세팅 
        PunLobbyManager.Instance.SettingNickName(inputNickName);

        // 화면 전환
        LobbyUIManager.Instance.ChangePanel(LobbyPanelType.NickName, LobbyPanelType.Lobby);
    }
}

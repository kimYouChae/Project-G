
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CreateRoomModel 
{
    public int roomMaxUser = 2;
    public int roomPassword;
    public bool isCreatePassword = false;
    public int currMapIndex = 0;

    public const int maxPasswordDigit = 8;

    public int GetRandomNPassword()
    {
        int min = (int)Mathf.Pow(10, maxPasswordDigit - 1);
        int max = (int)Mathf.Pow(10, maxPasswordDigit) - 1;
        return Random.Range(min, max + 1); // max는 포함되지 않으므로 +1
    }
}

public class CreateRoomController : ILobbyPanelInitionlize
{
    private CreateRoomView roomView;
    private CreateRoomModel roomModel;

    public CreateRoomController(CreateRoomView roomView, CreateRoomModel roomModel)
    {
        this.roomView = roomView;
        this.roomModel = roomModel;

        roomView.RegisterCopyPassWord(CopyPassWord);
        roomView.RegisterCreatRoom(CreateRoom);
        roomView.RegisterRightArrow(ChangeMapIndex);
        roomView.RegisterLeftArrow(ChangeMapIndex);
        roomView.RegisterBackButton(BackAction);
    }

    private void ClearRoomInfo() 
    {
        // 방 정보 초기화 
        PhotonRoomInfo.Clear();

        // 클립보드 빈칸
        GUIUtility.systemCopyBuffer = string.Empty;

        // 비밀번호 복사 플래그 false
        roomModel.isCreatePassword = false;
    }

    private void BackAction()
    {
        // SFX 실행
        SFXManager.Instance.PlaySFX(SFXType.UIBack);

        // 로비로 패널 전환 
        LobbyUIManager.Instance.ChangePanel(LobbyPanelType.CreateRoom, LobbyPanelType.Lobby);

        ClearRoomInfo();
    }

    private void CopyPassWord() 
    {
        // SFX 실행
        SFXManager.Instance.PlaySFX(SFXType.UIClick);

        roomModel.roomPassword = roomModel.GetRandomNPassword();

        // 클립보드에 복사하기
        GUIUtility.systemCopyBuffer = roomModel.roomPassword.ToString();

        // 비밀번호 복사 플래그 true
        roomModel.isCreatePassword = true;
    }

    private void CreateRoom(string nameTitle) 
    {
        // model에 저장된 index (mapType)에 해당하는 데이터가 있는지 
        if (StageDataManager.Instance.HasMapData((MapType)roomModel.currMapIndex))
        {
            // 데이터 없으면 -> 팝업 띄우기 
            TextPopUp textPopUp = UIManager.Instance.GetPopUP<TextPopUp>();
            textPopUp.UpdateText(LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.Map_NotAvailable));

            return;
        }

         // 방 정보 세팅
        RoominfoSetting(nameTitle);

        // 방 생성
        PhotonCreateRoom();
    }

    private void RoominfoSetting(string nameTitle)
    {
        PhotonRoomInfo.RoomCode = Guid.NewGuid().ToString().Substring(0, 8);
        PhotonRoomInfo.RoomName = nameTitle;
        PhotonRoomInfo.Password = roomModel.roomPassword; 
        PhotonRoomInfo.MaxUser = roomModel.roomMaxUser;
        PhotonRoomInfo.MapTypeName = ((MapType)roomModel.currMapIndex).ToString();
    }

    private void PhotonCreateRoom()
    {
        Debug.Log($"방 생성 시도 - RoomName: {PhotonRoomInfo.RoomName}");

        if (!roomModel.isCreatePassword)
        {
            //Debug.Log("비밀 번호를 복사 해야합니다!");

            // 팝업 띄우기 
            TextPopUp textPopUp = UIManager.Instance.GetPopUP<TextPopUp>();
            textPopUp.UpdateText(LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.Popup_PasswordCopyFailed));

            return;
        }

        if (string.IsNullOrEmpty(PhotonRoomInfo.RoomName))
        {
            //Debug.LogError("방 이름이 비어 있습니다!");

            // 팝업 띄우기 
            TextPopUp textPopUp = UIManager.Instance.GetPopUP<TextPopUp>();
            textPopUp.UpdateText(LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.Popup_RoomNameEmpty));

            return;
        }

        // SFX 실행
        SFXManager.Instance.PlaySFX(SFXType.UIClick);

        // PunLobbyManager 매서드 실행
        PunLobbyManager.Instance.CreateCusomRoom();

        // panel 전환 
        LobbyUIManager.Instance.ChangePanel(LobbyPanelType.CreateRoom, LobbyPanelType.WaitingRoom);
    }

    private void ChangeMapIndex( int idx )
    {
        int mapTypeLength = Extension.EnumCount<MapType>() - 1;
        int currIndex = roomModel.currMapIndex + idx;

        if (currIndex < 0)
            currIndex = mapTypeLength - 1;
        if (currIndex >= mapTypeLength)
            currIndex = 0;

        roomModel.currMapIndex = currIndex;

        // view 메서드 호출
        roomView.ChangeMapInfo(roomModel.currMapIndex);
    }

    public void IInitPanel()
    {
        // 다른 panel -> 방 생성 패널 입장 시 
        ClearRoomInfo();
        roomView.ChangeMapInfo(roomModel.currMapIndex);
        roomView.EmptyRoomTitle();
    }
}

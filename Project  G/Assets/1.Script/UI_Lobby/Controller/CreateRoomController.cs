using Photon.Pun.Demo.Cockpit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

// room을 만들 때 설정하는 데이터를 담아놓는 용도
public static class PhotonRoomInfo
{
    // 방 이름
    public static string RoomName
    {
        get => PlayerPrefs.GetString("RoomName");
        set => PlayerPrefs.SetString("RoomName", value);
    }

    // 비밀번호 
    public static int Password
    {
        get => PlayerPrefs.GetInt("Password");
        set => PlayerPrefs.SetInt("Password", value);
    }

    // 최대 유저
    public static int MaxUser
    {
        get => PlayerPrefs.GetInt("MaxUser");
        set => PlayerPrefs.SetInt("MaxUser", value);
    }

    // 맵 타입
    public static string MapTypeName
    {
        get => PlayerPrefs.GetString("MapType");
        set => PlayerPrefs.SetString("MapType", value);
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
    }

    private void CopyPassWord() 
    {
        roomModel.roomPassword = roomModel.GetRandomNPassword();

        // 복사하기
        GUIUtility.systemCopyBuffer = roomModel.roomPassword.ToString();

        roomModel.isCreatePassword = true;
    }

    private void CreateRoom(string nameTitle) 
    {
        // 방 정보 세팅
        RoominfoSetting(nameTitle);

        // 방 생성
        FusionCreateRoom();
    }

    private void RoominfoSetting(string nameTitle)
    {
        PhotonRoomInfo.RoomName = nameTitle;
        PhotonRoomInfo.MaxUser = roomModel.roomMaxUser;
        PhotonRoomInfo.Password = roomModel.roomPassword; ;
        PhotonRoomInfo.MapTypeName = ((MapType)roomModel.currMapIndex).ToString();
    }

    private void FusionCreateRoom()
    {
        Debug.Log($"방 생성 시도 - RoomName: {PhotonRoomInfo.RoomName}");

        if (!roomModel.isCreatePassword)
        {
            Debug.Log("비밀 번호를 복사 해야합니다!");

            // ##TODO : 팝업 띄우기 

            return;
        }

        if (string.IsNullOrEmpty(PhotonRoomInfo.RoomName))
        {
            Debug.LogError("방 이름이 비어 있습니다!");

            // ##TODO : 팝업 띄우기 

            return;
        }

        // PunLobbyManager 매서드 실행
        PunLobbyManager.Instance.CreateCusomRoom();
    }

    private void ChangeMapIndex(int spriteLength, int idx)
    {
        if (idx < 0)
            idx = spriteLength - 1;
        if (idx >= spriteLength)
            idx = 0;

        roomModel.currMapIndex = idx;

        // view 메서드 호출
        roomView.ChangeMapImage(roomModel.currMapIndex);
    }

    public void IInitPanel()
    {
        
    }
}

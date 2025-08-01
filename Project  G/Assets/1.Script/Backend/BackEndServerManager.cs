using System;
using UnityEngine;
using static BackEnd.SendQueue;
using BackEnd;
using LitJson;
using BackEnd.Content;

public class BackEndServerManager : Singleton<BackEndServerManager>
{
    [Header("===INFO===")]
    [SerializeField] private BackendReturnObject playerInfo;

    public BackendReturnObject PlayerInfo { get => playerInfo; }

    protected override void Singleton_Awake()
    {
        
    }

    public string ReturnNickName() 
    {
        return playerInfo.GetReturnValuetoJSON()["row"]["nickname"].ToString();
    }

    #region 게스트 로그인 / 로컬에 있는 게스트 정보 가져오기

    public void GuestLogin(Action onComplete = null) 
    {
        Enqueue(Backend.BMember.GuestLogin, callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log("게스트 로그인 성공 했습니다! ");

                // 게스트 로그인 
                // 로컬에 정보가 있으면 -> 로그인
                // 로컬에 정보가 없으면 -> 회원가입
                GetUserInfo(onComplete);
            }
            else 
            {
                Debug.Log("게스트 로그인 실패 했습니다 " + callback);
            }
        });
    }

    private void GetUserInfo(Action onComplete = null) 
    {
        // GuestLogin후 실행, 로컬에 무조건 정보 있음 
        // GetUserInfo : 로컬에 저장된 유저 정보 불러오기 (비동기)
        Enqueue(Backend.BMember.GetUserInfo, callback => 
        {
            if (!callback.IsSuccess())
            {
                Debug.Log("로컬의 유저 정보를 가져오지 못했습니다! ");
                return;
            }

            Debug.Log("로컬의 유저 데이터를 저장합니다");
            // 가져온 유저 정보 출력 
            playerInfo = callback;

            string userJson = playerInfo.ReturnValue;
            // JsonData data = playerInfo.GetReturnValuetoJSON();    // Lit Json 데이터타입

            Debug.Log("로컬에 저장되어 있는 정보 :" + userJson);

            // 끝난 후 콜백 실행 
            onComplete?.Invoke();

            // 포톤 닉네임 세팅 
            PunLobbyManager.Instance.SettingNickName(ReturnNickName());

        });
    }

    // 닉네임 있는지 없는지 검사
    public NickCheckResultType isHasNickName() 
    {
        if (playerInfo == null)
            return NickCheckResultType.NoPlayerInfo;

        JsonData data = playerInfo.GetReturnValuetoJSON();
        JsonData info = data["row"];

        if (info["nickname"] == null)
            return NickCheckResultType.NoNickname;

        return NickCheckResultType.HasNickname;
    }

    #endregion

    #region 닉네임 불러오기

    // 닉네임 업데이트
    // 닉네임이 없으면 매치 서버 접속이 안됨
    public void UpdateNickName(string nickname) 
    {
        Enqueue(Backend.BMember.UpdateNickname, nickname, callback =>
        {
            if (!callback.IsSuccess()) 
            {
                Debug.LogError("닉네임 생성 실패\n" + callback.ToString());

            }

            // 닉네임 설정 후 다시 유저 정보 가져오기 
            GetUserInfo();
        });
    }



    #endregion

}

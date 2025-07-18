using System;
using UnityEngine;
using static BackEnd.SendQueue;
using BackEnd;
using LitJson;

public enum NickCheckResultType
{
    NoPlayerInfo,
    NoNickname,
    HasNickname
}

public class BackEndServerManager : MonoBehaviour
{
    private static BackEndServerManager instance;   // 인스턴스

    [Header("===INFO===")]
    [SerializeField] private BackendReturnObject playerInfo;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
    }

    public static BackEndServerManager GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("BackEndServerManager 인스턴스가 존재하지 않습니다.");
            return null;
        }

        return instance;
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
            // string userJson = playerInfo.ReturnValue;
            // JsonData data = playerInfo.GetReturnValuetoJSON();    // Lit Json 데이터타입

            // Debug.Log(userJson);

            // 끝난 후 콜백 실행 
            onComplete?.Invoke();

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

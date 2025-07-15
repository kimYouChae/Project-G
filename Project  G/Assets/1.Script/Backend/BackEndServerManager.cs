using System;
using UnityEngine;
using static BackEnd.SendQueue;
using BackEnd;

public class BackEndServerManager : MonoBehaviour
{
    private static BackEndServerManager instance;   // 인스턴스

    // 로그인 여부
    public bool isLogin { get; private set; }   
    // 로그인 결과를 외부에 알리는 액션 (성공여부 , 해당하는 메시지)
    private Action<bool, string> loginSuccessFunc = null;

    // 로그인 유저 정보
    public string myNickName { get; private set; } = string.Empty;
    // 로그인 계정의 inDate
    public string myIndate { get; private set; } = string.Empty;

    // 디버깅
    private const string BackendError = "로그인 상태 : {0}\n에러코드 : {1}\n메세지 : {2}";

    void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
        // 모든 씬에서 유지
        DontDestroyOnLoad(this.gameObject);
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

    public void GuestLogin(Action<bool, string> func)
    {
        Enqueue(Backend.BMember.GuestLogin, callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log("게스트 로그인 성공");
                loginSuccessFunc = func;

                // 유저 데이터 가져오기 
                OnPrevBackendAuthorized();
                return;
            }

            Debug.Log("게스트 로그인 실패\n" + callback);
            func(false, string.Format(BackendError,
                callback.GetStatusCode(), callback.GetErrorCode(), callback.GetMessage()));
        });
    }

    // 유저 불러오기 사전작업 
    private void OnPrevBackendAuthorized() 
    {
        isLogin = true;

        OnBackendAuthorized();
    }

    // 유저 정보 불러오기 
    private void OnBackendAuthorized() 
    {
        // GetUserInfo : 로컬에 저장된 유저 정보 불러오기 (비동기)
        Enqueue(Backend.BMember.GetUserInfo, callback =>
        {
            // 불러오기 실패
            if (!callback.IsSuccess())
            {
                Debug.Log("유저 정보 불러오기 실패\n" + callback);
                loginSuccessFunc(false, string.Format(BackendError,
                    callback.GetStatusCode(), callback.GetErrorCode(), callback.GetErrorMessage()));
                return;
            }

            Debug.Log("유저정보\n" + callback);

            // BackendReturnObject 형태로 return (return value : json형태)
            var info = callback.GetReturnValuetoJSON()["row"];

            Debug.Log(info);

            if (info["nickname"] == null)
            {
                Debug.Log("닉네임이 NULL입니다");
                // 로그인 UI : 닉네임 UI

                // !!!여기서 return됨 
                return;
            }

            myNickName = info["nickname"].ToString();
            myIndate = info["inDate"].ToString();

            if (loginSuccessFunc != null)
            {
                BackEndMatchManager.GetInstance().GetMatchList(loginSuccessFunc);
            }

        });
    }
    #endregion

    #region 닉네임 불러오기

    // 닉네임 업데이트
    // 닉네임이 없으면 매치 서버 접속이 안됨
    public void UpdateNickName(string nickname ,Action<bool, string> func) 
    {
        Enqueue(Backend.BMember.UpdateNickname, nickname, callback =>
        {
            if (!callback.IsSuccess()) 
            {
                Debug.LogError("닉네임 생성 실패\n" + callback.ToString());

                func(false, string.Format(BackendError,
                    callback.GetStatusCode(), callback.GetErrorCode(), callback.GetMessage()));
            }

            loginSuccessFunc = func;
            OnBackendAuthorized();
        });
    }

    #endregion
}

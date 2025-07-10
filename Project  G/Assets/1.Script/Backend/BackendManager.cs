using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackendManager : MonoBehaviour
{
    void Start()
    {
        // 뒤끝 초기화
        var bro = Backend.Initialize(); 

        // 뒤끝 초기화에 대한 응답값
        if (bro.IsSuccess())
        {
            // 성공일 경우 statusCode 204 Success
            Debug.Log("초기화 성공 : " + bro); 
        }
        else
        {
            // 실패일 경우 statusCode 400대 에러 발생
            Debug.LogError("초기화 실패 : " + bro); 
        }

        PlayerLogIn();
    }

    void PlayerLogIn()
    {
        // BackendLogin.Instance.CustomSignUp("user1", "1234"); // [추가] 뒤끝 회원가입 함수
        BackendLogin.Instance.CustomLogin("user1", "1234"); // [추가] 뒤끝 로그인
        BackendLogin.Instance.UpdateNickname("원하는 이름"); // [추가] 닉네임 변겅

        BackendGameData.Instance.GameDataGet(); //[추가] 데이터 삽입 함수

        // [추가] 서버에 불러온 데이터가 존재하지 않을 경우, 데이터를 새로 생성하여 삽입
        if (BackendGameData.userData == null) 
        {
            BackendGameData.Instance.GameDataInsert();
        }

        // [추가] 로컬에 저장된 데이터를 변경 
        BackendGameData.Instance.LevelUp();

        //[추가] 서버에 저장된 데이터를 덮어쓰기(변경된 부분만)
        BackendGameData.Instance.GameDataUpdate();

        Debug.Log("테스트를 종료합니다.");
    }
}

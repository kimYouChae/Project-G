
using BackEnd;
using UnityEngine;
using System;
using BackEnd.Tcp;
using System.Reflection;

/*
 * 뒤끝 SDK에 정의되어 있음 . 주석 참고용 
 * 사용 시 using BackEnd.Tcp; 사용 필수
// 매칭 방법
public enum MatchType : byte
{
    None,
    Random, // 신청한 모든 유저 대상으로 매칭 
    Point,  // 뒤끝 콘솔에 입력한 포인트 이용해서 매칭
    MMR   // 점수 따라 매칭 
}

// 매칭 유형 
public enum MatchModeType : byte
{
    None,
    OneOnOne, // 일대일 / 1:1 매칭
    Melee,   // 다대다 / 최대 10까지 매칭이 가능한 팀전 매칭 (2:2 , 3:3, 4:4, 5:5)
    TeamOnTeam // 3 ~ 10명 개인전 매칭 
}
*/

public partial class BackEndMatchManager : MonoBehaviour
{
    // 현재 선택된 매치 타입 
    public MatchType nowMatchType { get; private set; } = MatchType.None;
    // 현재 선택된 매치 모드 타입
    public MatchModeType nowModeType { get; private set; } = MatchModeType.None;

    // 매칭 서버 접속
    public void JoinMatchServer()
    {
        if (isConnectMatchServer)
            return;

        ErrorInfo errorInfo;
        isConnectMatchServer = true;

        // 매칭 서버에 접속
        // 접속안되면 출력 
        if (!Backend.Match.JoinMatchMakingServer(out errorInfo)) 
        {
            Debug.Log($"매치 서버 접속 실패 : {errorInfo} ");
        }
    }

    // 매칭 서버 접속 종료
    public void LeaveMatchServer() 
    {
        isConnectMatchServer = false;
        Backend.Match.LeaveMatchMakingServer();
    }

    // 매칭 대기 방 만들기 
    // 혼자 매칭 해도 무조건 방을 생성한 뒤 매칭 신청해야함
    public bool CreateMatchRoom() 
    {
        // 매칭 서버에 연결되어 있지 않으면 매칭 서버 접속 
        if (isConnectMatchServer) 
        {
            Debug.Log("매치 서버에 연결되어 있지 않습니다.");
            Debug.Log("매치 서버에 접속을 시도합니다.");
            JoinMatchServer();
            return false;
        }

        Debug.Log("방 생성 요청을 서버로 보냄");
        Backend.Match.CreateMatchRoom();
        return true;
    }

    // 매칭 대기 방 나가기
    public void LeaveMatchLoom() 
    {
        Backend.Match.LeaveMatchRoom();
    }

    // 매칭 신청하기 
    // MatchType (1:1 / 개인전 / 팀전)
    // MatchModeType (랜덤/포인트/MMR)
    public void RequestMatchMaking(int idx) 
    {
        // 매칭 서버에 연결되어 있지 않으면 매칭 서버 접속
        if (!isConnectMatchServer) 
        {
            Debug.Log("매치 서버에 연결되어 있지 않습니다.");
            Debug.Log("매치 서버에 접속을 시도합니다.");
            JoinMatchServer();
            return;
        }

        isConnectMatchServer = false;

        // 매칭 신청
        Backend.Match.RequestMatchMaking(matchInfos[idx].matchType, 
            matchInfos[idx].matchModeType, matchInfos[idx].inDate);
    }
}

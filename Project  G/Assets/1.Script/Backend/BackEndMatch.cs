
using BackEnd;
using UnityEngine;
using System;
using BackEnd.Tcp;
using System.Reflection;
using Unity.VisualScripting;

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

        // 인게임 서버에 접속해있을 경우 대비해
        if (isConnectMatchServer) 
        {
            Backend.Match.LeaveGameServer();    // 인게임 서버 리브 호출 
        }

        nowMatchType = matchInfos[idx].matchType;
        nowModeType = matchInfos[idx].matchModeType;
        numOfClient = int.Parse(matchInfos[idx].headCount);
    }

    // 매칭 신청 취소하기 
    public void CancelRegistMatchMaking() 
    { 
        Backend.Match.CancelMatchMaking();
    }

    // '매칭 서버 접속'에 대한 리턴값 
    private void ProcessAccessMatchMakingServer(ErrorInfo errInfo) 
    {
        // 실패했으면
        if (errInfo != ErrorInfo.Success) 
        {
            // 접속 실패
            isConnectMatchServer = false;
        }

        if (!isConnectMatchServer)
        {
            Debug.Log("매치 서버 접속 실패 : " + errInfo.ToString());
        }
        else 
        {
            // 접속 성공
            Debug.Log("매치 서버 접속 성공");
        }
    }

    /*
     * 매칭 신청에 대한 리턴값
     * - 매칭 신청 성공했을 때
     * - 매칭 성공했을 때
     * - 매칭 신청 실패했을 때
     */
    private void ProcessmatchMakingResponse(MatchMakingResponseEventArgs args) 
    {
        string debugLog = string.Empty;
        bool isError = false;

        // ##TODO : 로비UI 작성 후 메서드 채워넣기 
        switch(args.ErrInfo)
        {
            // 매칭 성공
            case ErrorCode.Success:

                debugLog = string.Format("매칭 성공 : {0}", args.Reason);
                // 로비UI : 매치 끝 콜백 실행 

                // 성공 메서드 실행
                ProcessMatchSuccesss(args);
                break;

            // 매칭 '신청' 성공 or 매칭 중일때 - 매칭 신청 시도했을 때
            case ErrorCode.Match_InProgress:
                
                // 매칭 신청 성공
                if (args.Reason == string.Empty) 
                {
                    debugLog = "매칭 대기열에 등록되었습니다.";

                    // 로비UI : 매칭 요청 콜백 실행 

                }
                break;

            // 매칭 신청이 취소 되었을 때 
            case ErrorCode.Match_MatchMakingCanceled:
                debugLog = string.Format("매칭 신청 취소 : {0}", args.Reason);
                break;

            // 매치 타입을 잘못 전송했을 때
            case ErrorCode.Match_InvalidMatchType:
                isError = true;
                debugLog = string.Format("매칭 실패 : {0}" , "잘못된 매치 타입입니다.");

                // 로비UI : 매칭 요청 콜백 실행 

                break;

            // 매치 모드를 잘못 전송했을 때
            case ErrorCode.Match_InvalidModeType:
                isError = true;
                debugLog = string.Format("매칭 실패 : {0}", "잘못된 모드 타입입니다.");

                // 로비UI : 매칭 요청 콜백 실행 

                break;

            // 잘못된 요청을 전송했을 때
            case ErrorCode.InvalidOperation:
                isError = true;
                debugLog = string.Format("잘못된 요청입니다\n{0}", args.Reason);
                // 로비UI : 매칭 요청 콜백 실행 

                break;

            // 잘못된 요청을 전송했을 때
            case ErrorCode.Match_Making_InvalidRoom:
                isError = true;
                debugLog = string.Format("잘못된 요청입니다\n{0}", args.Reason);
                // 로비UI : 매칭 요청 콜백 실행 

                break;

            // 매칭 되고, 서버에서 방 생성할 때 에러 발생 시 exception이 return
            // 이 경우 다시 매칭 신청
            case ErrorCode.Exception:
                isError = true;
                debugLog = string.Format("예외 발생 : {0}\n다시 매칭을 시도합니다.", args.Reason);

                // 로비UI : 매칭 요청 실행

                break;
        }

        if (!debugLog.Equals(string.Empty)) 
        {
            Debug.Log(debugLog);
            if(isError == true) 
            {
                // 로비UI : 에러 메세지 넘겨주기
            }
                
        }
    }

    // 매칭 성공했을 때
    // 인게임 서버로 접속 
    private void ProcessMatchSuccesss(MatchMakingResponseEventArgs args) 
    {
        ErrorInfo errorInfo;
        if (sessionIdList != null) 
        {
            Debug.Log("이전 세션 저장 정보");
            sessionIdList.Clear();
        }

        // JoinsGameServer ( 서버 address, 서버 port , 재접속 여부 , 성공/실패 여부 )
        if (!Backend.Match.JoinGameServer(args.RoomInfo.m_inGameServerEndPoint.m_address,
            args.RoomInfo.m_inGameServerEndPoint.m_port, false, out errorInfo)) 
        {
            Debug.Log("인게임 접속 실패 : " + errorInfo.ToString());
        }

        // 인자값에서 인게임 룸 토큰을 저장해두어야 함. 
        // 인게임 서버에서 룸에 접속할 때 필요
        // 1분 내에 모든 유저가 룸에 접속하지 않으면 해당 룸은 파기된다 
        isConnectMatchServer = true;
        isJoinGameRoom = false;
        InGameRoonToken = args.RoomInfo.m_inGameRoomToken;
        isSandBoxGame = args.RoomInfo.m_enableSandbox;

        var info = GetMatchInfo(args.MatchCardIndate);
        if (info == null)
        {
            Debug.LogError("매치 정보를 불러오는 데 실패했습니다");
            return;
        }

        // 매치 정보 저장 
        nowMatchType = info.matchType;
        nowModeType = info.matchModeType;
        numOfClient = int.Parse(info.headCount);
    }

    // 재접속
    public void ProcessReconnet() 
    {
        Debug.Log("재접속 프로세스 진입");
        if (roomInfo == null) 
        {
            Debug.LogError("재접속 할 룸 정보가 존재하지 않습니다");
            return;
        }

        if(sessionIdList != null) 
        {
            Debug.Log("이전 세션 저장 정보 : " + sessionIdList.Count);
            sessionIdList.Clear();
        }

        ErrorInfo errorInfo;

        if(!Backend.Match.JoinGameServer(roomInfo.host , roomInfo.port,true, out errorInfo))
        {
            Debug.Log(string.Format("인게임 접속 실패 : {0} - {1}", errorInfo.ToString(), string.Empty));
        }

        isConnectMatchServer = true;
        isJoinGameRoom = false;
        isReconnectProcess = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using BackEnd.Tcp;
using System.Linq;
using System;
using Battlehub.Dispatcher;

#region 클래스
// 콘솔에서 생성한 매칭 카드 정보 
public class MatchInfo
{
    public string title;                // 매칭 명
    public string inDate;               // 매칭 inDate (UUID)
    public MatchType matchType;         // 매치 타입
    public MatchModeType matchModeType; // 매치 모드 타입
    public string headCount;            // 매칭 인원
    public bool isSandBoxEnable;        // 샌드박스 모드 (AI매칭)
}

public class ServerInfo
{
    public string host;
    public ushort port;
    public string roomToken;
}
#endregion


public partial class BackEndMatchManager : MonoBehaviour
{
    private static BackEndMatchManager instance;

    // 매칭서버에 접속했는지 
    public bool isConnectMatchServer { get; private set; } = false;
    // 매치에 참가중인 유저들의 세션 목록 (세선:각각의 클라이언트)
    public List<SessionId> sessionIdList { get; private set; }
    // 매치에 참가한 유저의 총 수
    private int numOfClient = 2;

    // 인게임에 접속할 때 필요한 정보들 (뇌피셜)
    public bool isConnectMathServer { get; private set; } = false;
    // private bool isConnectInGameServer = false;
    private bool isJoinGameRoom = false;
    // 게임 룸 토큰 (인게임 접속 토큰)
    private string InGameRoonToken = string.Empty;
    public bool isSandBoxGame { get; private set; } = false;
    public bool isReconnectProcess { get; private set; } = false;

    // 게임 룸 정보 
    private ServerInfo roomInfo = null;


    // 콘솔에서 생성한 매칭 카드들의 리스트
    public List<MatchInfo> matchInfos { get; private set; } = new List<MatchInfo>();

    public static BackEndMatchManager GetInstance()
    {
        if (!instance)
        {
            //Debug.LogError("BackEndMatchManager 인스턴스가 존재하지 않습니다.");
            return null;
        }

        return instance;
    }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
    }

    // 매치 리스트 조회
    public void GetMatchList(Action<bool, string> func) 
    {
        // 매칭 카드 정보 초기화
        matchInfos.Clear();

        // 매칭 카드 정보 조회 (비동기)
        Backend.Match.GetMatchList(callback =>
        {
            // 요청 실해파면 재요청
            if (callback.IsSuccess() == false)
            {
                Debug.Log("매칭 카드 리스트 불러오기 실패\n" + callback);

                // 다시실행
                Dispatcher.Current.BeginInvoke(() =>
                {
                    GetMatchList(func);
                });
                return;
            }

            // callBack값 출력
            Debug.Log("매칭 카드 정보 callBack 출력 : " + callback);
            Debug.Log("매칭 카드 정보 callBack 출력 (타입 Json) : " + callback.ReturnValue);

            // matchInfo리스트에 값 넣어주기
            // litJson -> 역직렬화
            // Rows : 행 값을 jsonData로
            foreach (LitJson.JsonData row in callback.Rows())
            {
                Debug.Log("매칭카드 행 출력 : \n" + row);
                MatchInfo matchInfo = new MatchInfo();
                matchInfo.title = row["matchTitle"]["S"].ToString();
                matchInfo.inDate = row["inData"]["S"].ToString();
                matchInfo.headCount = row["matchHeadCount"]["N"].ToString();
                matchInfo.isSandBoxEnable = row["enable_sandBox"]["Bool"].ToString().Equals("true") ? true : false;

                // 매치 타입 enum이랑 비교, 타입지정
                foreach (MatchType type in Enum.GetValues(typeof(MatchType)))
                { 
                    string typeStr = row["matchType"]["S"].ToString().ToLower();
                    if (type.ToString().ToLower().Equals(typeStr))
                        matchInfo.matchType = type;
                }

                // 매치 모드 타입 enum이랑 비교, 타입 지정
                foreach (MatchModeType type in Enum.GetValues(typeof(MatchModeType))) 
                {
                    string typeStr = row["matchModeType"]["S"].ToString().ToLower();
                    if (type.ToString().ToLower().Equals(typeStr))
                        matchInfo.matchModeType = type;
                }

                // 카드 리스트에 넣기
                matchInfos.Add(matchInfo);
            }

        });

        Debug.Log("매칭 카드 불러오기 성공 : " + matchInfos.Count );
        func(true, string.Empty);
    }

    /// <summary>
    /// 매칭 리스트에서 indate값이 같은 매치정보 return
    /// </summary>
    /// <param name="indate">indate값</param>
    /// <returns>매치정보</returns>
    public MatchInfo GetMatchInfo(string indate) 
    {
        MatchInfo result = matchInfos.FirstOrDefault(x => x.inDate == indate);
        if (result.Equals(default(MatchInfo)) == true) 
        {
            return null;
        }
        return result;
    }
}

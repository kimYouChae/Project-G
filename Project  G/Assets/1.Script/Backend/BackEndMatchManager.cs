using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using BackEnd.Tcp;

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

public partial class BackEndMatchManager : MonoBehaviour
{
    private static BackEndMatchManager instance;

    // 매칭서버에 접속했는지 
    public bool isConnectMatchServer { get; private set; } = false;

    // 콘솔에서 생성한 매칭 카드들의 리스트
    public List<MatchInfo> matchInfos { get; private set; } = new List<MatchInfo>();
}

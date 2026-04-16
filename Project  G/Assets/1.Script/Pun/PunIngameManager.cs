using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InGamePlayer 
{
    // 같은 방 내에 플레이어가 공유해야할 데이터
    [SerializeField] private int actorNum;
    [SerializeField] private long steamID;
    [SerializeField] private string nickName;
    [SerializeField] private float bestScore;

    public InGamePlayer(int actorNum, long id , string nickName, float score)
    {
        this.actorNum = actorNum;
        this.steamID = id;
        this.nickName = nickName;
        this.bestScore = score;
    }

    public int ActorNum { get => actorNum;}
    public long SteamID { get => steamID; }
    public string NickName { get => nickName; }
    public float Score { get => bestScore;  }

    public void PrintPlayer() 
    {
        Debug.Log($"{actorNum} 에 해당하는 플레이어 정보 \n" +
            $": 닉네임 : {nickName} \n" +
            $": 스팀 ID : {steamID}" +
            $": 최고 점수 : {bestScore}");
    }
}

public class PunIngameManager : Singleton<PunIngameManager>
{

    [Header("===플레이어 스폰===")]
    [SerializeField] private PhotonView localPlayer;
    [SerializeField] private QuadrantType localQuadrantType;
    [SerializeField] private Transform[] playerField;       // 사분면 순서대로 배치되어 있어야함 

    [SerializeField] public SpawnerManager spawnerManager;

    [SerializeField]
    private Dictionary<int, InGamePlayer> ingamePlayer;
    [SerializeField]
    private List<InGamePlayer> ingamePlayerList;

    const string DEFAULT_PLAYER = "Player"; // 플레이어 상위 폴더 명 

    public QuadrantType LocalQuadrantType { get => localQuadrantType;  }
    public Transform[] PlayerField { get => playerField; }
    public List<InGamePlayer> IngamePlayerList { get => ingamePlayerList; }

    protected override void Singleton_Awake()
    {

    }

    private void Start()
    {
        spawnerManager = GetComponent<SpawnerManager>();

        ingamePlayer = new Dictionary<int, InGamePlayer>();

        // 호스트만 - 게임 (고유) 아이디 동기화
        if (PhotonNetwork.IsMasterClient)
        {
            // 게임 ID 동기화 
            SycnGameId(); 
        }
        // 로컬 플레이어 생성, 동기화 
        CreateAndSyncLocalPlayer();

        // BGM 끄기
        BGMManager.Instance.StopBGM();

        // 게임 시작 
        StartCoroutine(GameStartCorutine());
    }

#if DEV_BUILD_TEST

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12)) 
        {
            Debug.Log("[DEV_BUILD_TEST] 테스트용, 게임씬에서 로비씬으로 넘어감");
            PhotonSceneManager.Instance.ChangeScene(SceneType.Lobby);
        }
    }
#endif

    private void SycnGameId()
    {
        Debug.Log("[GameIdSync]게임 고유 ID Raise 이벤트");

        MapType maptype = GetMapType();
        string gameID = GameID(maptype);
        // 게임 ID, 맵 타입 동기화 
        object[] contcnt = new object[]
        {
            gameID,
            maptype
        };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOption = new SendOptions { Reliability = true };

        bool success =
            PhotonNetwork.RaiseEvent((byte)PunEventType.GameIdSync,
                contcnt,
                raiseEventOptions,
                sendOption);

        Debug.Log($"[GameIdSync] RaiseEvent 보냄? {success}");
    }

    IEnumerator GameStartCorutine ()
    {
        // 로딩 UI ON
        InGameUI.Instance.LoadingPanel.SetActive(true);

        // 딕셔너리에 들어온 count가 방의 입장인원과 같으면 -> 게임 시작 
        while (true) 
        {
            if (ingamePlayer.Count == PhotonNetwork.CurrentRoom.PlayerCount)
                break;

            yield return null;
        }

        // 로딩 UI OFF
        InGameUI.Instance.LoadingPanel.SetActive(false);

        // 시작 시간 지정
        double startTime = PhotonNetwork.Time + 3.0;

        Debug.Log("시작시간 + " + startTime);
        int prevSec = -1;
        while (true)
        {
            double remain = startTime - PhotonNetwork.Time;
            int sec = Mathf.CeilToInt((float)remain); // 항상 올림 처리

            if (sec != prevSec && remain > 0)
            {
                // UI 표시 
                InGameUI.Instance.CountDownUpdateText(sec);
                // 카운트다운 사운드
                SFXManager.Instance.PlaySFX(SFXType.Countdown);

                Debug.Log(sec);
                prevSec = sec;
            }

            if (remain <= 0)
                break;

            yield return null; // 매 프레임 검사
        }

        Debug.Log("게임 시작!");

        // 게임시작 사운드
        SFXManager.Instance.PlaySFX(SFXType.GameStart);

        // 게임 BGM 실행 ( 현재는 Forest 
        BGMManager.Instance.PlayBGM(BGMType.MapForest);

        // UI 업데이트
        InGameUI.Instance.CountDownText.gameObject.SetActive(false);
        InGameUI.Instance.GamePanel.SetActive(true);

        // 로직실행
        localPlayer.GetComponent<NetPlayer>().IsReadToMove = true;
        ScoreManager.Instance.ScoreBegin((float)PhotonNetwork.Time);
    }

    private void CreateAndSyncLocalPlayer() 
    {
        if (PhotonNetwork.InRoom)
        {
            // 고유한 ActorNum을 가짐 (1부터시작)
            int index = PhotonNetwork.LocalPlayer.ActorNumber - 1;
            // 보통 호스트가 1으로 설정되는듯

            QuadrantType quType = (QuadrantType)index;
            Vector2 playerPosi = Define.twoMemberPoint[quType];

            // Resources 파일 하위에 동일한 이름의 오브젝트가 있어야함 ! 
            GameObject temp = PhotonNetwork.Instantiate(PlayerPath(), playerPosi, Quaternion.identity);
            temp.GetComponent<NetPlayer>().SetIndex(quType);

            // 로컬 플레이어 저장 
            localPlayer = temp.GetComponent<PhotonView>();
            localQuadrantType = quType;
            UserDataRaiseEvent(index);

            // 스포너 manager에 로컬 플레이어 저장
            spawnerManager.SetLoacalPlayer(localPlayer);
        }
    }

    private string PlayerPath() 
    {
        CharacterType type = UserDataManager.Instance.CharacterType;
        return DEFAULT_PLAYER + "/" + type.ToString();
    }

    private void UserDataRaiseEvent(int actorNum) 
    {
        Debug.Log("[UserDataSync] 유저데이터Raise이벤트");

        // 현재 방 정보의 커스텀 정보에 접근 (포톤 hashTable에서 matType검사)
        MapType currMapType = GetMapType();

        // actor number, 스팀 아이디 , 닉네임, 최고점수
        object[] contcnt = new object[]
        {
            actorNum,
            UserDataManager.Instance.SteamID,
            UserDataManager.Instance.NickName,
            UserDataManager.Instance.ReturUserScore(currMapType)
        };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOption = new SendOptions { Reliability = true };

        bool success = PhotonNetwork.RaiseEvent((byte)PunEventType.UserDataSync,
            contcnt,
            raiseEventOptions,
            sendOption);

        Debug.Log($"[UserDataSync] RaiseEvent 보냄? {success}");
    }

    public void AddInGamePlayer(int actorNum, InGamePlayer player) 
    {
        Debug.Log("인게임Player딕셔너리에 추가");
        ingamePlayer.Add(actorNum, player);
        ingamePlayerList.Add(player);

        // 인게임 UI 업데이트 
        InGameUI.Instance.gameUI.UpdatePlayerInfoText(player);
    }

    private string GameID(MapType maptype) 
    {
        string gameId =
            $"{maptype}_{DateTime.UtcNow:yyMMddHHmmss}_{UnityEngine.Random.Range(1000, 9999)}";

        // ex) FOREST_260206173522_4821 타입 
        return gameId ;
    }

    #region Photon 관련 공통함수 

    public MapType GetMapType() 
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("MapType", out object type))
        {
            string typeString = (string)type;
            return Extension.StringToEnum<MapType>(typeString);
        }

        return MapType.None;
    }

    #endregion
}

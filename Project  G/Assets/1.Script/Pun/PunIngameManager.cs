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
    [SerializeField] private int actorNum;
    [SerializeField] private string nickName;
    [SerializeField] private float score;
    [SerializeField] private string indate;

    public InGamePlayer(int actorNum, string nickName, float score, string indate)
    {
        this.actorNum = actorNum;
        this.nickName = nickName;
        this.score = score;
        this.indate = indate;
    }

    public int ActorNum { get => actorNum;}
    public string NickName { get => nickName; }
    public float Score { get => score;  }
    public string Indate { get => indate;}

    public void PrintPlayer() 
    {
        Debug.Log($"{actorNum} 에 해당하는 플레이어 정보 : {nickName} : {score} : {indate}");
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
    private List<InGamePlayer> ingamePlayerList;        // 인스펙터용 리스트 

    [Header("===GAME ID===")]
    private string gameIDGuid;


    const string DEFAULT_PLAYER = "Player"; // 플레이어 상위 폴더 명 

    public string GameIdGuid { get => gameIDGuid; set { gameIDGuid = value; } }
    public QuadrantType LocalQuadrantType { get => localQuadrantType;  }
    public Transform[] PlayerField { get => playerField; }
    public InGamePlayer inGamePlayer(int num) 
    {
        if(ingamePlayer.ContainsKey(num))
            return ingamePlayer[num];

        return null;
    } 


    protected override void Singleton_Awake()
    {

    }

    private void Start()
    {
        spawnerManager = GetComponent<SpawnerManager>();

        ingamePlayer = new Dictionary<int, InGamePlayer>();

        MemberTwoCreatePlayer();

        StartCoroutine(Test());

        SycnGameId();
    }

    private void SycnGameId()
    {
        // 호스트만 - 게임 (고유) 아이디 동기화
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("게임 고유 ID Raise 이벤트");

            string id = Guid.NewGuid().ToString();
            object[] contcnt = new object[]
            {
            id
            };

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            SendOptions sendOption = new SendOptions { Reliability = true };

            bool success =
                PhotonNetwork.RaiseEvent((byte)PunEventType.GameIdSync,
                    contcnt,
                    raiseEventOptions,
                    sendOption);

            Debug.Log($"[Photon] RaiseEvent 보냄? {success}");
        }
    }

    IEnumerator Test ()
    {
        // 로딩 UI ON
        InGameUI.GetInstance().LoadingPanel.SetActive(true);

        // 딕셔너리에 들어온 count가 방의 입장인원과 같으면 -> 게임 시작 
        while (true) 
        {
            if (ingamePlayer.Count == PhotonNetwork.CurrentRoom.PlayerCount)
                break;

            yield return null;
        }

        // 로딩 UI OFF
        InGameUI.GetInstance().LoadingPanel.SetActive(false);

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
                InGameUI.GetInstance().CountDownUpdateText(sec);
                Debug.Log(sec);
                prevSec = sec;
            }

            if (remain <= 0)
                break;

            yield return null; // 매 프레임 검사
        }

        Debug.Log("게임 시작!");

        InGameUI.GetInstance().CountDownText.gameObject.SetActive(false);
        InGameUI.GetInstance().GamePanel.SetActive(true);
        localPlayer.GetComponent<NetPlayer>().IsReadToMove = true;
        ScoreManager.Instance.ScoreBegin((float)PhotonNetwork.Time);
    }

    private void MemberTwoCreatePlayer() 
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
        Debug.Log("유저데이터Raise이벤트");

        MapType currMapType = GetMapType();

        // 현재 타입에 해당하는 맵 타입
        // 현재 방 정보의 커스텀 정보에 접근 (hashTable에서 matType검사)

        object[] contcnt = new object[]
        {
            actorNum,
            UserDataManager.Instance.UserData.NickName,
            UserDataManager.Instance.UserData.MapTypeToScore[currMapType],
            UserDataManager.Instance.BroIndate
        };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOption = new SendOptions { Reliability = true };

        PhotonNetwork.RaiseEvent((byte)PunEventType.UserDataSync,
            contcnt,
            raiseEventOptions,
            sendOption);
    }

    public void AddInGamePlayer(int actorNum, InGamePlayer player) 
    {
        Debug.Log("인게임Player딕셔너리에 추가");
        ingamePlayer.Add(actorNum, player);
        ingamePlayerList.Add(player);

        InGameUI.GetInstance().UpdatePlayerInfoText(player);
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

using BackEnd;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[System.Serializable]
public class UserData 
{
    private string nickName;        // 닉네임
    private Dictionary<MapType, float> mapTypeToScore;  // 맵 타입별 점수 
    private Dictionary<MapType, int> mapTypeToStage;    // 맵 타입별 스테이지 

    public string NickName { get => nickName; set => nickName = value; }
    public Dictionary<MapType, float> MapTypeToScore { get => mapTypeToScore; set => mapTypeToScore = value; }
    public Dictionary<MapType, int> MapTypeToStage { get => mapTypeToStage; set => mapTypeToStage = value; }

    public UserData(Dictionary<MapType, float> scoreDic, Dictionary<MapType, int> stageDic) 
    { 
        this.mapTypeToScore = scoreDic;
        this.mapTypeToStage = stageDic;
    }

    public void SettingTypeByScoreRound(MapType type, float score)
    {
        if (mapTypeToScore.ContainsKey(type))
        {
            mapTypeToScore[type] = (float)Math.Round(score, 2);
        }
    }

    public void SettingTypeByStage(MapType type, int stage) 
    {
        if (mapTypeToStage.ContainsKey(type)) 
        {
            mapTypeToStage[type] = stage;
        }
    }

    public int UserStageByType(MapType type) 
    {
        return mapTypeToStage[type];
    }

    // 정보 출력
    public void PrintUser() 
    {
        StringBuilder sb    = new StringBuilder();
        sb.Append( "**유저닉네임 : " + nickName +  "\n");
        foreach(var temp in mapTypeToScore) 
        {
            sb.Append("맵 타입 : " + temp.Key + " | 점수 : " + temp.Value);
        }
        foreach(var temp in mapTypeToStage) 
        {
            sb.Append(" | 스테이지 : " + temp.Value);
        }
        Debug.Log(sb);
    }
}

public class UserDataManager : Singleton<UserDataManager>
{
    // 유저데이터
    [SerializeField]
    private UserData userData;
    [SerializeField]
    string gameDataRowIndate; // 테이블에 삽입한 게임 정보 고유값 
    [SerializeField] 
    private CharacterType characterType;
    [SerializeField]
    private BackendReturnObject localPlayerInfo;    // 로컬 유저 backend 오브젝트

    public UserData UserData { get => userData;  }
    public CharacterType CharacterType { get => characterType; set { characterType = value; } }
    public BackendReturnObject LocalPlayerBro { get => localPlayerInfo; set { localPlayerInfo = value; } }
    public string BroIndate { get => localPlayerInfo.GetInDate(); }

    protected override void Singleton_Awake()
    {
        SetUpDontDestroy();
    }

    private void Start()
    {
        Dictionary<MapType, float > typeScore = new Dictionary<MapType, float>()
        {
            { MapType.Forest , 0 },   
            { MapType.GiganticTree , 0 },   
            { MapType.Island , 0 },  
            { MapType.Market, 0 },  
            { MapType.Hell , 0 },  
            { MapType.IceVillage , 0 }  
        };
        Dictionary<MapType, int> typeStage = new Dictionary<MapType, int>()
        {
            { MapType.Forest , 0 },
            { MapType.GiganticTree , 0 },
            { MapType.Island , 0 },
            { MapType.Market, 0 },
            { MapType.Hell , 0 },
            { MapType.IceVillage , 0 }
        };
        userData = new UserData(typeScore, typeStage);
    }

    private Param GetUserDataParam(string nickName)
    {
        Param param = new Param();
        param.Add("UserNickName", nickName);
        param.Add("MapByScore" , userData.MapTypeToScore);
        param.Add("MapByStage", userData.MapTypeToStage);
        return param;
    }

    // 1회 입력받은 닉네임으로 데이터테이블에 저장 
    public void InsertToUserTable(string nickName) 
    {
        Param param = GetUserDataParam(nickName);
        var bro = Backend.GameData.Insert(Define.USERTABLE, param);

        if (bro.IsSuccess())
        {
            Debug.Log("게임 정보 데이터 삽입에 성공 했습니다 " + bro);

            // 삽입한 데이터의 고유값 
            gameDataRowIndate = bro.GetInDate();
        }
        else 
        {
            Debug.Log("게임 정보 삽입에 실패 했습니다" + bro);
        }
    }

    public void GetUserDataInTable() 
    {
        Debug.Log("게임 정보 조회 함수를 실행합니다");

        // 조건 세팅
        Where where = ConditionIsOwnerDataisLocal();

        // 테이블명, where절, 불러올 게임정보 row 갯수
        BackendReturnObject bro = Backend.GameData.GetMyData(Define.USERTABLE , where, 10);

        Debug.Log("!!!!!!!!!!!!@@ indate" + bro.GetInDate());

        if(bro.IsSuccess()) 
        {
            // Json으로 리턴된 데이터 받아오기 
            string gameJson = bro.ReturnValue;
            Debug.Log(gameJson);

            // if (gameJson.Equals(string.Empty))
            //    return;

            LitJson.JsonData gamedataJson = bro.FlattenRows();
            if (gamedataJson.Count <= 0) 
            {
                Debug.Log("불러올 데이터가 존재 X");
            }
            else 
            {
                // 불러온 게임 정보의 고유값
                string gameDataInrow = gamedataJson[0]["inDate"].ToString();

                // 0. 닉네임
                string nickName = gamedataJson[0]["UserNickName"].ToString();

                // 2. 맵 별 점수 
                Dictionary<MapType, float> typeByScore = new Dictionary<MapType, float>();
                foreach (string mapKey in gamedataJson[0]["MapByScore"].Keys) 
                {
                    JsonData value = gamedataJson[0]["MapByScore"][mapKey];

                    if(System.Enum.TryParse(mapKey , out MapType keyType)) 
                    { 
                        typeByScore.Add(keyType, float.Parse(value.ToString()));
                    }
                }

                // 3. 맵 별 스테이지 
                Dictionary<MapType, int> typeByStage = new Dictionary<MapType, int>();
                foreach (string mapKey in gamedataJson[0]["MapByStage"].Keys)
                {
                    JsonData value = gamedataJson[0]["MapByStage"][mapKey];

                    if (System.Enum.TryParse(mapKey, out MapType keyType))
                    {
                        typeByStage.Add(keyType, int.Parse(value.ToString()));
                    }
                }

                userData.NickName = nickName;
                userData.MapTypeToScore = typeByScore;
                userData.MapTypeToStage = typeByStage;
            }

            // 유저 프린트
            userData.PrintUser();
 
        }
    }

    // 점수 + 스테이지 달정 점수 저장
    public void SettingAchiveData(float score, int stage) 
    {
        MapType type = PunIngameManager.Instance.GetMapType();

        if (type == MapType.None)
            return;

        userData.SettingTypeByScoreRound(type, score);
        userData.SettingTypeByStage(type, stage);
    }

    // 유저테이블에 정보 업데이트
    public void UpdateUserData() 
    {
        // MapByScore 칼럼의 값을 수정
        Param param = new Param();
        param.Add("MapByScore", userData.MapTypeToScore);
        param.Add("MapByStage", userData.MapTypeToStage);

        // 조건 세팅
        Where where = ConditionIsOwnerDataisLocal();

        // 업데이트
        Backend.GameData.Update(Define.USERTABLE, where, param);
    }

    private Where ConditionIsOwnerDataisLocal() 
    {
        // owner_inData 칼럼이 "로컬에 저장된 returnObject의 inData"
        Where where = new Where();
        where.Equal("owner_inDate", BackEndServerManager.Instance.PlayerInfo.GetInDate());
        return where;
    }
}

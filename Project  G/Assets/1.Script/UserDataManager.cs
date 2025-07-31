using BackEnd;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Android;

[System.Serializable]
public class UserData 
{
    private CharacterType userAppearType;    // 외형 인덱스 
    private Dictionary<MapType, float> mapTypeToScore;  // 맵 타입별 점수 

    public CharacterType UserAppearType { get => userAppearType; set => userAppearType = value; }
    public Dictionary<MapType, float> MapTypeToScore { get => mapTypeToScore; set => mapTypeToScore = value; }

    // 정보 출력
    public void PrintUser() 
    {
        StringBuilder sb    = new StringBuilder();
        sb.Append("**유저번호 : " + userAppearType + "\n");
        foreach(var temp in mapTypeToScore) 
        {
            sb.Append("맵 타입 : " +  temp.Key + " | 점수 : " + temp.Value );
        }
        Debug.Log(sb);
    }
}

public class UserDataManager : MonoBehaviour
{
    // 싱글톤 
    private static UserDataManager instance;

    // 유저데이터
    [SerializeField]
    private UserData userData;
    // 테이블에 삽입한 게임 정보 고유값 
    [SerializeField]
    string gameDataRowIndate;

    public UserData UserData { get => userData;  }

    public static UserDataManager GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("UserDataManager 인스턴스가 존재하지 않습니다.");
            return null;
        }
        return instance;
    }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        userData = new UserData();
        userData.UserAppearType = CharacterType.None;
        Dictionary<MapType, float > dic = new Dictionary<MapType, float>()
        {
            { MapType.Forest , 0 },   
            { MapType.GiganticTree , 0 },   
            { MapType.Island , 0 },  
            { MapType.Market, 0 },  
            { MapType.Hell , 0 },  
            { MapType.IceVillage , 0 }  
        };
        userData.MapTypeToScore = dic;
    }


    public void SetCharacterIndex(int index) 
    {
        // 아무것도 선택 안되면 제일 기본 캐릭터
        if ((CharacterType)index == CharacterType.None)
            index = 0;

        userData.UserAppearType = (CharacterType)index;
    }

    private Param GetUserDataParam()
    {
        Param param = new Param();
        param.Add("UserApreaIndex",userData.UserAppearType);
        param.Add("MapByScore" , userData.MapTypeToScore);
        return param;
    }

    // 처음 들어온 유저 한정 -> 테이블에 데이터 넣기 
    public void InsertToUserTable() 
    {
        Param param = GetUserDataParam();
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

        Where where = new Where();
        where.Equal("owner_inDate" , BackEndServerManager.GetInstance().PlayerInfo.GetInDate());
        // owner_inData 칼럼이 "로컬에 저장된 returnObject의 inData"

        // 테이블명, where절, 불러올 게임정보 row 갯수
        BackendReturnObject bro = Backend.GameData.GetMyData(Define.USERTABLE , where, 10);

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


                // 1. 외형 인덱스
                string apreadIndex = gamedataJson[0]["UserApreaIndex"].ToString();

                // 2. 맵 별 점수 
                Dictionary<MapType, float> tempDic = new Dictionary<MapType, float>();

                foreach (string mapKey in gamedataJson[0]["MapByScore"].Keys) 
                {
                    JsonData value = gamedataJson[0]["MapByScore"][mapKey];

                    if(System.Enum.TryParse(mapKey , out MapType keyType)) 
                    { 
                        tempDic.Add(keyType, float.Parse(value.ToString()));
                    }
                }

                userData.UserAppearType = (CharacterType)(int.Parse(apreadIndex));
                userData.MapTypeToScore = tempDic;
            }

            // 유저 프린트
            userData.PrintUser();
 
        }
    }
}

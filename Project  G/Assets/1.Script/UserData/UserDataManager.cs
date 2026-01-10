using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataManager : Singleton<UserDataManager>
{
    [SerializeField] 
    private UserData userdata = new UserData();

    public string NickName { get => userdata.NickName; }
    public CharacterType CharacterType { get => userdata.CharacterType; set { userdata.CharacterType = value; } }

    protected override void Singleton_Awake()
    {
        SetUpDontDestroy();          
    }

    public void InsertUserInfo(string id, string name, string cnt) 
    {
        userdata.SteamID = id;
        userdata.NickName = name;
        userdata.Country = cnt;
    }

    public void SetScoreByMapType(MapType type, float score)
    {
        if (userdata.ScoreByMaptype.ContainsKey(type))
        {
            userdata.ScoreByMaptype[type] = score;
        }
        else 
        {
            Debug.Log($"UserData의 ScoreByMapType 딕셔너리에 값을 넣지 못함 {type} : {score}");
        }
    }

    public int ReturnUserStage(MapType type)
    {
        if (userdata.StageByMaptype.ContainsKey(type))
        {
            return userdata.StageByMaptype[type];
        }

        return -1;
    }

    public float ReturUserScore(MapType type)
    {
        if (userdata.ScoreByMaptype.ContainsKey(type))
        {
            return userdata.ScoreByMaptype[type];
        }

        return -1;
    }
}

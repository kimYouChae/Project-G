using BackEnd;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDataManager : Singleton<ScoreDataManager>
{
    const string VerticalBar = "|";

    public void InserToLeaderBoardTable(float score) 
    {
        // 현재 맵 타입    
        // 현재 방 정보의 커스텀 정보에 접근 (hashTable에서 matType검사)
        MapType type = PunIngameManager.Instance.GetMapType();

        // 유저들의 indate값
        // "유저1 Indate" + | + "유저2 Indate"
        string indates = UserIndates();

        Param param = new Param();
        param.Add("Score", score);
        param.Add("UserIndates" , indates);

        // 테이블에 넣기 
        var bro = Backend.GameData.Insert(Define.MAPTYPEBY_LEADERBOARD_TABLE[type], param);
        if (bro.IsSuccess())
        {
            Debug.Log($"{this.name} 데이터 삽입에 성공 했습니다 " + bro);
        }
        else
        {
            Debug.Log($"{this.name} 데이터 삽입에 실패 했습니다" + bro);
        }
    }

    protected override void Singleton_Awake()
    {

    }

    private string UserIndates() 
    {
        string userIndates = string.Empty;
        try
        {
            string indate = PunIngameManager.Instance.inGamePlayer(0).Indate;
            userIndates += indate;
        }
        catch (Exception ex)
        {
            Debug.LogError("첫번째 유저의 Indate를 가져올 수 없습니다!" + ex.Message);
        }

        // 중간에 | 추가 (LeaderBoard 의 "추가항목" 기능을 사용하기 위해서 )
        userIndates += VerticalBar;

        try
        {
            string indate = PunIngameManager.Instance.inGamePlayer(1).Indate;
            userIndates += indate;
        }
        catch (Exception ex)
        {
            Debug.LogError("두번째 유저의 Indate를 가져올 수 없습니다!" + ex.Message);
        }

        return userIndates;
    }
}

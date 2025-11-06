using BackEnd;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ScoreDataManager : Singleton<ScoreDataManager>
{

    const string VerticalBar = "|";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="score"></param>
    /// <returns> item1 : insert한 행의 indate 값 |
    /// item2 : extraData (다른유저 indate 값 + 고유 키 값) </returns>
    public Tuple<string,string> InserToLeaderBoardTableAndReturnIndate(MapType type, float score) 
    {
        // insert한 행의 indate
        string rowIndate = string.Empty;

        // 로컬 유저 제외한 유저의 indate + Guid ( 고유 키 )
        string extraData = GetExtraData();

        Param param = new Param();
        param.Add("Score", score);
        param.Add("ExtraData" , extraData);

        // 테이블에 넣기 
        var bro = Backend.GameData.Insert(Define.MAPTYPEBY_LEADERBOARD_TABLE[type], param);
        if (bro.IsSuccess())
        {
            rowIndate = bro.GetInDate();
            Debug.Log($"{this.name} 데이터 삽입에 성공 했습니다 " + bro);
        }
        else
        {
            Debug.Log($"{this.name} 데이터 삽입에 실패 했습니다" + bro);
        }

        return Tuple.Create(rowIndate, extraData);
    }

    protected override void Singleton_Awake()
    {

    }

    private string GetExtraData() 
    {
        string extraData = string.Empty;

        // 내 indate
        string localIndate = UserDataManager.Instance.LocalPlayerBro.GetInDate();
        // 다른사람 indate
        string anotherIndate = string.Empty;

        // inGamePlayer의 indate에 접근해서 내 indate가 아닌 다른 사람의 indate 합치기
        for (int i = 0; i < 2; i++)
        {
            string indate = PunIngameManager.Instance.inGamePlayer(i).Indate;

            if (localIndate.Equals(indate))
                continue;

            anotherIndate = indate;
        }

        // Guid로 고유 키 값 구하기 
        string key = PunIngameManager.Instance.GameIdGuid;

        // extraData 완성
        // | : LeaderBoard 의 "추가항목" 기능을 사용하기 위해서 
        extraData = $"{anotherIndate}|{key}";

        // 몇바이트인지 디버깅 (뒤끝의 extraData는 265 byte 까지만 가능)
        Debug.Log($"extraData : { extraData } / 크기(byte) : {Encoding.UTF8.GetByteCount(extraData)}");

        /*
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
        */

        return extraData;
    }

}

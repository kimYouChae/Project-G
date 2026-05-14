using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingModel : IRankingModel
{
    // 랭크 API 가 끝나면 여기에 정보 담김
    private UserRankDTO myRankDto;
    private List<UserRankDTO> rankersDto;

    // Set
    public void SetRankers(List<UserRankDTO> list)
    {
        this.rankersDto = list;
    }

    public void SetUserRanker(UserRankDTO dto)
    {
        this.myRankDto = dto;
    }

    // Get
    public List<UserRankDTO> GetRankersList()
    {
        return rankersDto;
    }

    public UserRankDTO GetUserRanker()
    {
        return myRankDto;
    }

    public void PrintUserRanker()
    {
        if(myRankDto == null) 
        {
            Debug.Log("[Ranking Model] 유저 랭킹 정보가 NULL 입니다");
            return;
        }

        Debug.Log($"[Ranking Model] 유저 랭킹 정보 \n " +
            $"닉네임 : {myRankDto.player1_nick} / {myRankDto.player2_nick} \n" +
            $"맵 타입 : {myRankDto.mapType} \n " +
            $"점수와 스테이지 : {myRankDto.score}점 , {myRankDto.stage} \n" +
            $"랭킹 : {myRankDto.ranking} \n" +
            $"날짜 : {myRankDto.createdAt}" );
    }

    public void PrintRankersList()
    {
        if (rankersDto == null)
        {
            Debug.Log("[Ranking Model] 랭커 정보가 NULL 입니다");
            return;
        }

        Debug.Log("[Ranking Model] 랭커 정보 ");
        for(int i = 0; i < rankersDto.Count; i++) 
        {
            var ranker = rankersDto[i];

            Debug.Log(
                $"닉네임 : {ranker.player1_nick} / {ranker.player2_nick} \n" +
                $"맵 타입 : {ranker.mapType} \n " +
                $"점수와 스테이지 : {ranker.score}점 , {ranker.stage} \n" +
                $"랭킹 : {ranker.ranking} \n" +
                $"날짜 : {ranker.createdAt}");
        }
    }
}

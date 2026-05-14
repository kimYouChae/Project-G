using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataModel : IGameDataModel
{
    // 게임데이터 API 가 끝나면 여기에 정보 담김
    private BestScoreUpdateResponse bsResponse;

    public BestScoreUpdateResponse GetBestScoreInfo()
    {
        return bsResponse;
    }

    public void SetGameData(BestScoreUpdateResponse response)
    {
        bsResponse = response;
    }
}

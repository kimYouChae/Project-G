using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataModel : IGameDataModel
{
    // 게임데이터 API 가 끝나면 여기에 정보 담김
    private BestScoreUpdateResponse bsResponse;

    // API 실패 시 flag
    private bool isSuccess;

    public BestScoreUpdateResponse GetBestScoreInfo()
    {
        return bsResponse;
    }

    public void SetGameData(BestScoreUpdateResponse response)
    {
        bsResponse = response;
    }

    public bool GetIsSuccess()
    {
        return isSuccess;
    }

    public void SetIsSuccess(bool flag)
    {
        isSuccess = flag;
    }
}

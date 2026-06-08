using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#region DTO
public class AchieveProgressRequest
{
    public long SteamID;
}
public class AchieveProgressResponse
{
    public AchieveType AchieveType;
    public bool isClear;
}
#endregion

public class WebUserProgressService : IUserProgressService
{
    private string achieaveProgressUrl = "";

    private IAchieveProgressModel achieveProgressModel;

    public WebUserProgressService(string url, IAchieveProgressModel achieveProgressModel)
    {
        this.achieaveProgressUrl = url;
        this.achieveProgressModel = achieveProgressModel;
    }

    public IEnumerator GetAchievementService(long uid)
    {
        AchieveProgressRequest achieveProgress = new AchieveProgressRequest()
        {
            SteamID = uid
        };

        string requestJson = JsonUtility.ToJson(achieveProgress);
        yield return WebRequestCore.CommonLogic<List<AchieveProgressResponse>>
         (
             requestJson,
             achieaveProgressUrl,
             HttpRequestType.Post,
             AchieveProgressParsing,
             () => AchieveProgressFailed()
         );
    }

    private void AchieveProgressParsing(List<AchieveProgressResponse> apiResponse)
    {
        // achieve Progress 모델에 값 넣기
        achieveProgressModel.SetGameData(apiResponse);

        // Api 성공 flag
        achieveProgressModel.SetIsSuccess(true);
    }

    private void AchieveProgressFailed()
    {
        // APi 실패 flag
        achieveProgressModel.SetIsSuccess(false);
    }

}

using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#region DTO
public class AchiveProgressRequest
{
    public long SteamID;
}
public class AchiveProgressResponse
{
    public AchiveType AchiveType;
    public bool isClear;
}
#endregion

public class WebUserProgressService : IUserProgressService
{
    private string achiveProgressUrl = "";

    private IAchiveProgressModel achiveProgressModel;   

    public WebUserProgressService(string url, IAchiveProgressModel achiveProgressModel)
    {
        this.achiveProgressUrl = url;
        this.achiveProgressModel = achiveProgressModel;
    }

    public IEnumerator GetAchivementService(long uid)
    {
        AchiveProgressRequest achiveProgress = new AchiveProgressRequest()
        {
            SteamID = uid
        };

        string requestJson = JsonUtility.ToJson(achiveProgress);
        yield return WebRequestCore.CommonLogic<List<AchiveProgressResponse>>
         (
             requestJson,
             achiveProgressUrl,
             HttpRequestType.Post,
             AchiveProgressPasing,
             () => AchiveProgressFailed()
         );
    }

    private void AchiveProgressPasing(List<AchiveProgressResponse> apiResponse) 
    {
        // achive Progress 모델에 값 넣기
        achiveProgressModel.SetGameData(apiResponse);

        // Api 성공 flag
        achiveProgressModel.SetIsSuccess(true);
    }

    private void AchiveProgressFailed() 
    {
        // APi 실패 flag
        achiveProgressModel.SetIsSuccess(false);
    }

}

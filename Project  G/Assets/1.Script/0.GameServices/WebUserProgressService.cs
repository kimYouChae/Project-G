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
    private string baseUrl;
    private static string achiveProgressUrl = "UserProgress/AchiveProgress";

    private IAchiveProgressModel achiveProgressModel;   

    public WebUserProgressService(string url, IAchiveProgressModel achiveProgressModel)
    {
        this.baseUrl = url;
        this.achiveProgressModel = achiveProgressModel;
    }

    public IEnumerator GetAchivementService(long uid)
    {
        AchiveProgressRequest achiveProgress = new AchiveProgressRequest()
        {
            SteamID = uid
        };

        string requestJson = JsonUtility.ToJson(achiveProgress);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestJson);

        var request = new UnityWebRequest(baseUrl + achiveProgressUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.SetRequestHeader("Content-Type", "application/json");
        request.downloadHandler = new DownloadHandlerBuffer(); // 응답 바디 확인용

        yield return CoroutineHandler.Instance.Run(StartAchiveProgressRequest(request));
    }

    IEnumerator StartAchiveProgressRequest(UnityWebRequest request) 
    {
        Debug.Log($"[HTTP] 요청 코루틴 시작 URL = {request.url}");

        // 요청보내기 (비동기)
        yield return request.SendWebRequest();
        Debug.Log($"[{nameof(WebUserProgressService)}] / responseBody={request.downloadHandler?.text}");

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
            yield break;
        }

        // 받은 요청을 string 타입으로
        string responseText = request.downloadHandler.text;

        AchiveProgressPasing(responseText);
    }

    private void AchiveProgressPasing(string responseText) 
    {
        ApiResponse<List<AchiveProgressResponse>> apiResponse
            = JsonConvert.DeserializeObject<ApiResponse<List<AchiveProgressResponse>>>(responseText);

        if (apiResponse == null)
        {
            Debug.Log($"WebUserProgressService : 유저 도전과제 파싱 중에 오류 발생 , Json으로 변환 불가 \n {responseText}");
            return;
        }

        if (apiResponse.success == false)
        {
            Debug.Log($"WebUserProgressService : 유저 도전과제 불러오기 실패 \n {responseText}");
            return;
        }

        // achive Progress 모델에 값 넣기
        achiveProgressModel.SetGameData(apiResponse.data);
    }
}

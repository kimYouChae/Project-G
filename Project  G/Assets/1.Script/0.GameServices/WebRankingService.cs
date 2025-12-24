using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RequestUserRankDTO
{
    public string SteamId { get; set; }
    public int MapType { get; set; }
}

public class RequestRankerDTO
{
    public int MapType { get; set; }
}

public class WebRankingService : IRankingService
{
    private string baseUrl;
    private static string userRankUrl = "GetRank";
    private static string rankerUrl = "GetRankers";

    public WebRankingService(string url)
    {
        this.baseUrl = url;
    }

    public void GetMyRankingService(string myId, int mapType)
    {
        RequestUserRankDTO requestDTO = new RequestUserRankDTO()
        {
            SteamId = myId,
            MapType = mapType
        };

        string requestJson = JsonUtility.ToJson(requestDTO);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestJson);

        var request = new UnityWebRequest(baseUrl + userRankUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);

        CoroutineHandler.Instance.Run(StartRequest(request));
    }

    public void GetRankerService(int mapType)
    {
        RequestRankerDTO requestDTO = new RequestRankerDTO()
        {
            MapType = mapType
        };

        string requestJson = JsonUtility.ToJson(requestDTO);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestJson);

        var request = new UnityWebRequest(baseUrl + userRankUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);

        CoroutineHandler.Instance.Run(StartRequest(request));
    }

    IEnumerator StartRequest(UnityWebRequest request)
    {
        // 요청보내기 (비동기)
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
            yield break;
        }

        // 받은 요청을 string 타입으로
        string responseText = request.downloadHandler.text;
        Debug.Log(responseText);
    }

}

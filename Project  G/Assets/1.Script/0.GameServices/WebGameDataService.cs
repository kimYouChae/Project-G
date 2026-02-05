using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#region DTO

public class UpdateUserRequestDTO
{
    public string MatchId;
    public ulong MySteamId;
    public ulong PartnerId;
    public int MapType;
    public float Score;
    public int Stage;
}
#endregion

public class WebGameDataService : IGameDataService
{
    private string baseUrl;
    private static string gamdDataUrl = "GameData/Update";

    public WebGameDataService(string url)
    {
        this.baseUrl = url;
    }

    public void UpdateGameDataService(string myId, string partnerId, float score, int mapType)
    {
        UpdateUserRequestDTO updateUserInfoDTO = new UpdateUserRequestDTO() 
        {
            //MySteamId = myId,
            //PartnerId = partnerId,
            Score = score,
            MapType = mapType
        };

        string requestJson = JsonUtility.ToJson(updateUserInfoDTO);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestJson);

        var request = new UnityWebRequest(baseUrl + gamdDataUrl, "POST");
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

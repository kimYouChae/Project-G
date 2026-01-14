using System;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine;
using Newtonsoft.Json;

public class WebChartService : IChartService
{
    private string baseUrl;

    public WebChartService(string url)
    {
        this.baseUrl = url;
    }

    public void ChartService(DataType dataType)
    {
        if (dataType == DataType.None)
            return;

        // var request = new UnityWebRequest(baseUrl + dataType.ToString(), "GET");
        var request = UnityWebRequest.Get(baseUrl + dataType.ToString());

        CoroutineHandler.Instance.Run(StartRequest(request, dataType));
    }

    IEnumerator StartRequest(UnityWebRequest request, DataType dataType)
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
        
        // Chart 관련 API의 응답은 API Response 타입의 Json임 . 
        ApiResponse<object> apiResponse = JsonConvert.DeserializeObject<ApiResponse<object>>(responseText);
        if (apiResponse == null)
        {
            Debug.Log($"WebChartService : 차트 파싱 중에 오류 발생 , Json으로 변환 불가 \n {responseText}");
            yield break;
        }
        if (apiResponse.success == false)
        {
            Debug.Log($"차트 불러오기 실패. 타입 {dataType}");
            yield break;
        }

        TypeByChartHandler(dataType, responseText);
    }

    private void TypeByChartHandler( DataType dataType , string jsonStr ) 
    {
        ICharHandler chartH = ChartHandlerFactory.Instance.DataTypeByChart(dataType);

        if (chartH != null) 
        {
            chartH.IParseAndStore(jsonStr);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

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
        // Debug.Log(responseText);

        TypeByChartHandler(dataType, responseText);
    }

    private void TypeByChartHandler( DataType dataType , string jsonStr ) 
    {
        ICharHandler chartH = ChartHandlerFactory.instance.DataTypeByChart(dataType);

        if (chartH != null) 
        {
            chartH.IParseAndStore(jsonStr);
        }
    }
}

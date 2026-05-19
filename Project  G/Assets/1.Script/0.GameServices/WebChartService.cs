using System;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine;
using Newtonsoft.Json;

public class WebChartService : IChartService
{
    private string chartUrl = "";

    public WebChartService(string url)
    {
        this.chartUrl = url;
    }

    public IEnumerator ChartService(DataType dataType)
    {
        if (dataType == DataType.None)
            yield break;

        yield return WebRequestCore.CommonLogic<object>
            (
                "",
                chartUrl + dataType.ToString(),
                HttpRequestType.Get,
                (obj) => ParsingChart(obj, dataType),
                () => ParsingFailed(dataType)
            );
    }

    private void ParsingFailed(DataType dataType) 
    {
        Debug.Log($"[ {dataType} ]에 해당하는 차트 파싱 실패");
    }

    // List<MapData> 같은 object가 들어옴
    private void ParsingChart(object obj, DataType type) 
    {
        string json = JsonConvert.SerializeObject(obj);

        TypeByChartHandler(type, json);
    }

    private void TypeByChartHandler( DataType dataType , string jsonStr ) 
    {
        ICharHandler chartH = ChartHandlerFactory.Instance.DataTypeByChart(dataType);

        if (chartH != null)
        {
            chartH.IParseAndStore(jsonStr);
        }
        else 
        {
            Debug.LogError($"[WebChartService] 차트 펙토리에 {dataType} 에 해당하는 {nameof(ICharHandler)}가 없습니다");
        }
    }
}

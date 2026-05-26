using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEditor.LightingExplorerTableColumn;


public class WebChartService : IChartService
{
    private string chartUrl = "";
    private string chartVersionUrl = "";

    private string versionKey = "chartVesion";
    private int localChartVersion;      // 플레이어 프리팹으로 저장된 차트 버전
    private int serverChartVersion;     // API로 리턴되는 차트 버전
    private bool getChartFailed;         // 차트Get 할 때 성공,실패 flag

    private Dictionary<DataType, string> tempChartByjson;   // json 임시 저장용
    private string cacheDir = Path.Combine(Application.persistentDataPath, "ChartCache"); // 임시 json 로컬 저장 경로

    public WebChartService(string url, string versionUrl)
    {
        this.chartUrl = url;
        this.chartVersionUrl = versionUrl;

        // 차트 버전 가져오기 
        // 현재 저장된 버전 없으면 -1 
        localChartVersion = PlayerPrefs.GetInt(versionKey, -1);
        getChartFailed = false;

        tempChartByjson = new Dictionary<DataType, string>();
    }

    public IEnumerator ChartService()
    {
        // 버전 가져오기
        yield return WebRequestCore.CommonLogic<string>
            (
                "",
                chartVersionUrl,
                HttpRequestType.Get,
                CheckChartVersion,
                ChartVersionFailed
            );

        // 차트 버전 비교 
        // 1. -1일 때 : api 실패했을 때
        if (serverChartVersion == -1)
        {
            // 차트 fall back 실행
            ChartFallback();
            yield break;
        }

        // 2. 버전이 같으면 -> pass
        if (serverChartVersion == localChartVersion)
            yield break;
        
        // 3. 버전이 다르거나 , 로컬에 없을 때  
        // 차트 불러오기 
        DataType[] array = (DataType[])Enum.GetValues(typeof(DataType));
        for (int i = 0; i < array.Length; i++)
        {
            yield return GetChart(array[i]);

            // 만약 차트 가져오기가 하나라도 실패 한다면, 
            if(getChartFailed)
                break;
        }

        if (getChartFailed)
        {
            // 차트 fall back 실행
            ChartFallback();
            yield break;
        }

        // 차트를 로컬에 저장 ( 임시로 저장된 json을 저장하기)
        for (int i = 0; i < array.Length; i++) 
        {
            string path = Path.Combine(cacheDir, $"{array[i]}.json");
            Directory.CreateDirectory(cacheDir);
            foreach (var kv in tempChartByjson)
                File.WriteAllText(path, kv.Value);
        }

        // 버전 갱신
        PlayerPrefs.SetInt(versionKey, serverChartVersion);
        PlayerPrefs.Save();

        yield break;

    }

    private void ChartFallback()
    {
        // 1. 로컬에 저장된 JSON파일이 있는지 
        string cacheDir = Path.Combine(Application.persistentDataPath, "ChartCache");

        // 1. 캐시 있으면 캐시 사용
        if (Directory.Exists(cacheDir))
        {
            // persistentDataPath에서 읽기
            DataType[] array = (DataType[])Enum.GetValues(typeof(DataType));
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == DataType.None) 
                    continue;

                string path = Path.Combine(cacheDir, $"{array[i]}.json");
                if (File.Exists(path))
                    TypeByChartHandler(array[i], File.ReadAllText(path));
            }

            return;
        }

        // 2. 캐시가 없으면 Resource 하위 json에 접근 (완전 최후의 방법)
        TextAsset[] textAssets = ResourceManager.Instance.FallBackChartTextfile;

        for (int i = 0; i < textAssets.Length; i++)
        {
            DataType type = Extension.StringToEnum<DataType>(textAssets[i].name);

            TypeByChartHandler(type, textAssets[i].text);
        }
    }

    public IEnumerator GetChart(DataType dataType)
    {
        if (dataType == DataType.None)
            yield break;

        // 차트 가져오기
        yield return WebRequestCore.CommonLogic<object>
            (
                "",
                chartUrl + dataType.ToString(),
                HttpRequestType.Get,
                (obj) => ParsingChart(obj, dataType),
                () => ParsingFailed(dataType)
            );
    }

    private void CheckChartVersion(string version) 
    {
        if (!int.TryParse(version, out serverChartVersion))
        {
            serverChartVersion = -1;
            return;
        }

        serverChartVersion = int.Parse(version);
    }

    private void ChartVersionFailed() 
    {
        serverChartVersion = -1;
    }

    private void ParsingFailed(DataType dataType) 
    {
        Debug.Log($"[ {dataType} ]에 해당하는 차트 파싱 실패");

        getChartFailed = true;
    }

    // List<MapData> 같은 object가 들어옴
    private void ParsingChart(object obj, DataType type) 
    {
        string json = JsonConvert.SerializeObject(obj);

        // json 임시 보관 
        tempChartByjson.Add(type, json);

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

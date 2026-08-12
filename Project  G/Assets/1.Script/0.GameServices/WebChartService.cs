using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class WebChartService : IChartService
{
    private string chartUrl = "";
    private string chartVersionUrl = "";

    private string versionKey = "chartVesion";
    private int localChartVersion;      // 플레이어 프리팹으로 저장된 차트 버전
    private int serverChartVersion;     // API로 리턴되는 차트 버전
    private bool getChartFailed;         // 차트Get 할 때 성공,실패 flag

    private Dictionary<DataType, string> tempChartByjson;   // DataType에 해당하는 json 저장용
    private Dictionary<MapType, string> tempChartMap; // MapType에 해당하는 json 저장용
    private string cacheDir = Path.Combine(Application.persistentDataPath, "ChartCache"); // json 로컬 저장 경로

    public WebChartService(string url, string versionUrl)
    {
        this.chartUrl = url;
        this.chartVersionUrl = versionUrl;

        // 차트 버전 가져오기 
        // 현재 저장된 버전 없으면 -1 
        localChartVersion = PlayerPrefs.GetInt(versionKey, -1);
        getChartFailed = false;

        tempChartByjson = new Dictionary<DataType, string>();
        tempChartMap = new Dictionary<MapType, string>();
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

        Debug.Log($"[WebChartService] 로컬에 저장된 차트 버전 : {localChartVersion} " +
            $"/ \n 서버 차트 버전 : {serverChartVersion}");

        // 차트 버전 비교 
        // 1. -1일 때 : api 실패했을 때
        if (serverChartVersion == -1)
        {
            // 차트 fall back 실행
            UseCacheOrFallback();
            yield break;
        }

        // 2. 버전이 같으면 -> 차트 캐시 사용
        if (serverChartVersion == localChartVersion)
        {
            UseCacheOrFallback();
            yield break;
        }


        // 3. 버전이 다르거나 , 로컬에 없을 때  
        // DataType에 해당하는 - 차트 불러오기 
        yield return TryGetChart<DataType>(GetChart);
        // MapType에 해당하는 - 차트 불러오기
        yield return TryGetChart<MapType>(GetChart);

        if (getChartFailed)
        {
            // 차트 fall back 실행
            UseCacheOrFallback();
            yield break;
        }

        // 로컬에 저장 
        SaveLocal<DataType>(tempChartByjson);
        SaveLocal<MapType>(tempChartMap);

        // 버전 갱신
        PlayerPrefs.SetInt(versionKey, serverChartVersion);
        PlayerPrefs.Save();

        yield break;

    }

    /// <summary>
    /// 타입에 맞는 GetChart() 오버로딩 메서드를 실행
    /// Func(매개변수, 리턴값) : GetChart()메서드는 IEnumerator를 리턴함, 리턴값이 있는 func 를 사용
    /// </summary>
    private IEnumerator TryGetChart<T>(Func<T , IEnumerator> getLogic)
        where T : Enum
    {
        foreach (T arr in (T[])Enum.GetValues(typeof(T))) 
        {
            // 만약 차트 가져오기가 하나라도 실패 한다면, 
            if (getChartFailed)
                break;

            yield return getLogic?.Invoke(arr);
        }
    }

    private void SaveLocal<T>(Dictionary<T, string> keyValuePairs)
        where T : Enum
    {
        T[] array = (T[])Enum.GetValues(typeof(T));

        // 차트를 로컬에 저장 ( 임시로 저장된 json을 저장하기)
        for (int i = 0; i < array.Length; i++)
        {
            T dataType = array[i];

            string path = Path.Combine(cacheDir, $"{dataType}.json");
            Directory.CreateDirectory(cacheDir);

            string json;
            if (keyValuePairs.TryGetValue(dataType, out json))
            {
                File.WriteAllText(path, json);
            }
        }
    }


    /// <summary>
    /// 텍스트에셋의 이름은 모두 type의 이름과 동일함! 
    /// ex) MapType의 Forest관련 텍스트에셋 : Forest.text 등등
    /// </summary>
    private void UseCacheOrFallback()
    {
        // 1. 로컬에 저장된 JSON파일이 있는지 
        string cacheDir = Path.Combine(Application.persistentDataPath, "ChartCache");

        // 1. 캐시 있으면 캐시 사용
        if (Directory.Exists(cacheDir)
             && Directory.GetFiles(cacheDir).Length > 0) 
        {
            LoadCache<DataType>(TypeByChartHandler);
            LoadCache<MapType>(TypeByChartHandler);

            return;
        }

        Debug.Log($"[WebChartService] : (!!) Resource 하위 Json에 접근");
        // 2. 캐시가 없으면 Resource 하위 json에 접근 (완전 최후의 방법)
        TextAsset[] textAssets = ResourceManager.Instance.FallBackChartTextfile;

        for (int i = 0; i < textAssets.Length; i++)
        {
            // 텍스트에셋이름을 enum으로 타입변경
            // -> DataType와 MapType에 해당하는 메서드 각각 실행

            string name = textAssets[i].name;

            if (Enum.TryParse(name, out DataType dataType)
                && Enum.IsDefined(typeof(DataType), dataType))
            {
                TypeByChartHandler(dataType, textAssets[i].text);
            }
            else if (Enum.TryParse(name, out MapType mapType)
                && Enum.IsDefined(typeof(MapType), mapType))
            {
                TypeByChartHandler(mapType, textAssets[i].text);
            }
            
        }
    }

    private void LoadCache<T>(Action<T, string> pars)
        where T : Enum
    {
        // 각 path에 맞는 json만 가져옴
        // ex) T가 MapType , path가 .../Forest.json 
        // 일 때 해당 json파일만 가져옴

        T[] array = (T[])Enum.GetValues(typeof(T));
        for (int i = 0; i < array.Length; i++)
        {
            string path = Path.Combine(cacheDir, $"{array[i]}.json");
            if (File.Exists(path))
            {
                pars?.Invoke(array[i], File.ReadAllText(path));
            }
        }

        Debug.Log($"[WebChartService] : 로컬 캐시 사용");
        return;
    }

    #region 버전 체크 메서드 

    private void CheckChartVersion(string version) 
    {
        if (!int.TryParse(version, out serverChartVersion))
        {
            serverChartVersion = -1;
            return;
        }

        // int로 잘 변경되면 다시 파싱할 필요 X 
        // try Parse에서 파싱한 값 들어감
        // serverChartVersion = int.Parse(version);
    }

    private void ChartVersionFailed() 
    {
        serverChartVersion = -1;
    }
    #endregion

    #region Data Type 차트 가져오기 + 파싱 메서드

    // DataType
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
    #endregion

    #region MapData 차트 가져오기 + 파싱 메서드

    // MapType
    private IEnumerator GetChart(MapType type)
    {
        if (type == MapType.None)
            yield break;

        // 차트 가져오기
        yield return WebRequestCore.CommonLogic<object>
            (
                "",
                chartUrl + type.ToString(),
                HttpRequestType.Get,
                (obj) => ParsingMap(obj, type),
                null
            );

        // 실패 Action을 null
        // -> 해당 맵만 데이터가 없는 상태가 되고, StageDataManager의 HasMapData()에서 잠긴 맵으로 처리됨
    }

    // 맵 파싱 메서드
    private void ParsingMap(object obj, MapType type) 
    {
        string json = JsonConvert.SerializeObject(obj);

        // json 임시 보관 
        tempChartMap.Add(type, json);

        StageChart.ParseAndStoreMapData(json, type);
    }

    private void TypeByChartHandler(MapType type, string jsonStr) 
    {
        StageChart.ParseAndStoreMapData(jsonStr, type);
    }

    #endregion
}

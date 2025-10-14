using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using LitJson;
using System;
using System.IO;
using static BackEnd.Quobject.SocketIoClientDotNet.Parser.Parser.Encoder;
using BackEnd.Content;
using static UnityEditor.Progress;
using static UnityEngine.Rendering.DebugUI;

public class BackendChartManager : Singleton<BackendChartManager>
{
    // 차트 목록 담겨져 있는 bro (서버에서 가져온)
    private BackEnd.Content.BackendContentTableReturnObject chartListServerBro;

    // 차트 ID 별 Contenct 
    // *주의 :차트 파일 ID 아님*
    private Dictionary<string, BackEnd.Content.ContentItem> chartIdByContenct;

    // name -> ICharHandler 리턴 펙토리
    private ChartHandlerFactory chartHandlerFactory;

    // key : 차트 ID - value : 차트에 해당하는 클래스 
    private Dictionary<string, ICharHandler> keyValuePairs;

    // 차트 .dat 파일 명
    const string chartFileName = "backend_cdn.dat";

    protected override void Singleton_Awake()
    {
        chartHandlerFactory = new ChartHandlerFactory();
        keyValuePairs = new Dictionary<string, ICharHandler>();
    }

    public void InitBackendChart() 
    {
        ChartTableCheck();
    }

    private void ChartTableCheck()
    {
        // 1. 서버에서 차트 불러오기
        chartListServerBro = Backend.CDN.Content.Table.Get();

        if (chartListServerBro.IsSuccess() == false)
        {
            Debug.LogError(chartListServerBro);
            return;
        }

        // 2. Application persistentData 경로에 파일 있는지 확인 
        string filePath = Path.Combine(Application.persistentDataPath, chartFileName);

        if (File.Exists(filePath))
        {
            // 파일 존재 -> 차트 업데이트
            BackEnd.Content.BackendContentReturnObject localCallback = null;
            localCallback = Backend.CDN.Content.Local.Update(chartListServerBro.GetContentTableItemList());
        }
        else
        {
            // 파일 x -> 로컬 저장

            // 불러온 차트 내용 조회
            BackEnd.Content.BackendContentReturnObject callback2 = Backend.CDN.Content.Get(chartListServerBro.GetContentTableItemList());

            if (Backend.CDN.Content.Local.Save(callback2.GetContentList(), out Exception e) == false)
            {
                Debug.LogError("Save Error : " + e);
                return;
            }

            Debug.Log("로컬 저장에 성공했습니다");
        }

        // 3. 로컬에 저장된거 가져오기 
        BackEnd.Content.BackendContentReturnObject temp = Backend.CDN.Content.Local.Load();

        // 성공 시 딕셔너리 형태로 변환
        // key : 차트 Id - value : content
        chartIdByContenct = temp.GetContentDictionarySortByChartId();

        // 4. 로컬에 저장된 차트의 내용 조회
        string str = "";
        foreach (string keyName in chartIdByContenct.Keys)
        {
            ContentItem item = chartIdByContenct[keyName];
            str += item.chartName + '\n';
            str += item.chartId;

            Debug.Log(str);
            str = "";

            // 차트 이름에 해당하는 클래스 생성후 return
            ICharHandler handler = chartHandlerFactory.ChartNameByChart(item.chartName);

            // ChartHandle실행 
            LitJson.JsonData jsonData = item.contentJson;
            handler.IParseAndStore(jsonData);
        }
    }
}

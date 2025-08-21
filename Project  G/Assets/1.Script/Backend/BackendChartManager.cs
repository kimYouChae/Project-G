using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using LitJson;
using System;


public class BackendChartManager : Singleton<BackendChartManager>
{
    // 차트 목록 담겨져 있는 bro
    private BackEnd.Content.BackendContentTableReturnObject chartListBro;

    // 차트 ID 별 Contenct 
    // *주의 :차트 파일 ID 아님*
    private Dictionary<string, BackEnd.Content.ContentItem> chartIdByContenct;

    // 차트 라우터
    private ChartRouter chartRouter;
    // name -> ICharHandler 리턴 펙토리
    private ChartHandlerFactory chartHandlerFactory;

    protected override void Singleton_Awake()
    {
        chartRouter = new ChartRouter();
        chartHandlerFactory = new ChartHandlerFactory();    
    }

    public void InitBackendChart() 
    {
        ChartTableCheck();
        ChartContentCheck();
    }

    private void ChartTableCheck()
    {
        // 로그인 후 차트 불러와야함 ! 
        chartListBro = Backend.CDN.Content.Table.Get();

        if (chartListBro.IsSuccess() == false)
        {
            Debug.LogError(chartListBro);
            return;
        }

        string str = "";
        foreach (BackEnd.Content.ContentTableItem item in chartListBro.GetContentTableItemList())
        {
            str += item.chartName + '\n';
            str += item;

            Debug.Log(str);
            str = "";

            // 차트 이름에 해당하는 클래스 생성후 return
            ICharHandler handler = chartHandlerFactory.ChartNameByChart(item.chartName);
            // 라우터에 register
            chartRouter.RegisterChartHanlder(item.chartId, handler);
        }
    }

    // 차트 내용 조회
    private void ChartContentCheck() 
    {
        BackEnd.Content.BackendContentReturnObject bro2;

        bro2 = Backend.CDN.Content.Get(chartListBro.GetContentTableItemList());

        if (!bro2.IsSuccess())
        {
            Debug.LogError("GetContent Fail : " + bro2);
            return;
        }

        // 성공 시 딕셔너리 형태로 변환
        // key : 차트 Id - value : content
        chartIdByContenct = bro2.GetContentDictionarySortByChartId();

        // 내용 확인
        foreach (string keyName in chartIdByContenct.Keys) 
        {
            Debug.Log( "키 이름 : " + keyName + " \n "+ chartIdByContenct[keyName].ToString());

            // ChartHandle실행 
            LitJson.JsonData jsonData = chartIdByContenct[keyName].contentJson;
            chartRouter.ChartHandle(keyName, jsonData);
        }
    }

}

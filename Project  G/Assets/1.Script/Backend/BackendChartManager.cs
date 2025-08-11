using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using LitJson;


public class BackendChartManager : Singleton<BackendChartManager>
{
    // 차트 목록 담겨져 있는 bro
    private BackEnd.Content.BackendContentTableReturnObject chartListBro;

    // 차트 ID 별 Contenct 
    // *주의 :차트 파일 ID 아님*
    private Dictionary<string, BackEnd.Content.ContentItem> chartIdByContenct;

    // 차트 아이디 *주의 :차트 파일 ID 아님*
    const string MAP_CHART_ID = "34128";

    protected override void Singleton_Awake()
    {

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

        foreach (BackEnd.Content.ContentTableItem item in chartListBro.GetContentTableItemList())
        {
            Debug.Log(item.chartName);
            Debug.Log(item);
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
        // 차트 Id별 content
        chartIdByContenct = bro2.GetContentDictionarySortByChartId();

        // 내용 확인
        
        foreach (string keyName in chartIdByContenct.Keys) 
        {
            Debug.Log( "키 이름 : " + keyName + " \n "+ chartIdByContenct[keyName].ToString());
        }

        // ** 테이블에서 값 가져오기 !
        GetJsonByCharId(MAP_CHART_ID);
    }

    private void GetJsonByCharId(string charID) 
    {
        if (chartIdByContenct.ContainsKey(charID)) 
        {
            Debug.Log("콘텐츠스트링:"+ chartIdByContenct[charID].contentString);

            LitJson.JsonData temp = chartIdByContenct[charID].contentJson;

            foreach (LitJson.JsonData row in temp) 
            {
                // row는 각 원소(오브젝트)
                string mapType = row["MapType"].ToString();
                string difficulty = row["Difficulty"].ToString();
                string contents = row["MapContents"].ToString();
                int rate = int.TryParse(row["Rate"].ToString(), out var r) ? r : 0;

                Debug.Log($"MapType={mapType}, Difficulty={difficulty}, Rate={rate}, Contents={contents}");
            }
        }
    }
    
}

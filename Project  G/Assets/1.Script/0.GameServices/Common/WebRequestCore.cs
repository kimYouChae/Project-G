using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class WebRequestCore
{
    private static int DefaultRequestTime = 30;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"> 응답 받고 역직렬화 할 클래스 타입</typeparam>
    /// <param name="requestJSon"> 요청 클래스를 json 문자열로</param>
    /// <param name="requestUrl"> 요청 주소 </param>
    /// <param name="reqeuestType"> http 요쳥 타입 </param>
    /// <param name="requestSuccessAction"> 성공시 <T> 전달</param>
    /// <param name="requestFailedAction"> 실패시 실행할 action </param>
    /// <returns></returns>
    public static IEnumerator CommonLogic<T>(
        string requestJSon,
        string requestUrl,
        HttpRequestType reqeuestType,
        Action<T> requestSuccessAction,
        Action requestFailedAction)
        where T : class
    {
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestJSon);

        // 요청 생성 
        var request = new UnityWebRequest(requestUrl, GetHttpType(reqeuestType));
        // body에 데이터를 추가 -> post만
        // get은 body 자체가 없기 때문에 해도 소용없음 (url에 붙여서 발송)
        if (reqeuestType == HttpRequestType.Post)
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.SetRequestHeader("Content-Type", "application/json");
        request.downloadHandler = new DownloadHandlerBuffer(); // 응답 바디 확인용
        request.timeout = DefaultRequestTime;
        request.useHttpContinue = false;

        // 요청보내기 (비동기)
        yield return request.SendWebRequest();

        // 조건을 (!= UnityWebRequest.Result.Success)로 할 시 BadRequest 응답은 모두 걸린다
        // > success =  false 응답을 처리하지 못함
        // 수정 : ConnectionError (서버까지 닿지못함/연결끊김/타임아웃)
        //      / DataProcessingError (DownloadHandler 처리 불가능)
        // > success = false 응답 처리 가능 
        if (request.result == UnityWebRequest.Result.ConnectionError
            || request.result == UnityWebRequest.Result.DataProcessingError)
        {
            Debug.Log($"{requestUrl} : APi 응답에 실패 했습니다.");

            requestFailedAction?.Invoke();
            yield break;
        }

        // 받은 요청을 string 타입으로
        string responseText = request.downloadHandler.text;

        // 제네릭 타입으로 역직렬화
        ApiResponse<T> apiResponse;
        try
        {
            apiResponse = JsonConvert.DeserializeObject<ApiResponse<T>>(responseText);
        }
        catch (Exception ex)
        {
            Debug.LogError($"{typeof(T).Name} : JSON 파싱 예외: {ex.Message}");

            // NetworkErrorAlert.AlertNetworkError(NetworkErrorType.Parse);
            requestFailedAction?.Invoke();
            yield break;
        }

        if (apiResponse == null)
        {
            Debug.Log($"{typeof(T).Name} : 파싱 중에 오류 발생 , Json으로 변환 불가 \n {responseText}");

            // NetworkErrorAlert.AlertNetworkError(NetworkErrorType.Parse);
            requestFailedAction?.Invoke();
            yield break;
        }

        if (apiResponse.success == false)
        {
            string reason = apiResponse.error != null ? $"[{apiResponse.error.code}] {apiResponse.error.message}"
                : $"[{request.responseCode}] 규격 밖 응답 \n {responseText}";

            Debug.LogError($"{typeof(T).Name} 요청 실패 : {requestUrl} \n {reason}");

            // NetworkErrorAlert.AlertNetworkError(NetworkErrorType.Api);
            requestFailedAction?.Invoke();
            yield break;
        }

        // 성공 시 
        requestSuccessAction?.Invoke(apiResponse.data);
    }

    private static string GetHttpType(HttpRequestType type) 
    {
        switch (type)
        {
            case HttpRequestType.Post: return "POST";
            case HttpRequestType.Get: return "GET";
        }

        return string.Empty;
    }
}

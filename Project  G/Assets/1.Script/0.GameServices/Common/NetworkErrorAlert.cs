using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NetworkErrorAlert 
{
    public static void AlertNetworkError(NetworkErrorType errorType) 
    {
        string errorMessage = ErrorTypeByErrorMessage(errorType);

        TextPopUp popup = UIManager.Instance.GetPopUP<TextPopUp>();
        popup.UpdateText(errorMessage);
    }

    private static string ErrorTypeByErrorMessage(NetworkErrorType errorType) 
    {
        // ##TODO 로컬라이징 추가 
        switch (errorType)
        {
            case NetworkErrorType.Connection: return "서버에 연결할 수 없습니다.";
            case NetworkErrorType.Server : return "서버 오류가 발생했습니다.";
            case NetworkErrorType.Parse: return "데이터를 불러오는 중 오류가 발생했습니다.";
            case NetworkErrorType.Api: return "요청을 처리하지 못했습니다.";
        }

        return "알수없는 오류 입니다";
    }
}

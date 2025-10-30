using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationChart : ICharHandler
{
    /// <summary>
    /// key : Server_Conneting
    /// English : Connecting to server
    /// Koran : 서버에 연결 중
    /// Japanese : サーバーに接続中
    /// Chinese : 连接到服务器
    /// </summary>

    public void IParseAndStore(LitJson.JsonData jsonData)
    {
        foreach (LitJson.JsonData row in jsonData)
        {
            string key = row["key"].ToString();

            // 언어만큼 for 돌기 
            for(int i = 0; i < Extension.EnumCount<LanguageType>(); i++) 
            {
                LanguageType currType = Extension.GetElement<LanguageType>(i);

                // lang 타입에 맞는 value 가져오기
                string value = row[currType.ToString()].ToString();

                LocalizationManager.Instance.AddLanguageDictionary(currType, key, value);
            }
        }
    }

    
}

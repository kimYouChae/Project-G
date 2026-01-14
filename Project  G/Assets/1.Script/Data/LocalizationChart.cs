using Newtonsoft.Json;
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
    /// 

    private class LocalizationClass
    {
        public string key { get; set; }
        public string english { get; set; }
        public string korean { get; set; }
        public string japanese { get; set; }
        public string chinese { get; set; }

        public string TypeByString(LanguageType type) 
        {
            switch(type) 
            {
                case LanguageType.English: return english;
                case LanguageType.Korean: return korean;
                case LanguageType.Japanese: return japanese;
                case LanguageType.Chinese: return chinese;
                default: return string.Empty;
            }
        }
    }


    public void IParseAndStore(string jsonStr)
    {
        ApiResponse<List<LocalizationClass>> obj = JsonConvert.DeserializeObject<ApiResponse<List<LocalizationClass>>>(jsonStr);

        if (obj == null)
        {
            Debug.LogError($"ApiResponse 파싱 실패 : {nameof(LocalizationChart)}");
            return;
        }

        List<LocalizationClass> datalist = obj.data;

        if (datalist == null || datalist.Count == 0)
        {
            Debug.LogWarning($"Data 리스트가 비었거나 null : {nameof(LocalizationChart)}");
            return;
        }

        for (int i = 0; i < datalist.Count; i++) 
        {
            LocalizationClass temp = datalist[i];
            string key = temp.key;

            // 언어만큼 for 돌기 
            for (int j = 0; j < Extension.EnumCount<LanguageType>(); j++)
            {
                LanguageType currType = Extension.GetElement<LanguageType>(j);

                LocalizationManager.Instance.AddLanguageDictionary(currType, key, temp.TypeByString(currType));
            }
        }

        /*
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
        */
    }
}

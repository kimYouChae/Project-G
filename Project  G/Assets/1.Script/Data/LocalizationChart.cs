using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LocalizationClass
{
    public string key;
    public string english;
    public string korean;
    public string japanese;
    public string chinese;

    public string TypeByString(LanguageType type)
    {
        switch (type)
        {
            case LanguageType.English: return english;
            case LanguageType.Korean: return korean;
            case LanguageType.Japanese: return japanese;
            case LanguageType.Chinese: return chinese;
            default: return string.Empty;
        }
    }
}

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

    public void IParseAndStore(string jsonStr)
    {
        List<LocalizationClass> datalist;
        datalist = JsonConvert.DeserializeObject<List<LocalizationClass>>(jsonStr);

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
    }
}

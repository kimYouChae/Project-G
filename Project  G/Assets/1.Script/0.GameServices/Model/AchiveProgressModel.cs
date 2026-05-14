using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchiveProgressModel : IAchiveProgressModel
{
    private List<AchiveProgressResponse> data;
    private Dictionary<AchiveType, AchiveProgressResponse> keyValuePairs;

    public AchiveProgressResponse GetAchiveProgress(AchiveType type)
    {
        if(keyValuePairs.ContainsKey(type))
            return keyValuePairs[type];

        return null;
    }

    public List<AchiveProgressResponse> GetBestScoreInfo()
    {
        return data;
    }

    public void SetGameData(List<AchiveProgressResponse> response)
    {
        this.data = response;

        keyValuePairs = new Dictionary<AchiveType, AchiveProgressResponse>();
        for(int i = 0;  i < data.Count; i++) 
        {
            keyValuePairs.Add(data[i].AchiveType, data[i]);
        }
    }
}

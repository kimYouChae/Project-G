using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchieveProgressModel : IAchieveProgressModel
{
    private List<AchieveProgressResponse> data;
    private Dictionary<AchieveType, AchieveProgressResponse> keyValuePairs;

    // API 실패 시 flag
    private bool isSuccess;

    public AchieveProgressResponse GetAchieveProgress(AchieveType type)
    {
        if(keyValuePairs.ContainsKey(type))
            return keyValuePairs[type];

        return null;
    }

    public List<AchieveProgressResponse> GetBestScoreInfo()
    {
        return data;
    }

    public void SetGameData(List<AchieveProgressResponse> response)
    {
        this.data = response;

        keyValuePairs = new Dictionary<AchieveType, AchieveProgressResponse>();
        for(int i = 0;  i < data.Count; i++)
        {
            keyValuePairs[data[i].AchieveType] = data[i];
        }
    }

    public void SetIsSuccess(bool flag)
    {
        isSuccess = flag;
    }

    public bool GetIsSuccess()
    {
        return isSuccess;
    }
}

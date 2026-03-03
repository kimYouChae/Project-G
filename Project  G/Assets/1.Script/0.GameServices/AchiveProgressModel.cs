using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchiveProgressModel : IAchiveProgressModel
{
    private List<AchiveProgressResponse> data;

    public List<AchiveProgressResponse> GetBestScoreInfo()
    {
        return data;
    }

    public void SetGameData(List<AchiveProgressResponse> response)
    {
        this.data = response;
    }
}

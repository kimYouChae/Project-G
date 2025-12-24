using System.Collections;
using System.Collections.Generic;

public class WebRankingService : IRankingService
{
    private string baseUrl;

    public WebRankingService(string url)
    {
        this.baseUrl = url;
    }

    public void GetMyRankingService(string myId, int mapType)
    {
        
    }

    public void GetRankerService(int mapType)
    {
        
    }
}

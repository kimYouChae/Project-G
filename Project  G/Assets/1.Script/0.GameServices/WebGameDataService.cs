using System.Collections;
using System.Collections.Generic;

public class WebGameDataService : IGameDataService
{
    private string baseUrl;

    public WebGameDataService(string url)
    {
        this.baseUrl = url;
    }

    public void UpdateGameDataService(string myId, string partnerId, float score, int mapType)
    {
        
    }
}

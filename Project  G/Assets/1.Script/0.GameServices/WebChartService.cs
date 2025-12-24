using System.Collections;
using System.Collections.Generic;

public class WebChartService : IChartService
{
    private string baseUrl;

    public WebChartService(string url)
    {
        this.baseUrl = url;
    }

    public void ChartService(DataType dataType)
    {
        
    }
}

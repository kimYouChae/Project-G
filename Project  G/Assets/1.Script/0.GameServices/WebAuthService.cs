using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebAuthService : IAuthService
{
    private string baseUrl;

    public WebAuthService(string url) 
    {
        this.baseUrl = url;
    }

    public void AuthService(string steamID, string nick, string country)
    {
        
    }
}

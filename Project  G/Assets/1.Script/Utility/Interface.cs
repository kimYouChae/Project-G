using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

#region GameServies
public interface IAuthService
{
    public void AuthService(string id, string nick, string country );
}

public interface IRankingService
{
    public void GetMyRankingService(string myId, int mapType);
    public void GetRankerService(int mapType);
}

public interface IGameDataService
{
    public void UpdateGameDataService(string myId, string partnerId, float score, int mapType);
}

public interface IChartService 
{
    public void ChartService(DataType dataType);           
}

#endregion


public interface ICharHandler
{
    public void IParseAndStore(LitJson.JsonData data);
}

public interface ILobbyPanelInitionlize
{
    public void IInitPanel();
}

public interface ILocalizable 
{
    public void IUpdateLocalization(LanguageType type);
}

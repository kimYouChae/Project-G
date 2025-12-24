using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

#region GameServies
public interface IAuthService
{
    public void AuthServies(string steamID, string nick, string country );
}

public interface IPlayerDataService
{

}

public interface IRankingService
{

}

public interface IGameDataService
{

}

public interface IChartService 
{
         
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

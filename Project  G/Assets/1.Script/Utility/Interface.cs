using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

#region GameServies
public interface IAuthService
{
    public IEnumerator AuthService(long id, string nick, string country );
}

public interface IRankingService
{
    public IEnumerator GetMyRankingService(long myId, int mapType);
    public IEnumerator GetRankerService(int mapType);
}

public interface IRankingModel 
{
    public void SetUserRanker(UserRankDTO dto);
    public void SetRankers(List<UserRankDTO> list);

    public UserRankDTO UserRanker();
    public List<UserRankDTO> RankersList();

    // 디버깅용 
    public void PrintUserRanker();
    public void PrintRankersList();
}

public interface IGameDataService
{
    public void UpdateGameDataService(string matchid, int mapType, long myId, long partnerId, float score, int stage);
}

public interface IChartService 
{
    public IEnumerator ChartService(DataType dataType);           
}

#endregion


public interface ICharHandler
{
    public void IParseAndStore(string jsonStr);
}

public interface ILobbyPanelInitionlize
{
    public void IInitPanel();
}

public interface ILocalizable 
{
    public void IUpdateLocalization(LanguageType type);
}

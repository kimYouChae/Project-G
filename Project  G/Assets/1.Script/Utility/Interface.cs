using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

#region GameServies
public interface IApiResult
{
    public void SetIsSuccess(bool flag);
    public bool GetIsSuccess();
}

public interface IAuthService
{
    public IEnumerator AuthService(long id, string nick, string country );
}

public interface IRankingService
{
    public IEnumerator GetMyRankingService(long myId, int mapType);
    public IEnumerator GetRankerService(int mapType);
}

public interface IRankingModel : IApiResult
{
    public void SetUserRanker(UserRankDTO dto);
    public void SetRankers(List<UserRankDTO> list);

    public UserRankDTO GetUserRanker();
    public List<UserRankDTO> GetRankersList();
}

public interface IGameDataModel : IApiResult
{
    public void SetGameData(BestScoreUpdateResponse response);

    public BestScoreUpdateResponse GetBestScoreInfo();
}

public interface IGameDataService
{
    public IEnumerator UpdateGameDataService(string matchid, int mapType, long myId, long partnerId, float score, int stage);
}

public interface IChartService
{
    public IEnumerator ChartService();
}

public interface IUserProgressService
{
    public IEnumerator GetAchievementService(long uid);
}

public interface IAchieveProgressModel : IApiResult
{
    public void SetGameData(List<AchieveProgressResponse> response);

    public List<AchieveProgressResponse> GetBestScoreInfo();

    public AchieveProgressResponse GetAchieveProgress(AchieveType type);
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

public interface IAchievement
{
    public bool IIsComplete();
    public string ITitle();
    public string IProgressText();
}

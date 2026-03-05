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

    public UserRankDTO GetUserRanker();
    public List<UserRankDTO> GetRankersList();

    // 디버깅용 
    public void PrintUserRanker();
    public void PrintRankersList();
}

public interface IGameDataModel 
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
    public IEnumerator ChartService(DataType dataType);           
}

public interface IUserProgressService 
{
    public IEnumerator GetAchivementService(long uid);
}

public interface IAchiveProgressModel 
{
    public void SetGameData(List<AchiveProgressResponse> response);

    public List<AchiveProgressResponse> GetBestScoreInfo();

    // 추후 수정 할 예정!! 어우 지저분해
    public AchiveProgressResponse GetAchiveProgress(AchiveType type);
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

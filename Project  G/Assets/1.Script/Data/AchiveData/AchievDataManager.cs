using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AchievDataManager : Singleton<AchievDataManager>
{
    [SerializeField] List<StageAchieve> achievements;
    [SerializeField] Dictionary<AchieveType, StageAchieve> achievementsDict;

    public List<StageAchieve> Achievements => achievements;

    protected override void Singleton_Awake()
    {
        achievements = new List<StageAchieve>();
        achievementsDict = new Dictionary<AchieveType, StageAchieve>();
    }

    public void AddtoAchieveContainer(StageAchieve achi)
    {
        achievements.Add(achi);
        if ( ! achievementsDict.ContainsKey(achi.AchieveType))
        {
            achievementsDict.Add(achi.AchieveType, achi);
        }
    }

    // 타입에 해당하는 도전과제 return
    public StageAchieve GetAchieveByType(AchieveType type)
    {
        if(achievementsDict.ContainsKey(type))
            return achievementsDict[type];

        return null;
    }
}

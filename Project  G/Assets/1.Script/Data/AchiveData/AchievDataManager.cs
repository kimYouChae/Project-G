using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AchievDataManager : Singleton<AchievDataManager>
{
    [SerializeField] List<StageAchive> achievements;
    [SerializeField] Dictionary<AchiveType, StageAchive> achievementsDict;

    public List<StageAchive> Achievements => achievements;

    protected override void Singleton_Awake()
    {
        achievements = new List<StageAchive>();
        achievementsDict = new Dictionary<AchiveType, StageAchive>();
    }

    public void AddtoAchiveContainer(StageAchive achi) 
    {
        achievements.Add(achi);
        if ( ! achievementsDict.ContainsKey(achi.AchiveType)) 
        {
            achievementsDict.Add(achi.AchiveType, achi);
        }
    }

    // 타입에 해당하는 도전과제 return
    public StageAchive GetAchiveByType(AchiveType type)
    {
        if(achievementsDict.ContainsKey(type))
            return achievementsDict[type];

        return null;
    }
}

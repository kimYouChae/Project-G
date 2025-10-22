using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AchievementsManager : Singleton<AchievementsManager>
{
    [SerializeField] List<Achievement> achievements;
    [SerializeField] Dictionary<AchiveType, Achievement> achievementsDict;

    public List<Achievement> Achievements => achievements;

    protected override void Singleton_Awake()
    {
        achievements = new List<Achievement>();
        achievementsDict = new Dictionary<AchiveType, Achievement>();
    }

    public void AddtoAchiveContainer(Achievement achi) 
    {
        achievements.Add(achi);
        if ( ! achievementsDict.ContainsKey(achi.AchiveType)) 
        {
            achievementsDict.Add(achi.AchiveType, achi);
        }
    }

    // 타입에 해당하는 도전과제 return
    public Achievement GetAchiveByType(AchiveType type)
    {
        if(achievementsDict.ContainsKey(type))
            return achievementsDict[type];

        return null;
    }
}

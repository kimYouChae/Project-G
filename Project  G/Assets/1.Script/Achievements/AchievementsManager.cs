using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsManager : Singleton<AchievementsManager>
{
    [SerializeField] List<Achievement> achievements;
    [SerializeField] Dictionary<AchiveType, Achievement> achievementsDict;

    public List<Achievement> Achievements => achievements;

    protected override void Singleton_Awake()
    {

    }

    private void Start()
    {
        achievementsDict = new Dictionary<AchiveType, Achievement>();
        for (int i = 0; i< achievements.Count; i++) 
        {
            Achievement ach = achievements[i];
            achievementsDict.Add(ach.AchiveType, ach);
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

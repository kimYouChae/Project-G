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
        achievements = new List<Achievement>();
        achievementsDict = new Dictionary<AchiveType, Achievement>();

        // Sriptable 오브젝트가 유니티 껏다 켤 때 마다 오류나서 일단 하드코딩으로 적음
        StageAchievement forestAchive = new StageAchievement("(임시)숲도전과제", AchiveType.Stage_Forest, 10, MapType.Forest);
        StageAchievement giganticAchive = new StageAchievement("(임시)거대숲 도전과제", AchiveType.Stage_GiganticTree, 10, MapType.GiganticTree);
        StageAchievement islandAchive = new StageAchievement("(임시)섬 도전과제", AchiveType.Stage_Island, 10, MapType.Island);

        achievements.Add(forestAchive);
        achievements.Add (giganticAchive);
        achievements.Add(islandAchive);

        // 딕셔너리에 저장 
        for (int i = 0; i < achievements.Count; i++)
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

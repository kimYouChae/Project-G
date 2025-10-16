using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAchievement
{
    public bool IIsComplete();
    public string ITitle();
    public string IProgressText();
}

[System.Serializable]
public abstract class Achievement : ScriptableObject, IAchievement
{
    [Header("===도전과제===")]
    [SerializeField] private string title;
    [SerializeField] AchiveType type;

    public AchiveType AchiveType => type;

    public abstract bool IIsComplete();

    public abstract string IProgressText();

    public string ITitle() => title;

}


[CreateAssetMenu(fileName = "Achievement", menuName = "Achievement/Stage Achievement")]
public class StageAchievement : Achievement
{
    [Header("===스테이지===")]
    [SerializeField] private int achiveStage;
    [SerializeField] private MapType mapType;

    public override bool IIsComplete()
    {
        int userStage = UserDataManager.Instance.UserData.UserStageByType(mapType);

        // 유저가 달성한 스테이지가 achive 스테이지보다 많으면 true, 아니면 false
        return userStage >= achiveStage;
    }

    public override string IProgressText()
    {
        return UserDataManager.Instance.UserData.UserStageByType(mapType) + " / " + achiveStage + "스테이지";
    }

}
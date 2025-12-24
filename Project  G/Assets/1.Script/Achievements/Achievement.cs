using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public interface IAchievement
{
    public bool IIsComplete();
    public string ITitle();
    public string IProgressText();
}

[System.Serializable]
public abstract class Achievement :  IAchievement
{
    [Header("===도전과제===")]
    [SerializeField] private string title;
    [SerializeField] AchiveType achiveType;

    public AchiveType AchiveType => achiveType;

    public Achievement(string title, AchiveType type) 
    {
        this.title = title;
        this.achiveType = type;
    }

    public abstract bool IIsComplete();

    public abstract string IProgressText();

    public string ITitle() => LocalizationManager.Instance.ReturnLocalizationString(achiveType.ToString() + "_Title");

}


[System.Serializable]
public class StageAchievement : Achievement
{
    [Header("===스테이지===")]
    [SerializeField] private int achiveStage;
    [SerializeField] private MapType mapType;

    public StageAchievement(string title, AchiveType achiveType,int achiveStage, MapType mapType) : base(title, achiveType)
    {
        this.achiveStage = achiveStage;
        this.mapType = mapType;
    }

    public override bool IIsComplete()
    {
        int userStage = UserData.instance.ReturnUserStage(mapType);

        // 유저가 달성한 스테이지가 achive 스테이지보다 많으면 true, 아니면 false
        return userStage >= achiveStage;
    }

    public override string IProgressText()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append( LocalizationManager.Instance.MapNameReturn(mapType) + " : ");
        builder.Append('\n');
        builder.Append(UserData.instance.ReturnUserStage(mapType) + " / " + achiveStage 
            + LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.Stage));
        return builder.ToString();
    }

}
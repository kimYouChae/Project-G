using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchivePopUP : UIPopUP
{
    [SerializeField] GameObject achiveObj;
    [SerializeField] Transform content;
    [SerializeField] List<AchiveObject> objList;

    [Header("===Localization===")]
    [SerializeField] TextMeshProUGUI achiveTitle;

    private void InstantiateAchiveObject() 
    {
        // 처음 1회 생성
        for (int i = 0; i < AchievementsManager.Instance.Achievements.Count; i++)
        {
            GameObject temp = Instantiate(achiveObj);
            temp.transform.SetParent(content.transform, false);

            AchiveObject achi = temp.GetComponent<AchiveObject>();
            objList.Add(achi);
            achi.OnOffCompleteImage(false);
        }
    }

    // On 될 때 마다 업데이트 
    public void InitAchivePopup() 
    {
        // 타이틀 로컬라이징
        achiveTitle.text = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.Achievement);

        // 리스트에 없으면 -> 1회 오픈, 새로 생성
        if (objList.Count <= 0) 
        {
            InstantiateAchiveObject();
        }

        // 도전과제 내용 + 진행상황 오브젝트에 표시 + 로컬라이징 
        for (int i = 0; i < AchievementsManager.Instance.Achievements.Count; i++) 
        {
            Achievement achi = AchievementsManager.Instance.Achievements[i];
            AchiveObject obj = objList[i];

            // 도전과제 타이틀 텍스트
            obj.SetAchiveTitle(achi.ITitle());
            // 도전과제 완료 여부 체그
            if (achi.IIsComplete()) 
            {
                // 완료 아이콘 켜기 
                obj.OnOffCompleteImage(true);
                return;
            }

            obj.SetProgressText(achi.IProgressText());
        }
    }
}

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
        for (int i = 0; i < AchievDataManager.Instance.Achievements.Count; i++)
        {
            GameObject temp = Instantiate(achiveObj);
            temp.transform.SetParent(content.transform, false);

            AchiveObject achi = temp.GetComponent<AchiveObject>();
            objList.Add(achi);
            achi.OnOffCompleteImage(false);
        }
    }

    // On 될 때 마다 업데이트 
    public void InitAchivePopup(bool hasOpend) 
    {
        // 타이틀 로컬라이징
        achiveTitle.text = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.Achievement);

        // 도전과제 정보 가져오기 + UI에 표시 
        StartCoroutine(GetInfo(hasOpend));
    }

    private IEnumerator GetInfo(bool hasOpend ) 
    {
        List<AchiveProgressResponse> achivelist;

        // 한번도 UI를 안켰을 때만 
        if (!hasOpend) 
        {
            // API 호출 필요 
            yield return StartCoroutine(
                GameServices.Instance.UserProgressService.GetAchivementService(UserDataManager.Instance.SteamID));
        }

        // GamdService의 도전과제모델에서 가져오기 
        achivelist = GameServices.Instance.AchiveProgressModel.GetBestScoreInfo();

        // UI 업데이트 
        UpdateAchivePopup(achivelist);
    }

    private void UpdateAchivePopup(List<AchiveProgressResponse> achives) 
    {
        // UI에 표시 
        /*
           // 리스트에 없으면 -> 1회 오픈, 새로 생성
           if (objList.Count <= 0) 
           {
               InstantiateAchiveObject();
           }

           // 도전과제 내용 + 진행상황 오브젝트에 표시 + 로컬라이징 업데이트 
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
                   continue;
               }

               obj.SetProgressText(achi.IProgressText());
           }
           */
    }
}

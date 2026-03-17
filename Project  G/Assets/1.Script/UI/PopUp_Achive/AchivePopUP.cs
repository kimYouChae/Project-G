using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchivePopUP : UIPopUP
{
    [SerializeField] GameObject achiveObj;
    [SerializeField] Transform content;
    [SerializeField] List<AchiveObject> achiveObjList;

    [Header("===Localization===")]
    [SerializeField] TextMeshProUGUI achiveTitle;

    // On 될 때 마다 업데이트 
    public void OpenAchivePopup(bool hasOpend) 
    {
        // UI ON, 팝업 사운드 실행
        base.OpenPopUP();

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
        // 리스트에 없으면 -> 1회 오픈, 새로 생성
        if (achiveObjList.Count <= 0)
        {
            InstantiateAchiveObject(achives.Count);
        }

        // UI에 표시 
        // 도전과제 내용 + 진행상황 오브젝트에 표시 + 로컬라이징 업데이트 
        for (int i = 0; i < achives.Count; i++) 
        {
            AchiveObject achObj = achiveObjList[i];

            // 진행상황 데이터
            AchiveProgressResponse progressData = achives[i];

            // 타입에 해당하는 도전과제 데이터 
            StageAchive achive = AchievDataManager.Instance.GetAchiveByType(progressData.AchiveType);

            // 도전과제 타이틀 텍스트
            achObj.SetAchiveTitle(achive.Title);
            // 도전과제 완료 여부 체그
            // 성공했으면 
            if (progressData.isClear) 
            {
                // 완료 아이콘 켜기 
                achObj.OnOffCompleteImage(true);
                continue;
            }

            // 아직 미성공이면 
            string progressText = UserDataManager.Instance.ReturnUserStage(achive.MapType)
                + "/" + achive.AchiveStage;
            achObj.SetProgressText(progressText);
        }
    }

    private void InstantiateAchiveObject(int length)
    {
        // 처음 1회 생성
        for (int i = 0; i < length; i++)
        {
            GameObject temp = Instantiate(achiveObj);
            temp.transform.SetParent(content.transform, false);

            AchiveObject achi = temp.GetComponent<AchiveObject>();
            achiveObjList.Add(achi);
            achi.OnOffCompleteImage(false);
        }
    }

}

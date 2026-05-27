using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchivePopUP : UIPopUP
{
    [Header("===AchivePopUP===")]
    [SerializeField] GameObject achiveObj;
    [SerializeField] Transform content;
    [SerializeField] List<AchiveObject> achiveObjList;
    [SerializeField] GameObject uiContents; // 팝업 내부 ui ( 캐릭터 선택 + 디테일창 )
    [SerializeField] TextMeshProUGUI loadingText;   // 로딩중 텍스트

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
            loadingText.text = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.LoadingData);
            uiContents.SetActive(false);

            // API 호출 필요 
            yield return StartCoroutine(
                GameServices.Instance.UserProgressService.GetAchivementService(UserDataManager.Instance.SteamID));

            // API 호출이 실패하면 
            if (!GameServices.Instance.AchiveProgressModel.GetIsSuccess()) 
            {
                loadingText.text = "오프라인입니다";

                yield break;
            }

            loadingText.text = string.Empty;
            uiContents.SetActive(true);
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
            string title = LocalizationAchiveTitle(progressData.AchiveType);
            achObj.SetAchiveTitle(title);
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

    private string LocalizationAchiveTitle(AchiveType type)
    {
        string key = type.ToString() + "_Title";
        return LocalizationManager.Instance.ReturnLocalizationString(key);
    }

}

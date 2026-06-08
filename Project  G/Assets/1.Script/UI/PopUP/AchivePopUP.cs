using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievePopUP : UIPopUP
{
    [Header("===AchievePopUP===")]
    [SerializeField] GameObject achiveObj;
    [SerializeField] Transform content;
    [SerializeField] List<AchieveObject> achieveObjList;
    [SerializeField] GameObject uiContents; // 팝업 내부 ui ( 캐릭터 선택 + 디테일창 )
    [SerializeField] TextMeshProUGUI loadingText;   // 로딩중 텍스트

    [Header("===Localization===")]
    [SerializeField] TextMeshProUGUI achiveTitle;

    // On 될 때 마다 업데이트
    public void OpenAchievePopup(bool hasOpend)
    {
        // UI ON, 팝업 사운드 실행
        base.OpenPopUP();

        // 타이틀 로컬라이징
        achiveTitle.text = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.Achievement);

        // 도전과제 정보 가져오기 + UI에 표시
        StartCoroutine(GetInfo(hasOpend));
    }

    private IEnumerator GetInfo(bool hasOpend)
    {
        List<AchieveProgressResponse> achievelist;

        // 한번도 UI를 안켰을 때만
        if (!hasOpend)
        {
            loadingText.text = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.LoadingData);
            uiContents.SetActive(false);

            // API 호출 필요
            yield return StartCoroutine(
                GameServices.Instance.UserProgressService.GetAchievementService(UserDataManager.Instance.SteamID));

            // API 호출이 실패하면
            if (!GameServices.Instance.AchieveProgressModel.GetIsSuccess())
            {
                loadingText.text
                    = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.Achievement_Offline);
                yield break;
            }

            loadingText.text = string.Empty;
            uiContents.SetActive(true);
        }

        // GameService의 도전과제모델에서 가져오기
        achievelist = GameServices.Instance.AchieveProgressModel.GetBestScoreInfo();

        // UI 업데이트
        UpdateAchievePopup(achievelist);
    }

    private void UpdateAchievePopup(List<AchieveProgressResponse> achieves)
    {
        // 리스트에 없으면 -> 1회 오픈, 새로 생성
        if (achieveObjList.Count <= 0)
        {
            InstantiateAchieveObject(achieves.Count);
        }

        for (int i = 0; i < achieves.Count; i++)
        {
            AchieveObject achObj = achieveObjList[i];

            // 진행상황 데이터
            AchieveProgressResponse progressData = achieves[i];

            // 타입에 해당하는 도전과제 데이터
            StageAchieve achieve = AchievDataManager.Instance.GetAchieveByType(progressData.AchieveType);

            // 도전과제 타이틀 텍스트
            string title = LocalizationAchieveTitle(progressData.AchieveType);
            achObj.SetAchieveTitle(title);

            // 도전과제 완료 여부 체크
            if (progressData.isClear)
            {
                // 완료 아이콘 켜기
                achObj.OnOffCompleteImage(true);
                continue;
            }

            if (achieve == null) continue;

            // 아직 미성공이면
            string progressText = UserDataManager.Instance.ReturnUserStage(achieve.MapType)
                + "/" + achieve.AchieveStage;
            achObj.SetProgressText(progressText);
        }
    }

    private void InstantiateAchieveObject(int length)
    {
        // 처음 1회 생성
        for (int i = 0; i < length; i++)
        {
            GameObject temp = Instantiate(achiveObj);
            temp.transform.SetParent(content.transform, false);

            AchieveObject achi = temp.GetComponent<AchieveObject>();
            achieveObjList.Add(achi);
            achi.OnOffCompleteImage(false);
        }
    }

    private string LocalizationAchieveTitle(AchieveType type)
    {
        string key = type.ToString() + "_Title";
        return LocalizationManager.Instance.ReturnLocalizationString(key);
    }

}

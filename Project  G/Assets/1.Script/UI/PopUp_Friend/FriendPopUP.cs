using Photon.Pun;
using Photon.Realtime;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class FriendPopUP : UIPopUP
{
    [Header("FriendPopUP")]
    [SerializeField] private GameObject scrollView;
    [SerializeField] private Transform content;  // 친구 목록 스크롤뷰 content에 해당하는 부분 
    [SerializeField] private FriendObject friendObj;
    [SerializeField] private List<FriendObject> friendObjList;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private TextMeshProUGUI popupTitleText;    // 타이틀 텍스트 

    const int firstInstanceCount = 20;
    private bool isOpened = false; // 연적이있는지 여부 

    public void OpenFriendPopUP() 
    {
        // 팝업 타이틀 로컬라이징
        popupTitleText.text = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.FriendPopup_Title);

        // 리스트가 비어있으면 생성 
        if (friendObjList.Count <= 0)
        {
            for (int i = 0; i < firstInstanceCount; i++)
            {
                InstanceFriendObject();
            }
        }

        StartCoroutine(LoadFriendsAndOpenUI());
    }

    IEnumerator LoadFriendsAndOpenUI() 
    {
        SteamScript.Instance.GetSteamFriend();

        // 연적이 없으면 
        if (!isOpened)
        {
            scrollView.SetActive(false);
            loadingText.gameObject.SetActive(true);
            loadingText.text = LocalizationManager.Instance.ReturnLocalizationString(LocalizationKey.FriendPopup_Loading);

            isOpened = true;
            // 임시로 1초 기다리기 
            yield return new WaitForSeconds(1f);
        }

        // 텍스트 끄기 
        loadingText.gameObject.SetActive(false);
        scrollView.SetActive(true);

        // UI 생성
        MakeUi();
    }

    private void MakeUi() 
    {
        List<CSteamID> friendsInfo = SteamScript.Instance.FriendCSteamIDs;

        // 1. 부족한 만큼 오브젝트 생성
        while(friendObjList.Count < friendsInfo.Count) 
        {
            // 생성 후, friendObjList에 넣음 
            // "친구수"까지(조건 만족할 때 까지) 생성 
            InstanceFriendObject();
        }

        // 데이터 넣기 
        for (int i = 0; i < friendsInfo.Count; i++) 
        {
            SettingFriendObj(i);
        }

        // 혹시 오브젝트가 친구수 보다 많다면 끄기 
        for (int i = friendsInfo.Count; i < friendObjList.Count; i++)
        {
            friendObjList[i].gameObject.SetActive(false);
        }

        void SettingFriendObj(int index) 
        {
            CSteamID friendData = friendsInfo[index];

            // 프로필 이미지, 이름 필요함
            Texture2D texture = SteamScript.Instance.ReturnProfileTexureByCSteamID(friendData);
            string name = SteamScript.Instance.ReturnNickNameByCSteamID(friendData);

            FriendObject obj = friendObjList[index];
            // 꺼져있으면 켜기
            if(obj.gameObject.activeSelf == false)
                obj.gameObject.SetActive(true);
            // 데이터 업데이트
            obj.UpdateFriendObj(texture, name, index);
        }
    }

    private void InstanceFriendObject() 
    {
        FriendObject obj = Instantiate(friendObj);
        obj.SetParentPopup(this);
        obj.gameObject.transform.SetParent(content, false);
        friendObjList.Add(obj);
    }

    public void InviteFriendByIndex(int index) 
    {
        // 친구 초대 실행 
        SteamScript.Instance.InviteFriendByIndex(index);

        // 팝업 끄기 
        OffPanel();
    }
}

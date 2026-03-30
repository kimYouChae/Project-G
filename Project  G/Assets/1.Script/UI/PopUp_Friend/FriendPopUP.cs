using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FriendPopUP : UIPopUP
{
    [SerializeField]
    private Transform content;  // 친구 목록 스크롤뷰 content에 해당하는 부분 
    [SerializeField]
    private FriendObject friendObj;
    [SerializeField] 
    private List<FriendObject> friendObjList;
    [SerializeField]
    private TextMeshProUGUI loadingText;

    const int firstInstanceCount = 20;

    public void OpenFriendPopUP() 
    {
        loadingText.text = "(로컬라이징 전 입니다) 친구 목록 불러오는중";

        // 리스트가 비어있으면 생성 
        if (friendObjList.Count <= 0)
        {
            for (int i = 0; i < firstInstanceCount; i++)
            {
                InstanceFriendObject();
            }
        }

        StartCoroutine(temp());
    }

    IEnumerator temp() 
    {
        SteamScript.Instance.GetSteamFriend();

        // 임시로 1초 기다리기 
        yield return new WaitForSeconds(1f);

        // 텍스트 끄기 
        loadingText.gameObject.SetActive(false);

        // UI 생성
        MakeUi();
    }

    private void MakeUi() 
    {
        var friendsInfo = SteamScript.Instance.FriendCSteamIDs;

        // 데이터 넣기 
        for (int i = 0; i < friendsInfo.Count; i++) 
        {
            FriendObject obj = friendObjList[i];
        }
    }

    private void InstanceFriendObject() 
    {
        FriendObject obj = Instantiate(friendObj);
        obj.gameObject.transform.SetParent(content, false);
        friendObjList.Add(obj);
    }
}

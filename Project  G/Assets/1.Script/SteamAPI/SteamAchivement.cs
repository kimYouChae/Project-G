using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class SteamAchivement : MonoBehaviour
{
    // 도전과제 저장 콜백
    private Callback<UserAchievementStored_t> userAchiveCallBack;


    private void Start()
    {
        userAchiveCallBack = Callback<UserAchievementStored_t>.Create(SuccessAchive);
    }

    // 도전과제가 성공적으로 저장될 때마다 호출되는 콜백
    private void SuccessAchive(UserAchievementStored_t callback) 
    {
        Debug.Log($"[SteamAchivement] - 도전과제를 성공적으로 저장했습니다 {callback.m_rgchAchievementName} ");
    }

}

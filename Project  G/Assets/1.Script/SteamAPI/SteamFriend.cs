using Steamworks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SteamFriend : MonoBehaviour
{
    private Callback<GameRichPresenceJoinRequested_t> temp;

    private void Start()
    {
        temp = Callback<GameRichPresenceJoinRequested_t>.Create(FriendIsJoinedRoom);
    }

    // 초대된 플레이어가 game에 참가할 때 실행되는 콜백
    private void FriendIsJoinedRoom(GameRichPresenceJoinRequested_t callback) 
    {
        Debug.Log($"[SteamFriend] {callback.m_steamIDFriend} - 친구가 게임에 접속");
    }

    // 친구 창 오버레이 열기 
    public void OpenFriendOveray()
    {
        SteamFriends.ActivateGameOverlay("Friends");
    }



}

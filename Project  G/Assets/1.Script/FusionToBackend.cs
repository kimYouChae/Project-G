using BackEnd;
using Fusion;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionToBackend : MonoBehaviour
{
    private static FusionToBackend instance;

    // 플레이어 ref에 대한 뒤끝의 object 정보 
    private Dictionary<PlayerRef, string> playerRefToBackendObj;
    public static FusionToBackend GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("FusionManager 인스턴스가 존재하지 않습니다.");
            return null;
        }
        return instance;
    }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        // 초기화
        playerRefToBackendObj = new Dictionary<PlayerRef, string>();
    }

    // playerRef에 따른 뒤끝 ReturObject정보의 닉네임 return
    public string PlayerRefToNickName(PlayerRef pr) 
    {
        string nick = null;
        if( playerRefToBackendObj.TryGetValue(pr, out nick)) 
        {
            if (nick.Equals(string.Empty))
            {
                Debug.Log("닉네임 리턴중 : " + "문자열이 empty");
                return "문자열이 empty";
            }

            return nick;
        }

        Debug.Log("닉네임 리턴중 : " + "알수없는 유저정보 입니다" );
        return "알수없는 유저정보 입니다";
    }

    // 플레이어 정보 클라이언트 모두에게 전달 
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    // 해당 메서드는 
    public void RPC_SendUserInfo(string nick, PlayerRef pr,RpcInfo info = default)
    {
        if (nick == null)
        {
            Debug.Log("RPC_SendUserInfo" + "에서 닉네임 설정중 오류발생");
            return;
        }

        Debug.Log("RPC_SendUserInfo" + " : 닉네임 세팅 메서드 호출 ");
        AddUserInfoToDict(pr, nick);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    private void AddUserInfoToDict(PlayerRef pr, string nick)
    {
        if (!playerRefToBackendObj.ContainsKey(pr))
        {
            playerRefToBackendObj.Add(pr, nick);
        }

        Debug.Log("RPC_닉네임 딕셔너리에 넣기 :" + nick);

    }
}

using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public partial class LobbyUIManager : MonoBehaviour
{
    [Space]
    [Header("===WaitingUi===")]
    [SerializeField] TextMeshProUGUI roomTitle;
    [SerializeField] GameObject playeRefObject;
    [SerializeField] GameObject scrollViewContent;

    [SerializeField] List<GameObject> playerRefObj;

    public void UpdateWaitingRoomInfo()
    {
        // 현재 세션에 대한 정보를 가져옴
        SessionInfo info = FusionLobbyManager.GetInstance().currSession();

        // 방제 업데이트
        roomTitle.text = info.Name;

        // 플레이어 정보 입력 
        List<PlayerRef> playerref = FusionLobbyManager.GetInstance().JoinPlayersRefInfo;

        // 리스트 초기화
        DestoryListObject(playerRefObj);

        for (int i = 0; i < playerref.Count; i++)
        {
            GameObject temp = Instantiate(playeRefObject);
            TextMeshProUGUI text = temp.GetComponentInChildren<TextMeshProUGUI>();

            // ref에 따른 닉네임으로 설정 
            Debug.Log("Ui : 플레이어 re " + playerref[i]);
            text.text = FusionToBackend.GetInstance().PlayerRefToNickName(playerref[i]);

            playerRefObj.Add(temp);

            temp.transform.SetParent(scrollViewContent.transform);
        }
    }


}

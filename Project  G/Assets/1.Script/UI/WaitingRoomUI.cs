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

    public void UpdateWaitingRoomInfo()
    {
        // 현재 세션에 대한 정보를 가져옴
        SessionInfo info = FusionManager.GetInstance().currSession();

        // 방제 업데이트
        roomTitle.text = info.Name;

        // 플레이어 정보 입력 
        List<PlayerRef> playerref = FusionManager.GetInstance().JoinPlayersRefInfo;
        for (int i = 0; i < playerref.Count; i++)
        {
            GameObject temp = Instantiate(playeRefObject);
            TextMeshProUGUI text = temp.GetComponentInChildren<TextMeshProUGUI>();
            text.text = playerref[i].PlayerId.ToString();

            temp.transform.SetParent(scrollViewContent.transform);
        }
    }


}

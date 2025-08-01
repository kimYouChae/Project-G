using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class LobbyUIManager : MonoBehaviour
{
    [Space]
    [Header("===WaitingUi===")]
    [SerializeField] TextMeshProUGUI roomTitle;
    [SerializeField] GameObject playeRefObject;
    [SerializeField] GameObject scrollViewContent;

    [SerializeField] List<GameObject> playerRefObj;

    [SerializeField] Button gameStartButton;

    private void InitWaitinRoomUI() 
    {
        gameStartButton.onClick.AddListener(() => 
        {
            // 마스터클라이언트만 가능 
            if (PhotonNetwork.IsMasterClient)
                PhotonSceneManager.Instance.ChangeScene(SceneType.Game);
        });
    }

    public void UpdateWaitingRoomInfo(Player[] playerref)
    {
        // 현재 방 대한 정보를 가져옴
        Room info = PhotonNetwork.CurrentRoom;

        // 방제 업데이트
        roomTitle.text = info.Name;

        // 리스트 초기화
        DestoryListObject(playerRefObj);

        for (int i = 0; i < playerref.Length; i++)
        {
            GameObject temp = Instantiate(playeRefObject);
            TextMeshProUGUI text = temp.GetComponentInChildren<TextMeshProUGUI>();
            text.text = playerref[i].NickName;

            playerRefObj.Add(temp);

            temp.transform.SetParent(scrollViewContent.transform);
        }
    }


}

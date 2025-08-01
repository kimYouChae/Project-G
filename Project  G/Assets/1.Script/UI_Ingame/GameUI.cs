using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using System.Text;

public partial class InGameUI : MonoBehaviour
{
    [Space]
    [Header("===GameUI===")]
    // 사분면을 지켜서 순서대로 담아둬야함 !
    [SerializeField] TextMeshProUGUI[] playerInfoText;
    [SerializeField] PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }

    public void UpdatePlayerInfoText() 
    {
        view.RPC("RPC_UpdatePlayerInfoText", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void RPC_UpdatePlayerInfoText() 
    {
        // 로컬 플레이어의 사분면 위치에 해당하는 TMP
        TextMeshProUGUI tmp = playerInfoText[(int)PunIngameManager.Instance.LocalQuadrantType];

        StringBuilder sb = new StringBuilder();

        sb.Append($"유저 닉네임 : {UserDataManager.Instance.UserData.NickName} \n");

        // 현재 타입에 해당하는 맵 타입
        // 현재 방 정보의 커스텀 정보에 접근 (hashTable에서 matType검사)
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("MapType", out object type))
        {
            // 현재 방의 맵 타입(MapType)의 string 
            string roomTypeString = (string)type;

            // 현재 맵 타입에 대한 씬 타입
            MapType mapType = Extension.StringToEnum<MapType>(roomTypeString);

            sb.Append($"현재 맵 최고점수: {UserDataManager.Instance.UserData.MapTypeToScore[mapType]} \n");
        }

        tmp.text = sb.ToString();
    }
}

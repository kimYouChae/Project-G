using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using System;
using UnityEngine;


public class PhotonSceneManager : Singleton<PhotonSceneManager>
{
    protected override void Singleton_Awake()
    {
        SetUpDontDestroy();
    }

    // 임시 : 씬 동기 전환 

    public void ChangeGameScene() 
    {
        // 현재 방 정보의 커스텀 정보에 접근 (hashTable에서 matType검사)
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("MapType", out object type))
        {
            // 현재 방의 맵 타입(MapType)의 string 
            string roomTypeString = (string)type;

            // 현재 맵 타입에 대한 씬 타입
            SceneType sceneType = MapNameToSceneType(MapTypeByStr(roomTypeString));

            // 씬 변경 ( 씬 타입 따라 )
            ChangeScene(sceneType);
        }
        else 
        {
            // 없으면 기본맵으로 이동
            ChangeScene(SceneType.Game_Forest);
        }
    }

    private MapType MapTypeByStr(string type) 
    {
        return (MapType)Enum.Parse(typeof(MapType), type);
    }

    private SceneType MapNameToSceneType(MapType type) 
    {
        switch (type) 
        {
            case MapType.Forest:return SceneType.Game_Forest;
            case MapType.GiganticTree:return SceneType.Game_GiganticTree;
            case MapType.Market: return SceneType.Game_Market;
            case MapType.Island: return SceneType.Game_Island;
            case MapType.Hell: return SceneType.Game_Hell;
            case MapType.IceVillage: return SceneType.Game_IceVillage;
        }

        // 아무것도 리턴안되면 기본값
        return SceneType.Game_Forest;
    }

    public void ChangeScene(SceneType type) 
    {
        string nextSceneString = Define.SceneNames[type];

        if (nextSceneString == null)
            return;

        PhotonNetwork.LoadLevel(nextSceneString);
    }

}

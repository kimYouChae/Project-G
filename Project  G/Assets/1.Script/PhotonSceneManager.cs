using Photon.Pun;
using UnityEngine;

public enum SceneType 
{
    Lobby, Game
}

public class PhotonSceneManager : Singleton<PhotonSceneManager>
{
    protected override void Singleton_Awake()
    {
        
    }

    // 임시 : 씬 동기 전환 
    public void ChangeScene(SceneType type) 
    {
        string next = TypeBySceneName(type);
        if (next == null)
            return;

        PhotonNetwork.LoadLevel(next);
    }


    private string TypeBySceneName(SceneType type) 
    {
        switch (type) 
        {
            case SceneType.Lobby:
                return "01.LobbyScene";
            case SceneType.Game:
                return "02.GameScene";
        }

        return string.Empty;
    }
}

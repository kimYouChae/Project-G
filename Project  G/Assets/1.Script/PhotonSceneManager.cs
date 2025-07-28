using Photon.Pun;
using UnityEngine;

public enum SceneType 
{
    Lobby, Game
}

public class PhotonSceneManager : MonoBehaviour
{
    private static PhotonSceneManager instance;   // 인스턴스

    void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static PhotonSceneManager GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("PhotonSceneManager 인스턴스가 존재하지 않습니다.");
            return null;
        }

        return instance;
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

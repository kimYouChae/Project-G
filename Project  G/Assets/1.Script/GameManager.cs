using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("GameManager 인스턴스가 존재하지 않습니다.");
            return null;
        }
        return instance;
    }
    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void ChangeSceneToMatchLobby() 
    {
        SceneManager.LoadScene("1_MatchScene");
    }
}

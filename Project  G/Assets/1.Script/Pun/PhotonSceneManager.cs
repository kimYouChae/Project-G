using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PhotonSceneManager : Singleton<PhotonSceneManager>
{
    [Header("===Scene INFO===")]
    [SerializeField] private string nowSceneName;
    [SerializeField] private SceneType nowSceneType;

    [SerializeField]
    private Dictionary<SceneType, Action> sceneTypeByAction;

    protected override void Singleton_Awake()
    {
        SetUpDontDestroy();

        // 딕셔너리 초기화 
        sceneTypeByAction = new Dictionary<SceneType, Action>();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void RegisterAction(SceneType sceneType, Action action) 
    {
        if (sceneTypeByAction.ContainsKey(sceneType))
        {
            sceneTypeByAction[sceneType] += action;
        }
        else
        {
            sceneTypeByAction[sceneType] = action;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"씬 로드 완료: {scene.name}");

        if (!sceneTypeByAction.ContainsKey(nowSceneType))
        {
            Debug.Log($"[PhotonSceneManager] 씬 타입에 해당하는 액션이 없습니다");
            return;
        }

        sceneTypeByAction[nowSceneType]?.Invoke();
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
            SceneType sceneType = MapNameToSceneType(Extension.StringToEnum<MapType>(roomTypeString));

            // 씬 변경 ( 씬 타입 따라 )
            ChangeScene(sceneType);
        }
        else 
        {
            // 없으면 기본맵으로 이동
            ChangeScene(SceneType.Game_Forest);
        }
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
        nowSceneType = type;    
        nowSceneName = Define.sceneNames[type];

        if (nowSceneName == null)
            return;

        // 만약 Time.scaleTime이 0이라면 다시 1로 돌리기
        if (Time.timeScale == 0)
            TimeManager.Play();

        PhotonNetwork.LoadLevel(nowSceneName);
    }

}

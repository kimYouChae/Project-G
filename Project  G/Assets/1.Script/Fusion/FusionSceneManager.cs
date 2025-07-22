using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Collections.Unicode;


public enum SceneState 
{
    Lobby, Game
}

public class FusionSceneManager : MonoBehaviour, INetworkSceneManager
{
    /// <summary>
    /// INetworkSceneManager구현체 : 씬 관리자
    /// 러너.StartGame 필드를 통해 할당되어야함!
    /// </summary>

    private static FusionSceneManager instance;

    [SerializeField]
    private NetworkRunner runner;   // FusionLobbyManager의 Runner을 가져와야함 
    [SerializeField]
    private bool isLoading = false;

    // ICorutine (NetworkSceneManagerDefault 참고)
    private List<ICoroutine> _runningCoroutines = new List<ICoroutine>();
    public bool LogSceneLoadErrors = true;

    public static FusionSceneManager GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("FusionSceneManager 인스턴스가 존재하지 않습니다.");
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
    }

    public void ChangeScene(NetworkRunner nrunner, SceneState state)
    {
        this.runner = nrunner;

        Debug.Log("FusionSceneManager : 씬 전환 메서드 실행 ");
        var scene = SceneRef.FromIndex((int)state);

        if (nrunner == null)
        {
            Debug.Log("runner가 Null입니다");
            return;
        }    
                
        // 씬 전환은 서버/호스트에서만 가능
        if (nrunner.IsSceneAuthority)
        {
            Debug.Log("씬 전환은 호스트만 가능 , LoadScene 콜백을 실행합니다");
            // LoadScene 콜백을 호출 , 직접적으로 씬 전환 x (콜백 내부에서 씬 전환을 구현해야함)
            // NetworkRunner의 LoadScene을 호출 
            nrunner.LoadScene(scene, LoadSceneMode.Single);
        }
    }

    public bool IsBusy { get => isLoading;  }
    public Scene MainRunnerScene { get => SceneManager.GetActiveScene();}


public void Initialize(NetworkRunner runner)
    {
        
    }

    public void Shutdown()
    {
       
    }

    public bool IsRunnerScene(Scene scene)
    {
        return true;
    }

    public bool TryGetPhysicsScene2D(out PhysicsScene2D scene2D)
    {
        var mainScene = MainRunnerScene;
        if (mainScene.IsValid())
        {
            scene2D = mainScene.GetPhysicsScene2D();
            return true;
        }
        else
        {
            scene2D = default;
            return false;
        }
    }

    public bool TryGetPhysicsScene3D(out PhysicsScene scene3D)
    {
        var mainScene = MainRunnerScene;
        if (mainScene.IsValid())
        {
            scene3D = mainScene.GetPhysicsScene();
            return true;
        }
        else
        {
            scene3D = default;
            return false;
        }
    }

    public void MakeDontDestroyOnLoad(GameObject obj)
    {
        DontDestroyOnLoad(obj);
    }

    public bool MoveGameObjectToScene(GameObject gameObject, SceneRef sceneRef)
    {
        Scene scene = SceneManager.GetSceneByBuildIndex(sceneRef.AsIndex);
        if (scene.IsValid())
        {
            SceneManager.MoveGameObjectToScene(gameObject, scene);
            return true;
        }
        return false;
    }

    // 씬이 전환될 때 (NetworkSceneManagerDefault 참고)
    public NetworkSceneAsyncOp LoadScene(SceneRef sceneRef, NetworkLoadSceneParameters parameters)
    {
        Debug.Log($"[SceneManager] LoadScene 요청됨: {sceneRef}");
        return NetworkSceneAsyncOp.FromCoroutine(sceneRef, StartTracedCoroutine(LoadSceneCoroutine(sceneRef, parameters)));
    }

    // 씬이 언로드 될 때 (NetworkSceneManagerDefault 참고)
    public NetworkSceneAsyncOp UnloadScene(SceneRef sceneRef)
    {
        Debug.Log($"[SceneManager] UnloadScene 요청됨: {sceneRef}");
        return NetworkSceneAsyncOp.FromCoroutine(sceneRef, StartTracedCoroutine(UnloadSceneCoroutine(sceneRef)));
    }

   public SceneRef GetSceneRef(GameObject gameObject)
    {
        return SceneRef.FromIndex(gameObject.scene.buildIndex);
    }

    public SceneRef GetSceneRef(string sceneNameOrPath)
    {
        int buildIndex = FusionUnitySceneManagerUtils.GetSceneBuildIndex(sceneNameOrPath);
        if (buildIndex >= 0)
        {
            return SceneRef.FromIndex(buildIndex);
        }
        return SceneRef.None;
    }

    public bool OnSceneInfoChanged(NetworkSceneInfo sceneInfo, NetworkSceneInfoChangeSource changeSource)
    {
        return false;
    }

    IEnumerator LoadSceneCoroutine(SceneRef sceneRef, NetworkLoadSceneParameters sceneParams) 
    {
        yield return null;

        // 씬을 여기서 직접 로드 (동기 또는 Unity 비동기)
        SceneManager.LoadScene(sceneRef.AsIndex, LoadSceneMode.Single);

        // Fusion에게 "씬 로딩 완료!" 라고 알림
        runner.InvokeSceneLoadStart(sceneRef);
    }

    IEnumerator UnloadSceneCoroutine(SceneRef sceneRef) 
    {
        yield return null;
    }

    #region NetworkSceneManagerDefault 의 ICorutine을 참고
    private ICoroutine StartTracedCoroutine(IEnumerator inner)
    {
        var coro = new FusionCoroutine(inner);

        _runningCoroutines.Add(coro);

        coro.Completed += x => {

            if (LogSceneLoadErrors && x.Error != null)
            {
                Log.Error(runner, $"Failed async op: {x.Error.SourceException}");
            }

            // remove this one from the list
            var index = _runningCoroutines.IndexOf((ICoroutine)x);
            Debug.Assert(index == 0, "Expected the completed coroutine to be the first in the list");
            _runningCoroutines.RemoveAt(index);

            // start the next one
            if (index < _runningCoroutines.Count)
            {
                Log.TraceSceneManager(runner, $"Starting enqueued coroutine {index} of {_runningCoroutines.Count}");
                StartCoroutine(_runningCoroutines[index]);
            }
        };

        if (_runningCoroutines.Count == 1)
        {
            // start immediately
            StartCoroutine(coro);
        }
        else
        {
            Log.TraceSceneManager(runner, $"Enqueued coroutine, there are already {_runningCoroutines.Count - 1} running");
        }

        return coro;
    }
    #endregion
}

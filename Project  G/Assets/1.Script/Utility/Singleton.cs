using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // 싱글톤
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // 씬에서 오브젝트 찾기
                instance = (T)FindObjectOfType(typeof(T));

                // 씬에 없다면 
                if (instance == null)
                {
                    // 새 오브젝트 생성
                    GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                    instance = obj.AddComponent<T>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {

        // null이 아니고, 생성된 인스턴스랑 현재 인스턴스랑 다르면
        if (instance != null && instance != (T)(MonoBehaviour)this)
        {
            // 중복 인스턴스 제거
            Destroy(gameObject);
            return;
        }

        // 제네릭 타입의 인스턴스로 현재 객체 설정
        instance = (T)(MonoBehaviour)this;

        // 추상 메서드 호출
        Singleton_Awake();
    }

    // 각 Manager가 Awake가 필요한 경우 실행 
    protected abstract void Singleton_Awake();

    // 각 Manager가 DoneDestroy가 필요한 경우 실행 
    protected void SetUpDontDestroy()
    {
        // Managers
        //  ㄴ UiManager
        //  ㄴ SkillManager 처럼 manager가 하위에 있을수도 있다.

        // 내가 하위에 있는 오브젝트라면
        if (transform.parent != null && transform.root != null)
        {
            // 상위 부모를 dontDestroy
            DontDestroyOnLoad(this.transform.root.gameObject);
        }
        // 내가 상위 오브젝트면 
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
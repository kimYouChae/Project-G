using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Battlehub.Dispatcher
{
    public class Dispatcher : MonoBehaviour
    {
        private static Dispatcher m_instance;

        public static Dispatcher Current
        {
            get
            {
                if (m_instance == null)
                {
                    var go = new GameObject("Dispatcher");
                    m_instance = go.AddComponent<Dispatcher>();

                    DontDestroyOnLoad(go);
                }
                return m_instance;
            }
        }

        private readonly Queue<Action> m_executingActions = new Queue<Action>();
        private ConcurrentQueue<Action> m_actionQueue = new ConcurrentQueue<Action>();
        public void BeginInvoke(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            m_actionQueue.Enqueue(action);
        }

        /// <summary>
        /// Limiting execution time to a percentage of the target frame time (50% by default via m_frameBudget).
        /// This helps maintain a smooth framerate even when many actions are queued.
        /// </summary>
        [SerializeField]
        private float m_frameBudget = 0.5f;
        private float m_currentTime;
        private float m_allotedTimePerFrame;
        private float m_targetFramerate;

        private void ReadCurrentTime()
        {
            m_currentTime = Time.realtimeSinceStartup;
        }

        private void InitAllotedTimePerFrame()
        {
            if (Application.targetFrameRate != -1)
            {
                m_targetFramerate = Application.targetFrameRate;
            }
            else
            {
                m_targetFramerate = 30;
            }

            m_allotedTimePerFrame = 1 / m_targetFramerate;
            m_allotedTimePerFrame *= m_frameBudget;
            ReadCurrentTime();
        }

        private void Awake()
        {
            if (m_instance != null && m_instance != this)
            {
                Destroy(gameObject);
                return;
            }

            m_instance = this;
            DontDestroyOnLoad(gameObject);
            InitAllotedTimePerFrame();
        }

        private void Update()
        {
            if (!m_actionQueue.IsEmpty)
            {
                while (m_actionQueue.TryDequeue(out Action action))
                {
                    m_executingActions.Enqueue(action);
                }
            }

            ReadCurrentTime();

            while (m_executingActions.Count > 0)
            {
                float elapsedTime = Time.realtimeSinceStartup - m_currentTime;
                if (elapsedTime > m_allotedTimePerFrame)
                {
                    break;
                }

                var action = m_executingActions.Dequeue();
                try
                {
                    action.Invoke();
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Exception in dispatched action: {ex}");
                }
            }
        }
    }
}
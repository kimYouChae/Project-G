using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IslandMap : MonoBehaviour, IMapPattern
{
    /// <summary>
    /// 파도 마스크 1개의 캐싱 정보
    /// 씬에 배치된 위치/크기 = 최대 침수 상태 ( 비율 t = 1 )
    /// </summary>
    private class WaveMask
    {
        public GameObject obj;
        public Transform trs;
        public Vector3 basePos;     // Awake 시점의 localPosition
        public Vector3 baseScale;   // Awake 시점의 localScale
        public Vector2 spriteSize;  // 스프라이트 원본 크기 ( 스케일 미적용 )

        public WaveMask(GameObject obj)
        {
            if (obj == null)
            {
                Debug.LogError("[IslandMap] 파도 오브젝트가 인스펙터에 연결되지 않았습니다");
                return;
            }

            this.obj = obj;
            this.trs = obj.transform;
            this.basePos = trs.localPosition;
            this.baseScale = trs.localScale;

            // 콜라이더가 아닌 SpriteMask의 스프라이트 크기로 판정한다
            SpriteMask mask = obj.GetComponent<SpriteMask>();
            if (mask == null || mask.sprite == null)
            {
                Debug.LogError($"[IslandMap] {obj.name} 에 SpriteMask(또는 sprite)가 없습니다");
                return;
            }

            this.spriteSize = mask.sprite.bounds.size;
        }

        public void SetActive(bool active)
        {
            if (obj == null) return;

            obj.SetActive(active);
        }
    }

    [SerializeField]
    private MapType mapType = MapType.Island;

    [Header("===Sprite Mask Object===")]
    [Header("===Left===")]
    [SerializeField] GameObject leftFieldLeftWave;
    [SerializeField] GameObject leftFieldTopWave;
    [SerializeField] GameObject leftFieldRightWave;
    [SerializeField] GameObject leftFieldBottomWave;

    [Header("===Right===")]
    [SerializeField] GameObject rightFieldLeftWave;
    [SerializeField] GameObject rightFieldTopWave;
    [SerializeField] GameObject rightFieldRightWave;
    [SerializeField] GameObject rightFieldBottomWave;

    [Header("===Wave Timing===")]
    [SerializeField] private float minInterval = 5f;    // 웨이브 최소 간격
    [SerializeField] private float maxInterval = 10f;    // 웨이브 최대 간격
    [SerializeField] private float riseTime = 2f;        // 차오르는 시간
    [SerializeField] private float holdTime = 10f;       // 최대 침수 유지 시간
    [SerializeField] private float fallTime = 2.5f;      // 빠지는 시간
    [SerializeField] private AnimationCurve riseCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [SerializeField] private AnimationCurve fallCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Header("===Player===")]
    [SerializeField] private float slowMultiplier = 0.75f;   // 물에 잠겼을 때 이동속도 배율

    // DirType 순서 ( Left, Top, Right, Bottom ) 로 담는다
    private WaveMask[] leftWaves;
    private WaveMask[] rightWaves;

    private WaveMask activeWave;        // 내 필드에서 진행 중인 파도 ( 감속 판정 대상 )
    private Coroutine waveCorutine;

    public float SlowMultiplier { get => slowMultiplier; }

    private void Awake()
    {
        // 씬에 배치된 값 = 최대 침수 상태이므로 여기서 캐싱해둔다
        leftWaves = new WaveMask[]
        {
            new WaveMask(leftFieldLeftWave),
            new WaveMask(leftFieldTopWave),
            new WaveMask(leftFieldRightWave),
            new WaveMask(leftFieldBottomWave)
        };

        rightWaves = new WaveMask[]
        {
            new WaveMask(rightFieldLeftWave),
            new WaveMask(rightFieldTopWave),
            new WaveMask(rightFieldRightWave),
            new WaveMask(rightFieldBottomWave)
        };
    }

    private void Start()
    {
        // 에디터에 남아있던 최대 크기가 첫 프레임에 그려지면
        // 물이 번쩍하고 지나가므로 t = 0 으로 초기화한다
        ResetAllWaves();
    }

    public MapType IGetMapType()
    {
        return mapType;
    }

    public void IMapPatternEnter()
    {
        // 마스터만 타이머를 돌리고 방향을 뽑는다
        // 비마스터도 돌리면 양쪽이 각자 웨이브를 띄워 두 번 발생한다
        if (!PhotonNetwork.IsMasterClient)
            return;

        Debug.Log($"[IslandMap] 섬 맵의 MapPatternEnter() 실행 ");
        StartCoroutine(WaveTimerCorutine());
    }

    /// <summary>
    /// 마스터 전용 - 웨이브 간격마다 방향을 뽑아 RaiseEvent로 알린다
    /// </summary>
    private IEnumerator WaveTimerCorutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

            DirType dir = (DirType)Random.Range(0, Extension.EnumCount<DirType>());

            // 보내는 쪽도 RaiseEvent로 되받아서 실행한다 ( 로컬 직접 호출 x )
            WaveRaiseEvent(dir, PhotonNetwork.Time);

            // 진행 중인 웨이브가 끝나기 전에는 다음 방향을 뽑지 않는다
            yield return new WaitForSeconds(riseTime + holdTime + fallTime);
        }
    }

    private void WaveRaiseEvent(DirType dir, double startTime)
    {
        Debug.Log("[IslandWaveStart] 조수(파도) 시작 Raise Event");

        // 파도 방향 , 웨이브 시작 시각 ( 네트워크 시간 )
        object[] contcnt = new object[]
        {
            (byte)dir,
            startTime
        };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        // 놓치면 그 웨이브가 통째로 사라지므로 신뢰 전송
        SendOptions sendOption = new SendOptions { Reliability = true };

        bool success = PhotonNetwork.RaiseEvent((byte)PunEventType.IslandWaveStart,
            contcnt,
            raiseEventOptions,
            sendOption);

        Debug.Log($"[IslandWaveStart] RaiseEvent 보냄? {success}");
    }

    /// <summary>
    /// 웨이브 시작 - OnEvent 핸들러에서만 호출된다
    /// </summary>
    public void StartWave(DirType dir, double startTime)
    {
        // 이전 웨이브가 남아있으면 정리하고 새로 시작
        if (waveCorutine != null)
        {
            StopCoroutine(waveCorutine);
            ResetAllWaves();
        }

        waveCorutine = StartCoroutine(WaveCorutine(dir, startTime));
    }

    private IEnumerator WaveCorutine(DirType dir, double startTime)
    {
        // 한 번 뽑은 방향을 두 필드에 미러링한다
        // 왼쪽 Left <-> 오른쪽 Right , Top/Bottom 은 양쪽 동일
        DirType leftDir = dir;
        DirType rightDir = MirrorDir(dir);

        WaveMask leftWave = leftWaves[(int)leftDir];
        WaveMask rightWave = rightWaves[(int)rightDir];

        // 연출은 두 필드 모두 재생하고,
        // 감속 판정만 내 사분면의 필드 기준으로 한다
        // 1사분면 -> 오른쪽 필드 , 2사분면 -> 왼쪽 필드
        activeWave = PunIngameManager.Instance.LocalQuadrantType == QuadrantType.one
            ? rightWave : leftWave;

        leftWave.SetActive(true);
        rightWave.SetActive(true);

        // 1. 차오름
        // 진행도는 전달받은 시작 시각 기준 ( Time.deltaTime 누적 x )
        while (true)
        {
            float elapsed = (float)(PhotonNetwork.Time - startTime);
            if (elapsed >= riseTime)
                break;

            float t = riseCurve.Evaluate(elapsed / riseTime);
            ApplyWave(leftWave, leftDir, t);
            ApplyWave(rightWave, rightDir, t);

            yield return null;
        }

        ApplyWave(leftWave, leftDir, 1f);
        ApplyWave(rightWave, rightDir, 1f);

        // 2. 유지
        while ((float)(PhotonNetwork.Time - startTime) < riseTime + holdTime)
            yield return null;

        // 3. 빠짐
        while (true)
        {
            float elapsed = (float)(PhotonNetwork.Time - startTime) - (riseTime + holdTime);
            if (elapsed >= fallTime)
                break;

            float t = 1f - fallCurve.Evaluate(elapsed / fallTime);
            ApplyWave(leftWave, leftDir, t);
            ApplyWave(rightWave, rightDir, t);

            yield return null;
        }

        // 4. 종료
        ApplyWave(leftWave, leftDir, 0f);
        ApplyWave(rightWave, rightDir, 0f);

        activeWave = null;

        leftWave.SetActive(false);
        rightWave.SetActive(false);

        waveCorutine = null;
    }

    /// <summary>
    /// 비율 t ( 0 ~ 1 ) 에 맞춰 마스크의 scale과 position을 함께 계산한다
    /// pivot이 Center라 scale만 바꾸면 양쪽으로 커지므로 반드시 같이 계산해야 한다
    /// </summary>
    private void ApplyWave(WaveMask wave, DirType dir, float t)
    {
        if (wave == null || wave.trs == null)
            return;

        Vector3 scale = wave.baseScale;
        Vector3 pos = wave.basePos;

        float halfW = wave.baseScale.x * wave.spriteSize.x * 0.5f;
        float halfH = wave.baseScale.y * wave.spriteSize.y * 0.5f;

        // 좌/우는 X만 , 상/하는 Y만 변한다
        // 고정될 변(edge)은 제자리에 두고 중심만 절반씩 이동시킨다
        switch (dir)
        {
            case DirType.Left:
                scale.x = wave.baseScale.x * t;
                pos.x = (wave.basePos.x - halfW) + halfW * t;
                break;

            case DirType.Right:
                scale.x = wave.baseScale.x * t;
                pos.x = (wave.basePos.x + halfW) - halfW * t;
                break;

            case DirType.Bottom:
                scale.y = wave.baseScale.y * t;
                pos.y = (wave.basePos.y - halfH) + halfH * t;
                break;

            case DirType.Top:
                scale.y = wave.baseScale.y * t;
                pos.y = (wave.basePos.y + halfH) - halfH * t;
                break;
        }

        wave.trs.localScale = scale;
        wave.trs.localPosition = pos;
    }

    /// <summary>
    /// 모든 파도를 t = 0 상태로 되돌리고 끈다
    /// </summary>
    private void ResetAllWaves()
    {
        for (int i = 0; i < leftWaves.Length; i++)
        {
            ApplyWave(leftWaves[i], (DirType)i, 0f);
            ApplyWave(rightWaves[i], (DirType)i, 0f);

            leftWaves[i].SetActive(false);
            rightWaves[i].SetActive(false);
        }

        activeWave = null;
    }

    private DirType MirrorDir(DirType dir)
    {
        if (dir == DirType.Left)
            return DirType.Right;

        if (dir == DirType.Right)
            return DirType.Left;

        // Top / Bottom 은 그대로
        return dir;
    }

    /// <summary>
    /// 물에 잠겨있는지 판정 ( 콜라이더 없이 사각형 비교 4번 )
    /// 별도 Rect를 들고있지 않고 마스크의 현재 Transform에서 값을 가져오므로
    /// 보이는 물과 판정이 어긋나지 않는다
    /// </summary>
    public bool IsInWater(Vector2 worldPos)
    {
        if (activeWave == null || activeWave.trs == null)
            return false;

        Vector2 c = activeWave.trs.position;
        Vector3 lossyScale = activeWave.trs.lossyScale;

        Vector2 half = new Vector2(
            activeWave.spriteSize.x * Mathf.Abs(lossyScale.x),
            activeWave.spriteSize.y * Mathf.Abs(lossyScale.y)) * 0.5f;

        return Mathf.Abs(worldPos.x - c.x) <= half.x
            && Mathf.Abs(worldPos.y - c.y) <= half.y;
    }
}

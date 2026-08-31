using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MarketMapPattern : MonoBehaviour, IMapPattern
{
    const float coolTime = 15f;
    const float offset = 3f;    // 너무 외곽에 생성되지 않게

    [SerializeField]
    private MapType mapType = MapType.Market;
    [SerializeField]
    private MerchantPatternType merchantType;

    [SerializeField]
    private Transform merchantTrs; // 주민 오브젝트 담아둘 trs

    // ##TODO : Resource 리펙토링 전 Merchant 오브젝트 담아두기 
    [SerializeField]
    private List<GameObject> merchantPrefab;    // enum 순서대로 담겨있음 

    public MapType IGetMapType()
    {
        return mapType;
    }

    public void IMapPatternEnter()
    {
        StartCoroutine(PatterLogin());
    }

    public IEnumerator PatterLogin() 
    {
        while (true) 
        {
            yield return new WaitForSeconds(coolTime);

            // 내 사분면 가져오기 
            QuadrantType quType = PunIngameManager.Instance.LocalQuadrantType;

            // 사분면에 따라 생성할 방향 달라짐
            // 1사분면이면 오른쪽에 생성 ( 이동 : 오른쪽 > 왼쪽 )
            // 2사분변이면 왼쪽에 생성 ( 이동 : 왼쪽 > 오른쪽 )
            DirType dirtype = quType == QuadrantType.one ? DirType.Right : DirType.Left;

            merchantType = GetRandomMerchantType();

            // meeting 제외 주민 생성 
            if (merchantType != MerchantPatternType.Meeting) 
            {
                MerchantRaiseEvent(PunEventType.MerchantSpawn, merchantType, dirtype, GenerationPosi(dirtype), StopRandX());
            }
            // meeting 주민만 , 호스트일때만
            if (merchantType == MerchantPatternType.Meeting && PhotonNetwork.IsMasterClient)
            {
                // A [ ------targetA--x--tergetB-] B 
                // 

                // 여기서는 DirType이 뭐든지 상관없음
                Vector2 genePosiA = GenerationPosi(DirType.Left);
                Vector2 genePosiB = new Vector2(Define.mapMaxX, genePosiA.y);
                float X = StopRandX();

                // 속도 
                var data = MerchantManager.Instance.GetData(merchantType);
                float speed = data.Speed;

                float targetA = X - data.Sight;
                float targetB = X + data.Sight;

                float timeA = Math.Abs(targetA - genePosiA.x) / speed;
                float timeB = Math.Abs(targetB - genePosiB.x) / speed;
                float maxTime = Math.Max(timeA, timeB);

                MerchantRaiseEvent(PunEventType.MerchantMeetingSpawn, merchantType, DirType.Left, genePosiA, X, maxTime - timeA);
                MerchantRaiseEvent(PunEventType.MerchantMeetingSpawn, merchantType, DirType.Right, genePosiB, X, maxTime - timeB);
            }

        }
    }

    private MerchantPatternType GetRandomMerchantType() 
    {
        int ran = Random.Range(1, Extension.EnumCount<MerchantPatternType>());

        return (MerchantPatternType)ran;
    }

    private void MerchantRaiseEvent(PunEventType eventType, MerchantPatternType mType, 
        DirType dirtype, Vector2 randGenePosi , float stopRandX, float waitTime = 0)
    {
        Debug.Log("[MerchantSpawn] 마켓 주민 생성 Raise Event");

        // 주민 타입 , 주민 방향(DirType) , 
        // 생성할 랜덤 x/y 위치 , 멈출 랜덤 x 위치
        object[] contcnt = new object[]
        {
            mType,
            dirtype,
            randGenePosi.x,
            randGenePosi.y,
            stopRandX,
            waitTime
        };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOption = new SendOptions { Reliability = true };

        bool success = PhotonNetwork.RaiseEvent((byte)eventType,
            contcnt,
            raiseEventOptions,
            sendOption);

        Debug.Log($"[MerchantSpawn] RaiseEvent 보냄? {success}");
    }

    private Vector2 GenerationPosi( DirType dirtype ) 
    {
        // 생성 위치 구하기 
        // 오른쪽 : x는 max
        // 왼쪽 : x는 min
        float x = dirtype == DirType.Right? Define.mapMaxX : Define.mapMinX;
        float y = Random.Range(Define.mapMinY + offset, Define.mapMaxY - offset);

        return new Vector2(x,y);
    }

    private float StopRandX() 
    {
        // 사분면 타입에 상관없이 왼쪽 ~ 오른쪽 필드 내에서 랜덤 위치 
        float fieldMinX = Define.twoMemberFieldMin[QuadrantType.two].x;
        float fieldMaxX = Define.twoMemberFieldMax[QuadrantType.one].x;

        return Random.Range(fieldMinX, fieldMaxX);
    }

    /// <summary>
    /// 주민 타입 
    /// / 주민이 어디에 위치하는지 
    ///     : 오른쪽에 위치하면 왼쪽으로 가야함, 왼쪽에 있으면 오른쪽으로 가야함 
    /// / 생성 위치 
    ///     : 위치값에 따라 위치 랜덤 생성 후 동기화됨 
    /// </summary>
    public void GenerateMerchant(MerchantPatternType merchantType, DirType dirtype, Vector2 randPosi, float stopX, float waitTime = 0) 
    {
        // GameObject prefab = ResourceManager.Instance.MerchantObj(merchantType);
        // ##TODO : 임시 프리팹 가져오기 
        if ((int)merchantType >= merchantPrefab.Count
           || merchantPrefab[(int)merchantType] == null)
        {
            Debug.Log($"Type이 프리팹 배열을 넘습니다 , 타입 :{merchantType}");
            return;
        }
        GameObject prefab = merchantPrefab[(int)merchantType];

        if (prefab != null)
        {
            GameObject merch = Instantiate(prefab);

            // 위치지정
            merch.transform.position = randPosi;
            // 부모지정 
            merch.transform.SetParent(merchantTrs);

            merch.GetComponent<Merchant>().SetupMerchant(merchantType, dirtype, stopX, waitTime);
        }
    }

}

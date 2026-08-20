using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class MarketMapPattern : MonoBehaviour, IMapPattern
{
    const float coolTime = 10f;

    [SerializeField]
    private MapType mapType = MapType.Market;

    public MapType IGetMapType()
    {
        return mapType;
    }

    public void IMapPatternEnter()
    {
        PatterLogin();
    }

    public IEnumerator PatterLogin() 
    {
        yield return new WaitForSeconds(coolTime);

        // 내 사분면 가져오기 
        QuadrantType quType = PunIngameManager.Instance.LocalQuadrantType;

        // 사분면에 따라 생성할 방향 달라짐
        // 1사분면이면 오른쪽에 생성 ( 이동 : 오른쪽 > 왼쪽 )
        // 2사분변이면 왼쪽에 생성 ( 이동 : 왼쪽 > 오른쪽 )
        DirType dirtype = quType == QuadrantType.one ? DirType.Right : DirType.Left;

        // 마켓 주민 생성 이벤트 송신
        MerchantRaiseEvent( GetRandomMerchantType(), dirtype , GenerationPosi(dirtype) );
    }

    private MerchantPatternType GetRandomMerchantType() 
    {
        int ran = Random.Range(1, Extension.EnumCount<MerchantPatternType>());

        return (MerchantPatternType)ran;
    }

    private void MerchantRaiseEvent(MerchantPatternType mType, DirType dirtype, Vector2 randPosi)
    {
        Debug.Log("[MerchantSpawn] 마켓 주민 생성 Raise Event");

        // 주민 타입 , 주민 방향(DirType)
        object[] contcnt = new object[]
        {
            mType,
            dirtype,
            randPosi.x,
            randPosi.y,
        };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOption = new SendOptions { Reliability = true };

        bool success = PhotonNetwork.RaiseEvent((byte)PunEventType.MerchantSpawn,
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
        float y = y = Random.Range(Define.mapMinY, Define.mapMaxY);

        return new Vector2(x,y);
    }

    /// <summary>
    /// 주민 타입 
    /// / 주민이 어디에 위치하는지 
    ///     : 오른쪽에 위치하면 왼쪽으로 가야함, 왼쪽에 있으면 오른쪽으로 가야함 
    /// / 생성 위치 
    ///     : 위치값에 따라 위치 랜덤 생성 후 동기화됨 
    /// </summary>
    public void GenerateMerchant(MerchantPatternType merchantType, DirType dirtype, Vector2 posi) 
    {
        // GameObject merchat = Instantiate();
    }

}

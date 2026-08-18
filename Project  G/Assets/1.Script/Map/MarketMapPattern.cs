using Photon.Pun;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class MarketMapPattern : MonoBehaviour, IMapPattern
{
    const float coolTime = 10f;
    float randomTime;

    const string marketPeople = "MarketPeoplePrefab";

    [SerializeField]
    private MapType mapType = MapType.Market;

    public MapType IGetMapType()
    {
        return mapType;
    }

    public void IMapPatternEnter()
    {
        
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

        // 마켓 주민 생성
        // 애니메이션 동기화 때문에 Photon View 있어야함
        PhotonNetwork.Instantiate(marketPeople , GenerationPosi(dirtype), Quaternion.identity);
        
        // 마켓 주민 : DirType만 알면 직진으로만 뛰면됨 .
    }

    private Vector2 GenerationPosi(DirType dirtype ) 
    {
        // 생성 위치 구하기 
        // 오른쪽 : x는 max
        // 왼쪽 : x는 min
        float x = dirtype == DirType.Right? Define.mapMaxX : Define.mapMinX;
        float y = y = Random.Range(Define.mapMinY, Define.mapMaxY);

        return new Vector2(x,y);
    }

}

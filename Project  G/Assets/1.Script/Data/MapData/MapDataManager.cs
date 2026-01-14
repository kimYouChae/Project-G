using ExitGames.Client.Photon;
using Newtonsoft.Json;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class MapData 
{
    [JsonProperty("type")]
    private MapType maptype;
    [JsonProperty("difficulty")] 
    private Difficulty difficulty;
    [JsonProperty("contents")] 
    private string content;
    [JsonProperty("rate")] 
    private int rate;

    public MapType Maptype { get => maptype; }
    public Difficulty Difficulty { get => difficulty;  }
    public string Content { get => content; }
    public int Rate { get => rate; }

    public MapData(MapType maptype, Difficulty difficulty, string content, int rate)
    {
        this.maptype = maptype;
        this.difficulty = difficulty;
        this.content = content;
        this.rate = rate;
    }

}

public class MapDataManager : Singleton<MapDataManager>
{
    private Dictionary<MapType, MapData> typeByMapData;
    [SerializeField] private MapData nowMapData;

    // 인스펙터 창에서 보기용
    [SerializeField] private List<MapData> mapDataList;

    public int MapRate { get => nowMapData.Rate; }

    protected override void Singleton_Awake()
    {
        SetUpDontDestroy();

        typeByMapData = new Dictionary<MapType, MapData>();
        mapDataList = new List<MapData>();

        // 혹시 데이터가 없을것을 방지해서 None 데이터 만들어놓기
        MapData dummyData = new MapData(MapType.None, Difficulty.None, "", 20);
        typeByMapData.Add(MapType.None, dummyData);
    }

    public void AddtoMapDictionary(MapType type, MapData data)
    {
        if (typeByMapData.ContainsKey(type))
        {
            Debug.Log($"해당 타입은 이미 딕셔너리에 있습니다 Type : {type}");
            return;
        }

        typeByMapData.Add(type, data);

        // 인스펙터 창에서 보기용
        mapDataList.Add(data);
    }

    public void SettingNowMapData(string mapString)
    {
        MapType type = Extension.StringToEnum<MapType>(mapString);

        if (typeByMapData.TryGetValue(type, out MapData data))
        {
            Debug.Log("MapDataManager에 저장 : " + mapString);

            SycnGameId(data);

            return;
        }
        
        // PopUp 띄우기 
        TextPopUp textPopUp = UIManager.Instance.GetPopUP<TextPopUp>();
        textPopUp.UpdateText("(로컬라이징전) 맵 데이터를 설정하는데 오류가 발생하였습니다. 다시 실행해주세요");
    }

    private void SycnGameId(MapData mapdata)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("[MapDataSync]게임 고유 ID Raise 이벤트");

            object[] contcnt = new object[]
            {
                mapdata.Maptype.ToString(),
                mapdata.Difficulty.ToString(),
                mapdata.Content,
                mapdata.Rate
            };

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            SendOptions sendOption = new SendOptions { Reliability = true };

            bool success =
                PhotonNetwork.RaiseEvent((byte)PunEventType.MapDataSync,
                    contcnt,
                    raiseEventOptions,
                    sendOption);

            Debug.Log($"[MapDataSync] RaiseEvent 보냄? {success}");
        }
        else 
        {
            Debug.Log("호스트가아닙니다");
        }
    }

    public void MapDataSetting(MapData data) 
    {
        nowMapData = data;
    }
}

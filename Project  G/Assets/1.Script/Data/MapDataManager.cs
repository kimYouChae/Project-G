using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapData 
{
    [SerializeField] private MapType maptype;
    [SerializeField] private Difficulty difficulty;
    [SerializeField] private string content;
    [SerializeField] private int rate;

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

    public int MapRate { get => nowMapData.Rate;  }

    protected override void Singleton_Awake()
    {
        SetUpDontDestroy();

        typeByMapData = new Dictionary<MapType, MapData>();
        mapDataList = new List<MapData>();

        // 혹시 데이터가 없을것을 방지해서 None 데이터 만들어놓기
        MapData dummyData = new MapData(MapType.None, Difficulty.None, "" , 20);
        typeByMapData.Add(MapType.None, dummyData);
    }

    public void AddtoMapDictionary(MapType type, MapData data) 
    {
        typeByMapData.Add(type, data);

        // 인스펙터 창에서 보기용
        mapDataList.Add(data);
    }

    public void SettingNowMapData(string mapString) 
    {
        Debug.Log("MapDataManager에 저장 : " + mapString);
        MapType type = Extension.StringToEnum<MapType>(mapString);

        if (typeByMapData.TryGetValue(type, out MapData data)) 
        {
            nowMapData = data;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChart : ICharHandler
{
    public void IParseAndStore(string jsonStr)
    {
        /*
        foreach (LitJson.JsonData row in jsonData)
        {
            // row는 각 원소(오브젝트)
            MapType mapType = Extension.StringToEnum<MapType>(row["MapType"].ToString());
            Difficulty diffi = Extension.StringToEnum<Difficulty>(row["Difficulty"].ToString());
            string contents = row["MapContents"].ToString();
            int rate = int.Parse(row["Rate"].ToString());

            // Debug.Log($"MapType={mapType}, Difficulty={difficulty}, Rate={rate}, Contents={contents}");

            MapData data = new MapData(mapType, diffi, contents, rate);
            MapDataManager.Instance.AddtoMapDictionary(mapType, data);
        }
        */
    }
}

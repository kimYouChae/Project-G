using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class CharacterChart : ICharHandler
{
    public void IParseAndStore(LitJson.JsonData jsonData)
    {
        foreach (LitJson.JsonData row in jsonData) 
        {
            string characterName = row["charaterName"].ToString();
            CharacterType cType = Extension.StringToEnum<CharacterType>(row["characterType"].ToString());
            string sToolTip = row["characterToolTip"].ToString();
            AchiveType aType = Extension.StringToEnum<AchiveType>(row["achiveType"].ToString());

            CharacterData characterData = new CharacterData(characterName, cType, sToolTip, aType);
            CharacterManager.Instance.AddtoCharacterContainer(characterData);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharHandler
{
    public void IParseAndStore(LitJson.JsonData data);
}

public interface ILobbyPanelInitionlize
{
    public void IInitPanel();
}

public interface ILocalizable 
{
    public void IUpdateLocalization(LanguageType type);
}

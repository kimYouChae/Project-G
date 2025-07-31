using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum NickCheckResultType
{
    NoPlayerInfo,
    NoNickname,
    HasNickname
}

public enum CharacterType
{
    Man, Woman, Girl, Warrior, 
    None
}

public enum LobbyPanelType
{
    UnTitled, Title, NickName, CharacterSelect, Lobby,
    RoomList, CreateRoom, WaitingRoom,
    None
}

public enum MapType 
{
    Forest,
    GiganticTree,
    Market,
    Island,
    Hell,
    IceVillage
}

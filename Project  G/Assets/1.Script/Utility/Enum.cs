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

public enum SceneType
{
    Lobby,
    Game_Forest,
    Game_GiganticTree,
    Game_Market,
    Game_Island,
    Game_Hell,
    Game_IceVillage
}

public enum DirType
{
    Left, Top, Right, Bottom
}
public enum QuadrantType    // 맵 상 사분면
{
    one, two, three, four
}
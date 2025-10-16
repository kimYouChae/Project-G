using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum NickCheckResultType
{
    NoPlayerInfo,
    NoNickname,
    HasNickname
}

public enum LobbyPanelType
{ 
    Title, NickName, Lobby,
    RoomList, CreateRoom, WaitingRoom,
    UnTitled,
    None
}

public enum MapType 
{
    Forest,
    GiganticTree,
    Market,
    Island,
    Hell,
    IceVillage,
    None
}

public enum Difficulty 
{
    Upper,
    Middle,
    Lower,
    None
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

public enum SpawnerType 
{
    BasicSpanwer,
    GuideMissileSpawner,
    LaserSpawner,
    FourDirSpanwer
}

public enum CharaterAniState
{
    none, front, back, left, right
}

public enum SpanwerAnimState 
{
    none, Idle, Attack
}

public enum SoundType 
{
    Master, SFX, BGM
}

public enum SFXType 
{
    UIClick
}

public enum BGMType 
{ 
    LobbyBGM,
    GameBGM
}

public enum LanguageType
{
    English,
    Korean,
    Japanese,
    Chinese
}

public enum AchiveType 
{
    Stage_Forest,
    Stage_GiganticTree,
    Stage_Island
}
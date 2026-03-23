using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region
public enum PunEventType
{
    UserDataSync = 1,
    GameIdSync = 2,
    MapDataSync = 3,
    BestScoreSync = 4,
    SFXSync = 5
}

#endregion

#region GameService
public enum DataType
{
    None,
    Achievement,
    Character,
    Localization_basic,
    Localization_Ingame,
    Localization_Player,
    Map,
    Spawner,
    Stage_Forest_SpawnerInfo
}

#endregion


public enum LobbyPanelType
{ 
    Title, Lobby,
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

#region 사운드 

public enum SoundType 
{
    Master, SFX, BGM
}

public enum SFXType 
{
    None = 0,

    // UI
    UIClick,
    UIBack,
    UIPopup,
    UIWarning,

    // InGame
    Countdown,
    GameStart,

    // Character
    CharacterFootstep,
    CharacterDeath,
    ShieldBlock,
    ScoreCoin,
    InvincibleSkill,

    // Spawner
    BulletSpawnerShot,  // 기본 스포너 : 발사
    FourBulletObjPut,   // 4방향 스포너가 생성될 때
    MissileSpawnerShot, // 미사일 스포너 : 발사
    MissileFly,         // 미사일 날라갈때
    MissileExplosion,   // 미사일 터질때
    LaserCharge,        // 레이저 차징
    LaserFire           // 레이저 슛
}

public enum BGMType 
{
    None = 0,

    Title,
    Lobby,

    GameOver,
    MapForest
}

#endregion

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
    Stage_Island,
    None
}

public enum CharacterType 
{
    BasicCharacter,
    ShieldCharacter,
    ScoreCharacter,
    InvincibleCharacter
}
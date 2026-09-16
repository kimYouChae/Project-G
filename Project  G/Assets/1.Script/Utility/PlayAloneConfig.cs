using UnityEngine;

/// <summary>
/// 혼자서 인게임 테스트를 하기 위한 설정 ( PLAY_ALONE )
///
/// 2인이 모여야만 게임이 시작되는 구조라, 기능 하나 확인하려 해도 클라 두 대가 필요하다.
/// 방을 1인방으로 만들면 대기방의 인원 체크( MaxPlayers == PlayerCount )가 그대로 통과되므로
/// 인게임 로직은 한 줄도 건드리지 않는다.
/// 전처리문은 이 파일에만 두고, 호출부는 #if 없이 IsSolo / RoomMaxUser 만 부른다.
/// ( 호출부마다 #if를 쓰면 한 곳을 빠뜨리기 쉬움 )
/// </summary>
public static class PlayAloneConfig
{
    // 릴리즈 빌드에서는 항상 false라 기존 동작과 완전히 동일하다
    public static readonly bool IsSolo =
#if PLAY_ALONE
        true;
#else
        false;
#endif

    /// <summary>
    /// 솔로 테스트일 때만 방 최대 인원을 1로 낮춘다
    /// 릴리즈 빌드에서는 넘겨받은 값을 그대로 돌려준다
    /// </summary>
    public static int RoomMaxUser(int normalMaxUser)
    {
        return IsSolo ? 1 : normalMaxUser;
    }
}

using UnityEngine;

/// <summary>
/// 개발 빌드( DEV_BUILD_TEST )에서만 강제로 열리는 맵 설정
///
/// 서버에 차트를 올리지 않고 미오픈 맵을 테스트하기 위한 용도.
/// 전처리문은 이 파일에만 두고, 호출부는 #if 없이 IsForceOpen()만 부른다.
/// ( 호출부마다 #if를 쓰면 한 곳을 빠뜨리기 쉬움 )
/// </summary>
public static class DevMapConfig
{
    // DEV_BUILD_TEST 빌드에서만 강제로 열리는 맵 목록
    // 릴리즈 빌드에서는 빈 배열이라 기존 동작과 완전히 동일하다
    public static readonly MapType[] ForceOpenMaps =
#if DEV_BUILD_TEST
        new MapType[] { MapType.Island };
#else
        new MapType[0];
#endif

    /// <summary>
    /// 개발 빌드에서 강제로 여는 맵인지
    /// 릴리즈 빌드에서는 항상 false ( 배열이 비어있음 )
    /// </summary>
    public static bool IsForceOpen(MapType type)
    {
        for (int i = 0; i < ForceOpenMaps.Length; i++)
        {
            if (ForceOpenMaps[i] == type)
                return true;
        }

        return false;
    }
}

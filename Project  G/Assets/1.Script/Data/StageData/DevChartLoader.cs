using UnityEngine;

/// <summary>
/// 개발 빌드( DEV_BUILD_TEST ) 전용 - 미오픈 맵의 스테이지 차트를 로컬에서 적재
///
/// - 읽는 곳 : Assets/Resources/DevChart/{MapType}.json
///   ( 정상 폴백 경로인 Resources/ChartFallback 은 건드리지 않는다.
///     거기에 넣으면 서버 연결이 실패한 실제 유저에게도 맵이 열린다 )
/// - 서버에서 정상 수신한 맵은 서버 값이 우선이다
/// - persistentDataPath 로컬 캐시에 저장하지 않는다
///   ( WebChartService.SaveLocal 은 ChartService() 안에서만 호출되고,
///     이 로더는 그 밖에 있으므로 구조적으로 캐시에 남을 수 없다 )
/// </summary>
public static class DevChartLoader
{
    private const string DEV_CHART_PATH = "DevChart";

    public static void LoadForceOpenMaps()
    {
        // 릴리즈 빌드에서는 길이가 0이라 루프가 한 번도 돌지 않는다
        MapType[] maps = DevMapConfig.ForceOpenMaps;

        for (int i = 0; i < maps.Length; i++)
        {
            MapType type = maps[i];

            // 서버에서 정상 수신했으면 서버 값 우선
            // ( 같은 데이터가 두 번 적재되는 것도 여기서 막힌다 )
            if (StageDataManager.Instance.StageDataMaxLength(type) > 0)
            {
                Debug.Log($"[DevChartLoader] {type} : 서버 차트가 이미 있어 건너뜁니다");
                continue;
            }

            TextAsset text = ResourceLoaderGeneric.LoadAsset<TextAsset>($"{DEV_CHART_PATH}/{type}");
            if (text == null)
            {
                Debug.LogError($"[DevChartLoader] Resources/{DEV_CHART_PATH}/{type}.json 이 없습니다");
                continue;
            }

            // 메모리에만 적재
            StageChart.ParseAndStoreMapData(text.text, type);

            Debug.Log($"[DevChartLoader] {type} : 개발용 차트 적재 완료 " +
                $"( 스테이지 {StageDataManager.Instance.StageDataMaxLength(type)}개 )");
        }
    }
}

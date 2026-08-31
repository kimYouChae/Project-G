using ExitGames.Client.Photon;
using Newtonsoft.Json;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class InGameRiseEvent : MonoBehaviour, IOnEventCallback
{
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    
    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        // Debug.Log($"[Photon] 수신된 eventCode = {eventCode}");

        // 유저 데이터 싱크 이벤트
        if (eventCode == (int)PunEventType.UserDataSync)
        {
            Debug.Log("[UserDataSync] 유저 데이터 싱크 이벤트 OnEvent실행");
            object[] data = (object[])photonEvent.CustomData;

            int actorNum = (int)data[0];
            long steamId = (long)data[1];
            string nick = (string)data[2];
            float bestScore = (float)data[3];

            InGamePlayer player = new InGamePlayer(actorNum, steamId, nick, bestScore);
            player.PrintPlayer();

            PunIngameManager.Instance.AddInGamePlayer(actorNum, player);
        }

        // 게임 ID 싱크 이벤트
        if (eventCode == (int)PunEventType.GameIdSync)
        {
            Debug.Log("[GameIdSync] 게임 ID 싱크 이벤트 OnEvent실행");
            object[] data = (object[])photonEvent.CustomData;

            string gameId = (string)data[0];
            MapType mapType = (MapType)data[1];

            PunGameoverManager.Instance.SynchedGameIDGuid = gameId;
            PunGameoverManager.Instance.SynchedGameMapType = mapType;
        }

        if (eventCode == (int)PunEventType.BestScoreSyncOffline) 
        {
            Debug.Log("[BestScoreSyncOffline] 오프라인 게임 싱크 이벤트 OnEvent실행");
            object[] data = (object[])photonEvent.CustomData;

            float score = (float)data[0];
            int stage = (int)data[1];
            MapType type = (MapType)((int)data[2]);
            float time = (float)data[3];

            InGameUI.Instance.gameOverUI.OnOffGameContextTexts(true);
            InGameUI.Instance.gameOverUI.GameOverTextOffline(score, time);

            // 게임 종료 이벤트 호출 
            AnalyticsManager.SendGameEnd(
                isComplted: true,
                score: score,
                stage: stage,
                mapType: type.ToString(),
                isbestScore: false,
                playTime: time);
        }

        //  게임 종료 후 GameData API 데이터 이벤트
        if (eventCode == (int)PunEventType.BestScoreSync) 
        {
            Debug.Log("[BestScoreSync] 게임 ID 싱크 이벤트 OnEvent실행");
            object[] data = (object[])photonEvent.CustomData;

            string json = (string)data[0];
            float currTime = (float)data[1];
            MapType maptype = (MapType)data[2];

            Debug.Log($"[BestScoreSync] {json}");
            BestScoreUpdateResponse bsResponse = JsonConvert.DeserializeObject<BestScoreUpdateResponse>(json);
        
            for(int i = 0; i < bsResponse.results.Count; i++) 
            {
                UserBestScoreResult result = bsResponse.results[i];

                // 응답클래스의 id와 로컬에 있는 id가 같으면 
                if(result.steamId == SteamUserData.Instance.GetSteamID()) 
                {
                    // (임시)출력
                    Debug.Log($"{i}번째 유저 정보 \n {result.steamId} / 점수 : {result.score} / 스테이지 {result.stage}");

                    // 1. gameOver UI에 텍스트 표시
                    InGameUI.Instance.gameOverUI.OnOffGameContextTexts(true);
                    InGameUI.Instance.gameOverUI.GameOverText(result.score, currTime, result.isUpdated);

                    // 점수가 업데이트 됐으면 
                    // 2. 로컬의 유저 정보 업데이트 
                    if(result.isUpdated) 
                    {
                        UserDataManager.Instance.UpdateUserData(maptype, result.score, result.stage);
                    }

                    // 3~4. 도전과제 로직 실행
                    StartCoroutine(AchieveLogic(result.steamId));

                    // 5. 게임 종료 이벤트 호출 
                    AnalyticsManager.SendGameEnd(
                        isComplted: true,
                        score: result.score,
                        stage: result.stage,
                        mapType: maptype.ToString(),
                        isbestScore: result.isUpdated,
                        playTime: currTime);
                }
            }
        }

        // 인게임내 SFX 사운드 이벤트
        if (eventCode == (int)PunEventType.SFXSync)
        {
            Debug.Log("[SFXSync] SFX 실행 OnEvent실행");
            object[] data = (object[])photonEvent.CustomData;

            int sfxType = (int)data[0];

            SFXManager.Instance.LocalPlaySFX((SFXType)sfxType);
        }

        // 마켓 주민 생성
        if (eventCode == (int)PunEventType.MerchantSpawn) 
        {
            Debug.Log("[MerchantSpawn] 주민생성 OnEvent실행");
            object[] data = (object[])photonEvent.CustomData;

            MerchantPatternType merchantType = (MerchantPatternType)data[0];
            DirType dirType = (DirType)data[1];
            Vector2 posi = new Vector2((float)data[2], (float)data[3]);
            float stopX = (float)data[4];

            // IMapPattern을 형변환
            MarketMapPattern market = MapPatternManager.Instance.CurrentMapPattern as MarketMapPattern;
            market.GenerateMerchant(merchantType, dirType, posi, stopX);
        }

        // Meeting 주민 생성
        if (eventCode == (int)PunEventType.MerchantMeetingSpawn) 
        {
            Debug.Log("[MerchantMeetingSpawn] Meeting 주민생성 OnEvent실행");
            object[] data = (object[])photonEvent.CustomData;

            MerchantPatternType merchantType = (MerchantPatternType)data[0];
            DirType dirType = (DirType)data[1];
            Vector2 posi = new Vector2((float)data[2], (float)data[3]);
            float stopX = (float)data[4];
            // meeting -> 대기시간 존재
            float waitTime = (float)data[5];

            // IMapPattern을 형변환
            MarketMapPattern market = MapPatternManager.Instance.CurrentMapPattern as MarketMapPattern;
            market.GenerateMerchant(merchantType, dirType, posi, stopX, waitTime);
        }



            // (게임종료시) 점수, 스테이지 싱크 이벤트
            /*
            if (eventCode == (int)PunEventType.ScoreStageSync) 
            {
                Debug.Log("[ScoreStageSync] 점수,스테이지 싱크 이벤트 OnEvent실행");
                object[] data = (object[])photonEvent.CustomData;

                float score = (float)data[0];
                int stage = (int)data[1];

                PunGameoverManager.Instance.SynchedScore = score;
                PunGameoverManager.Instance.SynchedStage = stage;

            }
            */

        }

    private IEnumerator AchieveLogic(long stemaId)
    {
        // 3. 도전과제 API 실행
        yield return StartCoroutine(
                   GameServices.Instance.UserProgressService.GetAchievementService(stemaId));

        // 4. 이후 스팀 도전과제 성공여부 체크
        // 도전과제 API 실행 후 model이 세팅됨
        SteamScript.Instance.SetAchievement(GameServices.Instance.AchieveProgressModel.GetBestScoreInfo());
    }   
}

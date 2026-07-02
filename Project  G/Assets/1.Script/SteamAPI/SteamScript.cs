using JetBrains.Annotations;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class SteamConnected 
{
    // 스팀API에서 호출한 정보를 바탕으로 
    // 로그인, 차트 API까지 끝냈는가 
    public static bool isSteamReady = false;
}

public class SteamScript : Singleton<SteamScript>
{
    protected Callback<GameOverlayActivated_t> m_GameOverlayActivated;
    private CSteamID steamStruct;   // 스팀 유저 정보 구조체 

    // 아이디별 유저 프로필 텍스쳐
    // 나를 제외한 다른 유저 프로필텍스쳐를 저장하는 용도
    private Dictionary<long, Texture2D> steamIdByProfileTexture;

    // 친구창에 있는 친구 정보 담아놓은 배열
    private List<CSteamID> friendCSteamIDs;
    // 친구의 스팀아이디별 CSteamID 담아두는 컨테이너
    private Dictionary<long, CSteamID> friendIdByStruct;

    // 스팀 유저 데이터 가져온 뒤 실행할 액션
    private Action afterGetSteamUserAction;

    // 도전과제 타입별 스팀 도전과제 API
    private Dictionary<AchieveType, string> achieveTypeBySteamAPI;

    public List<CSteamID> FriendCSteamIDs { get => friendCSteamIDs; }

    protected override void Singleton_Awake()
    {
        steamIdByProfileTexture = new Dictionary<long, Texture2D>();
        friendCSteamIDs = new List<CSteamID>();
        friendIdByStruct = new Dictionary<long, CSteamID>();

        InitAchieveByApiContainer();
    }

    private void OnEnable()
    {
        // 프로필 가져오는건 나중에
        afterGetSteamUserAction += SetProfileImage;
        afterGetSteamUserAction += LoginStart;

        // 초기화 
        // ( 이미 연결되어 있으면 연결안함 )
        if (SteamManager.Initialized)
        {
            // 테스트 - 오버레이 콜백 
            // m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);

            GetUserSteamID();

            // 이후 유저 로그인 로직 실행해야함. 
            afterGetSteamUserAction?.Invoke();
        }
    }

    private void Start()
    {
        // 나중에 콜백을 받을일이 생기면 주석 제거
        // StartCoroutine(SteamRunCallbacks());
    }

    #region (테스트용) 스팀 오버레이 콜백

    // 스팀 오버레이 켜고 끄기 
    private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
    {
        if (pCallback.m_bActive != 0)
        {
            Debug.Log("Steam Overlay has been activated");
        }
        else
        {
            Debug.Log("Steam Overlay has been closed");
        }
    }
    #endregion

    IEnumerator SteamRunCallbacks()
    {
        while (true) 
        {
            yield return new WaitForSeconds(0.2f);

            SteamAPI.RunCallbacks();
        }
    }

    // 내 스팀 정보 가져오기 
    private void GetUserSteamID() 
    {
        // 이미 1회 저장했으면 다시 불러올 필요가 없음
        // CSteamID는 구조체라 NULL 체크가 안됨 
        if (SteamUserData.Instance.SteamID != 0)
        {
            Debug.Log($"[{nameof(SteamScript)}] 이미 로컬에 저장된 스팀 유저 정보 구조체가 있습니다" );
            return;
        }

        steamStruct = SteamUser.GetSteamID();
        ulong id = steamStruct.m_SteamID;
        string nickName = SteamFriends.GetPersonaName();
        string country = SteamUtils.GetIPCountry();

        Debug.Log($"스팀에서 가져온 유저 정보 = {id} / {nickName} / {country} ");

        // SteamUserData에 추가 
        SteamUserData.Instance.SteamID = (long)id;
        SteamUserData.Instance.NickName = nickName;
        SteamUserData.Instance.Country = country;
    }

    private void SetProfileImage() 
    {
        // 저장된 정보를 바탕으로 프로필 텍스쳐 변환 
        Texture2D profileImage = GetProfileImage(steamStruct);

        // 프로필 이미지 저장
        UserDataManager.Instance.UpdateUserProfileImage(profileImage);
    }

    #region CSteamID 바탕으로 Texture 2D 리턴
    private Texture2D GetProfileImage(CSteamID steamID) 
    {
        //128*128px
        // 0 : 프로필이 없는경우
        // -1 : 아직  로드되지 않는경우
        int imageId = SteamFriends.GetLargeFriendAvatar(steamID);

        // 정상작동이 안되면
        if (imageId <= 0)
            return ResourceManager.Instance.GetDefaultSprite().texture;

        // 받아올 사진의 너비x높이
        uint width;
        uint height;

        // 이미지 id에 해당하는 사진의 너비,높이 받아오기
        if (!SteamUtils.GetImageSize(imageId, out width, out height))
            return null;

        // 버퍼 ( 이미지 데이터 담아둘 공간 )
        // 한 픽셀당 필요한 정보 : R / G / B / A (알파값) 총 4바이트
        // 그래서 총 픽셀 x 4 -> 공간 만들기 
        byte[] buffer = new byte[width * height * 4];

        // 이미지 바이트를 가져와서 buffer 배열에 넣는다 
        if (!SteamUtils.GetImageRGBA(imageId, buffer, buffer.Length))
            return null;

        // 현재 축이 다른것같아서 한번 뒤집기 
        byte[] flippedBuffer = FlipVertical(buffer, (int)width, (int)height);

        // 가로 세로가 이 크기이고, RGBA32 포맷을 쓰는 텍스처 객체 생성
        Texture2D tex = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false);
        // buffer 안의 원시 이미지 데이터를 텍스처 객체에 복사
        tex.LoadRawTextureData(flippedBuffer); 
        // 텍스쳐 반영 
        tex.Apply();

        return tex;
    }

    private byte[] FlipVertical(byte[] original, int width, int height)
    {
        int bytesPerPixel = 4;
        int rowSize = width * bytesPerPixel;
        byte[] flipped = new byte[original.Length];

        for (int y = 0; y < height; y++)
        {
            int srcIndex = y * rowSize;
            int dstIndex = (height - 1 - y) * rowSize;

            System.Buffer.BlockCopy(original, srcIndex, flipped, dstIndex, rowSize);
        }

        return flipped;
    }
    #endregion
    
    private void LoginStart() 
    {
        StartCoroutine(LoginUser());

#if DEV_BUILD_TEST
        // 초기화
        // SteamUserStats.ClearAchievement("ACH_CLEAR_FOREST");
        // 도전과제 클리어
        // SetSteamAchievement(AchieveType.Stage_Forest);
#endif

    }

    private IEnumerator LoginUser()
    {
        // 1. 스팀 로그인
        long steamID = SteamUserData.Instance.GetSteamID();
        string nick = SteamUserData.Instance.GetSteamNick();
        string cnr = SteamUserData.Instance.GetCountry();

        // 2. 유저데이터 스크립트에 steam 관련 정보 저장
        UserDataManager.Instance.InsertUserInfo(steamID, nick, cnr);

        // 3. 로그인 API
        yield return StartCoroutine(
            GameServices.Instance.AuthService.AuthService(steamID, nick, cnr));

        // 4. 차트 불러오기 
        yield return GameServices.Instance.ChartDataService.ChartService();

        // API 호출까지 끝 
        SteamConnected.isSteamReady = true;
        Debug.Log($"스팀 API 호출이 끝났습니다 상태 : {SteamConnected.isSteamReady}");
    }

    #region 친구관련

    public void GetSteamFriend() 
    {
        // 컨테이너 초기화
        friendCSteamIDs.Clear();
        friendIdByStruct.Clear();

        // 기본친구 타입
        EFriendFlags basicFriendType = EFriendFlags.k_EFriendFlagImmediate;
        // 기본 친구타입에 해당하는 친구의 count가져오기
        int length = SteamFriends.GetFriendCount(basicFriendType);

        for(int i =0 ; i < length; i++) 
        {
            // 인덱스 바탕으로 친구 CSteam 구조체 가져오기
            CSteamID friend = SteamFriends.GetFriendByIndex(i, basicFriendType);

            // 컨테이너에 저장 
            friendCSteamIDs.Add(friend);
            friendIdByStruct.Add((long)friend.m_SteamID, friend);
        }
    }


    // CSteam 기준으로 닉네임 리턴
    public string ReturnNickNameByCSteamID(CSteamID id) 
    {
        return SteamFriends.GetFriendPersonaName(id);
    }

    // CSteam 기준으로 텍스쳐 리턴
    public Texture2D ReturnProfileTexureByCSteamID(CSteamID id) 
    {
        // 딕셔너리에 일단 검사
        if(steamIdByProfileTexture.ContainsKey((long)id.m_SteamID)) 
        {
            // 있으면 저장된 프로필 리턴하기
            return steamIdByProfileTexture[(long)id.m_SteamID];
        }

        // 없으면 프로필 이미지 생성
        Texture2D profileImage = GetProfileImage(id);
        // 딕셔너리에 저장
        steamIdByProfileTexture.Add((long)id.m_SteamID, profileImage);

        // 리턴
        if (profileImage != null) 
            return profileImage;

        return default(Texture2D);
    }

    public void InviteFriendByIndex(int index) 
    {
        // 친구 steam 구조체 가져오기
        CSteamID friend = GetCSteamId(index);

        // 친구가 이 게임(project G)을 켰는지 확인
        FriendGameInfo_t info;
        bool friendIsInGame = SteamFriends.GetFriendGamePlayed(friend, out info)
            &&info.m_gameID.m_GameID == SteamUtils.GetAppID().m_AppId;
        if (!friendIsInGame)
        {
            // ##TODO : 팝업 띄우기
            Debug.Log($"{friend.m_SteamID}에 해당하는 친구는 아직 게임을 켜지 않았습니다");

            return;
        }

        if (friend.m_SteamID != 0)
        {
            // ##TODO : 팝업 띄우기
            Debug.Log("친구 초대를 합니다");

            // 친구의 CSteam, 보낼 문자열
            // 접속하면 콜백에서 해당 문자열을 사용할 수 있음 
            bool isSuccessInviteFriend 
                = SteamFriends.InviteUserToGame(friend , PhotonRoomInfo.RoomCode);
        }
        else
        {
            // ##TODO : 팝업 띄우기
            Debug.Log("친구의 CSteam이 NULL 입니다");

            // 친구 CSteam데이터를 출력해보기
            PrintFriendCSteamData();
        }
    }

    private void PrintFriendCSteamData() 
    {
        StringBuilder builder = new StringBuilder();
        for(int i = 0; i < friendCSteamIDs.Count; i++) 
        {
            CSteamID friend = friendCSteamIDs[i];
            builder.Append(i + "번째 : ");
            builder.Append(friend.m_SteamID);
            builder.Append("/ \n");
        }

        Debug.Log(builder.ToString());
    }

    #endregion

    #region 도전과제 관련
    private void InitAchieveByApiContainer()
    {
        achieveTypeBySteamAPI = new Dictionary<AchieveType, string>()
        {
            { AchieveType.Stage_Forest , "ACH_CLEAR_FOREST"},
            { AchieveType.Stage_GiganticTree , "ACH_CLEAR_GIGANTIC_TREE"},
            { AchieveType.Stage_Island , "ACH_CLEAR_ISLAND"}
        };
    }

    // 도전과제 성공 
    private void SetSteamAchievement(AchieveType type)
    {
        string apiName = SteamAchieveApiName(type);
        if (apiName == string.Empty)
        {
            Debug.Log($"[SteamAchievement] {type}에 해당하는 도전과제가 없습니다");
            return;
        }

        // API 호출
        bool flag = SteamUserStats.SetAchievement(apiName);
        // 서버에 등록, 오버레이 알람 표시
        SteamUserStats.StoreStats();

        Debug.Log($"[SteamAchievement] {apiName} : 도전과제 달성 성공 여부 {flag}");
    }

    // 도전과제 리스트 바탕으로 도전과제 성공여부 체크
    public void SetAchievement(List<AchieveProgressResponse> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            AchieveProgressResponse progress = list[i];
            AchieveType type = progress.AchieveType;

            string apiName = SteamAchieveApiName(type);
            if (apiName == string.Empty)
            {
                Debug.Log($"[SteamAchievement] {type}에 해당하는 도전과제가 없습니다");
                continue;
            }

            // 깬 도전과제 일 때 
            if (progress.isClear) 
            {
                // 도전과제가 스팀에서 깨져있는지 확인 
                bool achieved;
                SteamUserStats.GetAchievement(apiName, out achieved);

                // 안깨져있으면 -> 스팀 도전과제 업데이트 필요            
                if (!achieved)
                    SetSteamAchievement(type);
            }
        }
    }

    private string SteamAchieveApiName(AchieveType aType)
    {
        string apiName;
        if (!achieveTypeBySteamAPI.TryGetValue(aType, out apiName))
            return string.Empty;

        return apiName;

    }

    #endregion

    private CSteamID GetCSteamId(int index)
    {
        if(friendCSteamIDs.Count > index && index >= 0)
            return friendCSteamIDs[index];

        return new CSteamID{ };
    }
}

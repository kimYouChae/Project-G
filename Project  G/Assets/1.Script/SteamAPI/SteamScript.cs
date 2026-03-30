using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
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
    private Dictionary<ulong, Texture2D> steamIdByProfileTexture;

    // 친구창에 있는 친구 정보 담아놓은 배열
    private List<CSteamID> friendCSteamIDs;
    // 친구의 스팀아이디별 CSteamID 담아두는 컨테이너
    private Dictionary<ulong, CSteamID> friendIdByStruct;

    // 스팀 유저 데이터 가져온 뒤 실행할 액션
    private Action afterGetSteamUserAction;

    protected override void Singleton_Awake()
    {
        steamIdByProfileTexture = new Dictionary<ulong, Texture2D>();
        friendCSteamIDs = new List<CSteamID>();
        friendIdByStruct = new Dictionary<ulong, CSteamID>();   
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
        SteamUserData.Instance.SteamID = id;
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

    private void LoginStart() 
    {
        StartCoroutine(LoginUser());
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
        yield return GameServices.Instance.ChartLogic();

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
            friendIdByStruct.Add(friend.m_SteamID, friend);
        }
    }

    #endregion 
}

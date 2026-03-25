using Steamworks;
using UnityEngine;

public class SteamScript : MonoBehaviour
{
    protected Callback<GameOverlayActivated_t> m_GameOverlayActivated;
    private CSteamID steamStruct;

    private void OnEnable()
    {
        if (SteamManager.Initialized)
        {
            m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);

            // 테스트 - 강제로 친구창 띄우기 
            // SteamFriends.ActivateGameOverlay("Friends");

            GetUserSteamID();

            // 이후 유저 로그인 로직 실행해야함. 
        }
    }

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

    // 내 스팀 정보 가져오기 
    private void GetUserSteamID() 
    {
        steamStruct = SteamUser.GetSteamID();
        ulong id = steamStruct.m_SteamID;
        string nickName = SteamFriends.GetPersonaName();


        Debug.Log($"스팀에서 가져온 유저 정보 = {id} / {nickName}");


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
}

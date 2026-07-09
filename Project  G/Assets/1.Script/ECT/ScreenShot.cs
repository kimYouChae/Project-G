using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScreenShot : MonoBehaviour
{
    public Camera captureCamera;

    private void Start()
    {
        // SavePNG();

        // WebRequest 테스트 코드 
        // SteamId = 0 → LoginController.cs:28 → BadRequest "Invalid SteamID value"
        StartCoroutine(GameServices.Instance.AuthService.AuthService(0, "test", "KR"));
    }

    public void SavePNG()
    {

        int width = 1920;
        int height = 1080;

        RenderTexture rt = new RenderTexture(width, height, 24);
        captureCamera.targetTexture = rt;

        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);

        captureCamera.Render();

        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/Tilemap.png", bytes);

        captureCamera.targetTexture = null;
        RenderTexture.active = null;

        DestroyImmediate(rt);
        DestroyImmediate(tex);
    }
}

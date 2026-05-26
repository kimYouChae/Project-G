using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ResourceLoader
{
    // 게임 오브젝트
    public GameObject RoadPrefab(string path) => Resources.Load<GameObject>(path);

    // 스프라이트
    public Sprite RoadSprite(string path) => Resources.Load<Sprite>(path);  
    public Sprite[] RoadSpriteAll(string path) => Resources.LoadAll<Sprite>(path);

    // 오디오 
    public AudioMixer RoadMixer(string path) => Resources.Load<AudioMixer>(path);
    public AudioClip RoadClip(string path) => Resources.Load<AudioClip>(path);
    public AudioClip[] RoadClipAll(string path) => Resources.LoadAll<AudioClip>(path);

    // fallback 텍스트파일
    public TextAsset RoadFallBackLocalization(string path) => Resources.Load<TextAsset>(path);

    // fallback 차트 파일
    public TextAsset[] RoadeFallBackChartData(string path) => Resources.LoadAll<TextAsset>(path);
}

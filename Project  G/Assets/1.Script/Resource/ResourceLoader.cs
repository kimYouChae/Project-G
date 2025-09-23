using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ResourceLoader
{
    ResourcePath path;

    public ResourceLoader(ResourcePath path)
    {
        this.path = path;
    }

    // 게임 오브젝트
    public GameObject RoadPrefab(string path) => Resources.Load<GameObject>(path);

    // 스프라이트
    public Sprite RoadSprite(string path) => Resources.Load<Sprite>(path);  
    public Sprite[] RoadSpriteAll(string path) => Resources.LoadAll<Sprite>(path);

    // 오디오 
    public AudioMixer RoadMixer(string path) => Resources.Load<AudioMixer>(path);
    public AudioClip RoadClip(string path) => Resources.Load<AudioClip>(path);
    public AudioClip[] RoadClipAll(string path) => Resources.LoadAll<AudioClip>(path);
}

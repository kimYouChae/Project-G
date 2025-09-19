using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Resource Data", menuName = "Scriptable Object/Resource Data")]
public class ResourcePath : ScriptableObject
{
    [SerializeField] private string defaultSpritePath;
    public string DefaultSpritePath => defaultSpritePath;

    [SerializeField] private string mapSpritePath;
    public string MapSpritePath => mapSpritePath;

}

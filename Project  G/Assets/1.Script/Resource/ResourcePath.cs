using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.LookDev;

public enum ResourceGetMethod 
{ 
    Load,
    LoadAll,
    None
}

/// <summary>
/// - 에디터 단계 
///     : SerializedProperty를 통해 수정된(입력된) 값(= entities 리스트)이
///     "ResourcePath"의 entities 필드에 반영되고, .asset 파일에 저장됨
/// - 빌드 단계
///     : editor 스크립트는 빌드에 포함되지 X
///     : "ResourcePath"에서 사용할 때는 .asset에 저장된 정보를 바탕으로 사용 
/// </summary>
/// 

[CreateAssetMenu(fileName = "Resource Path", menuName = "Scriptable Object/Resource Data")]
public class ResourcePath : ScriptableObject
{    
    [Serializable]
    public class PathEntity
    {
        public ResourceGetMethod method;
        public string key;
        public string value;
    }

    [SerializeField]
    private List<PathEntity> entities = new List<PathEntity>();

    // key , 경로객체
    private Dictionary<string, PathEntity> keyByPathEntity;

    public string GetPathEntity(string key) 
    {
        // 초기화 된적이 없으면 
        // .asset에 저장된 값을 딕셔너리에 채워넣음
        if (keyByPathEntity == null) 
        {
            keyByPathEntity = new Dictionary<string, PathEntity>();
            foreach (var entity in entities) 
            {
                keyByPathEntity.Add(entity.key, entity);
            }
        }

        if (keyByPathEntity.ContainsKey(key))
        {
            return keyByPathEntity[key].value;
        }
        else 
        {
            Debug.Log($"[Resourcepath] : key {key}를 찾을 수 없습니다.");
            return null;
        }
    }
}

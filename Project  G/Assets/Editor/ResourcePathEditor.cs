using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.UI;

[CustomEditor(typeof(ResourcePath))] // =ResourcePath 타입의 인스펙터 화면을 그려주겟다
public class ResourcePathEditor : Editor
{
    // 커스텀에디터에서 사용하는 객체 클래스
    private SerializedProperty entities;

    // 추가하기 쪽에서 사용하는 값
    private ResourceGetMethod newMethod = ResourceGetMethod.None;
    private string newKey = "";
    private string newPath = "";

    // 현재 몇번째 요소를 수정중인지 ( -1이면 수정 상태 X )
    private int editingIndex = -1;
    private ResourceGetMethod editMethod = ResourceGetMethod.None;
    private string editKey = "";
    private string editPath = "";

    // 컬럼 너비 상수
    private const float ColMethodWidth = 60f;  
    private const float ColKeyWidth = 100f;
    private const float ColValueWidth = 150f;
    private const float ColButtonWidth = 50f;

    // 켜질 떄
    public void OnEnable()
    {
        // ResourcePath 오브젝트의, entities라는 이름의 필드를 가리키는 경로(path)
        entities = serializedObject.FindProperty("entities");
    }

    // 화면에 보이는 코드
    // 버튼같은 움직임 발생 시 매번 다시 그림
    public override void OnInspectorGUI()
    {
        // 원본 (ResourcePath) 으로 부터 읽기
        // ResourcePath 에셋 오브젝트 ( .asset )
        serializedObject.Update();

        // 상단 : 새 경로 입력
        EditorGUILayout.LabelField("새로운 경로 입력하기", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        newMethod = (ResourceGetMethod)EditorGUILayout.EnumPopup(newMethod);    // enum 팝업
        newKey = EditorGUILayout.TextField(newKey);                             // 입력할 수있는 텍스트필드
        newPath = EditorGUILayout.TextField(newPath);

        // 추가하기 버튼이 눌리면
        if (GUILayout.Button("추가하기", GUILayout.Width(70)))
        {
            // 리스트에 값 추가
            AddEntry(newMethod, newKey, newPath);
            newKey = ""; newPath = "";
            GUI.FocusControl(null);
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(20);

        // 하단 : path 목록
        EditorGUILayout.LabelField("Resource Path 목록", EditorStyles.boldLabel);

        // ===== 헤더 행
        #region 헤더 행
        EditorGUILayout.BeginHorizontal("box");
        GUILayout.Label("Method", EditorStyles.boldLabel, GUILayout.Width(ColMethodWidth));
        GUILayout.Label("Key", EditorStyles.boldLabel, GUILayout.Width(ColKeyWidth));
        GUILayout.Label("Value", EditorStyles.boldLabel, GUILayout.ExpandWidth(true));
        GUILayout.Label("", GUILayout.Width(ColButtonWidth * 2)); // 수정/삭제 버튼 자리만큼 빈 헤더
        EditorGUILayout.EndHorizontal();
        #endregion

        // ==== 데이터 행 
        for (int i = 0; i < entities.arraySize; i++) 
        {
            var ele = entities.GetArrayElementAtIndex(i);

            // ResourcePath의 PathEntity클래스 필드명과 동일해야함 
            var resourceGetMethod = ele.FindPropertyRelative("method");
            var key = ele.FindPropertyRelative("key");
            var value = ele.FindPropertyRelative("value");

            #region 짝수/홀수 행 배경색 다르게 
            GUI.backgroundColor = (i % 2 == 0) ? Color.white : new Color(0.8f, 0.8f, 0.8f);
            EditorGUILayout.BeginHorizontal("box");
            GUI.backgroundColor = Color.white;
            #endregion

            // i번째가 수정중이면
            if (editingIndex == i)
            {
                // 이 줄이 지금 수정 중이면 -> 입력 가능한 텍스트필드로 표시
                editMethod = (ResourceGetMethod)EditorGUILayout.EnumPopup(editMethod, GUILayout.Width(ColMethodWidth));
                editKey = EditorGUILayout.TextField(editKey, GUILayout.Width(ColKeyWidth));
                editPath = EditorGUILayout.TextField(editPath , GUILayout.ExpandWidth(true));

                // 완료 버튼이 눌리면
                if (GUILayout.Button("완료", GUILayout.Width(ColButtonWidth)))
                {
                    resourceGetMethod.enumValueIndex = (int)editMethod;
                    key.stringValue = editKey;
                    value.stringValue = editPath;

                    editingIndex = -1;
                }
                if (GUILayout.Button("취소", GUILayout.Width(ColButtonWidth)))
                {
                    editingIndex = -1;
                }
            }
            else 
            {
                // 읽기 전용 
                GUILayout.Label(((ResourceGetMethod)resourceGetMethod.enumValueIndex).ToString(), GUILayout.Width(ColMethodWidth));
                GUILayout.Label(key.stringValue, GUILayout.Width(ColKeyWidth));
                GUILayout.Label(value.stringValue, GUILayout.ExpandWidth(true));

                // 수정 버튼이 눌리면
                if (GUILayout.Button("수정", GUILayout.Width(ColButtonWidth)))
                {
                    // 현재 값으로 채우기 
                    editMethod = (ResourceGetMethod)resourceGetMethod.enumValueIndex; 
                    editKey = key.stringValue; 
                    editPath = value.stringValue;

                    editingIndex = i;
                }
                // 삭제 버튼이 눌리면
                if (GUILayout.Button("삭제", GUILayout.Width(ColButtonWidth)))
                {
                    entities.DeleteArrayElementAtIndex(i);  // i번째 요소 지우기
                    editingIndex = -1;
                }

            }
            EditorGUILayout.EndHorizontal();
        }

        // 원본 (ResourcePath 오브젝트) 에 반영 
        // ResourcePath의 .asset의 entities리스트 부분에 적용됨. 
        serializedObject.ApplyModifiedProperties();
        // 어떻게 반영되나 ? 
        // "serializedObject"는 본인이 가지고 있는 "SerializedProperty"들의 수정사항 등을 가지고있음
        // 즉 그 경로가 가지고 있는 실제 필드에 최종 반영함 
    }

    private void AddEntry(ResourceGetMethod method ,string key, string value) 
    {
        entities.arraySize++;   // 한칸 +
        var newEle = entities.GetArrayElementAtIndex(entities.arraySize-1);    // 마지막거 가져오기

        // ResourcePath의 PathEntity클래스 필드명과 동일해야함 
        newEle.FindPropertyRelative("method").enumValueIndex = (int)method;
        newEle.FindPropertyRelative("key").stringValue = key;
        newEle.FindPropertyRelative("value").stringValue = value;
    }
}

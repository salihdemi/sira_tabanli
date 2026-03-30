using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AnimationSpeedHalver : EditorWindow
{
    [SerializeField] private List<AnimationClip> clips = new List<AnimationClip>();
    private SerializedObject serializedObject;
    private SerializedProperty clipsProperty;

    [MenuItem("Tools/Animation Speed Halver")]
    public static void ShowWindow()
    {
        GetWindow<AnimationSpeedHalver>("Animation Speed Halver");
    }

    private void OnEnable()
    {
        serializedObject = new SerializedObject(this);
        clipsProperty = serializedObject.FindProperty("clips");
    }

    private void OnGUI()
    {
        GUILayout.Label("Animation Speed Halver", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        GUILayout.Label("Kare aralıklarını 2'ye katlar.");
        EditorGUILayout.Space();

        serializedObject.Update();
        EditorGUILayout.PropertyField(clipsProperty, new GUIContent("Clips"), true);
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space();

        if (GUILayout.Button("Apply"))
        {
            foreach (var clip in clips)
            {
                if (clip != null) HalveSpeed(clip);
            }
            AssetDatabase.SaveAssets();
            Debug.Log("Tüm clipler güncellendi.");
        }
    }

    private void HalveSpeed(AnimationClip clip)
    {
        EditorCurveBinding binding = new EditorCurveBinding
        {
            type = typeof(SpriteRenderer),
            path = "",
            propertyName = "m_Sprite"
        };

        ObjectReferenceKeyframe[] keyframes = AnimationUtility.GetObjectReferenceCurve(clip, binding);

        if (keyframes == null || keyframes.Length == 0)
        {
            Debug.LogWarning($"{clip.name}: Sprite keyframe bulunamadı.");
            return;
        }

        for (int i = 0; i < keyframes.Length; i++)
            keyframes[i].time *= 2f;

        AnimationUtility.SetObjectReferenceCurve(clip, binding, keyframes);
        Debug.Log($"{clip.name} güncellendi.");
    }
}

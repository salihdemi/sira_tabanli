using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AnimationLoopFixer : EditorWindow
{
    [SerializeField] private List<AnimationClip> clips = new List<AnimationClip>();
    private SerializedObject serializedObject;
    private SerializedProperty clipsProperty;

    [MenuItem("Tools/Animation Loop Fixer")]
    public static void ShowWindow()
    {
        GetWindow<AnimationLoopFixer>("Animation Loop Fixer");
    }

    private void OnEnable()
    {
        serializedObject = new SerializedObject(this);
        clipsProperty = serializedObject.FindProperty("clips");
    }

    private void OnGUI()
    {
        GUILayout.Label("Animation Loop Fixer", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        GUILayout.Label("Son kareyi kopyalayıp clip sonuna ekler.");
        EditorGUILayout.Space();

        serializedObject.Update();
        EditorGUILayout.PropertyField(clipsProperty, new GUIContent("Clips"), true);
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space();

        if (GUILayout.Button("Fix Clips"))
        {
            foreach (var clip in clips)
            {
                if (clip != null) FixClip(clip);
            }
            AssetDatabase.SaveAssets();
            Debug.Log("Tüm clipler düzeltildi.");
        }
    }

    private void FixClip(AnimationClip clip)
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

        ObjectReferenceKeyframe lastFrame = keyframes[keyframes.Length - 1];
        float interval = keyframes.Length > 1 ? keyframes[1].time - keyframes[0].time : 1f / clip.frameRate;

        ObjectReferenceKeyframe[] newKeyframes = new ObjectReferenceKeyframe[keyframes.Length + 1];
        keyframes.CopyTo(newKeyframes, 0);
        newKeyframes[keyframes.Length] = new ObjectReferenceKeyframe
        {
            time = lastFrame.time + interval,
            value = lastFrame.value
        };

        AnimationUtility.SetObjectReferenceCurve(clip, binding, newKeyframes);
        Debug.Log($"{clip.name} düzeltildi.");
    }
}

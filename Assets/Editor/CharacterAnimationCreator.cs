using UnityEngine;
using UnityEditor;
using System.IO;

public class CharacterAnimationCreator : EditorWindow
{
    private string characterName = "";
    private string outputPath = "Assets/Animations";
    private Texture2D idleSheet;
    private Texture2D walkSheet;
    private int frameRate = 8;

    private readonly string[] directions = { "Down", "Left", "Right", "Up" };

    [MenuItem("Tools/Character Animation Creator")]
    public static void ShowWindow()
    {
        GetWindow<CharacterAnimationCreator>("Character Animation Creator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Character Animation Creator", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        characterName = EditorGUILayout.TextField("Character Name", characterName);
        outputPath = EditorGUILayout.TextField("Output Path", outputPath);
        frameRate = EditorGUILayout.IntField("Frame Rate", frameRate);
        idleSheet = (Texture2D)EditorGUILayout.ObjectField("Idle Sheet", idleSheet, typeof(Texture2D), false);
        walkSheet = (Texture2D)EditorGUILayout.ObjectField("Walk Sheet", walkSheet, typeof(Texture2D), false);

        EditorGUILayout.Space();

        if (GUILayout.Button("Create Animations"))
        {
            if (!Validate()) return;
            CreateAnimations();
        }
    }

    private bool Validate()
    {
        if (string.IsNullOrEmpty(characterName)) { Debug.LogError("Character Name boş olamaz."); return false; }
        if (idleSheet == null) { Debug.LogError("Idle Sheet atanmamış."); return false; }
        if (walkSheet == null) { Debug.LogError("Walk Sheet atanmamış."); return false; }
        if (!Directory.Exists(outputPath)) Directory.CreateDirectory(outputPath);
        return true;
    }

    private void CreateAnimations()
    {
        CreateClips(idleSheet, "Idle");
        CreateClips(walkSheet, "Walk");
        AssetDatabase.Refresh();
        Debug.Log($"{characterName} animasyonları oluşturuldu: {outputPath}");
    }

    private void CreateClips(Texture2D sheet, string type)
    {
        string sheetPath = AssetDatabase.GetAssetPath(sheet);
        Sprite[] sprites = GetSprites(sheetPath);

        if (sprites.Length < 16)
        {
            Debug.LogError($"{sheetPath} içinde en az 16 sprite olmalı, bulunan: {sprites.Length}");
            return;
        }

        for (int dirIndex = 0; dirIndex < 4; dirIndex++)
        {
            AnimationClip clip = new AnimationClip();
            clip.frameRate = frameRate;

            EditorCurveBinding binding = new EditorCurveBinding
            {
                type = typeof(SpriteRenderer),
                path = "",
                propertyName = "m_Sprite"
            };

            ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[5];
            for (int frame = 0; frame < 4; frame++)
            {
                keyframes[frame] = new ObjectReferenceKeyframe
                {
                    time = frame / (float)frameRate,
                    value = sprites[dirIndex * 4 + frame]
                };
            }
            keyframes[4] = new ObjectReferenceKeyframe
            {
                time = 4 / (float)frameRate,
                value = sprites[dirIndex * 4]
            };

            AnimationUtility.SetObjectReferenceCurve(clip, binding, keyframes);

            // Loop ayarı
            AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clip);
            settings.loopTime = true;
            AnimationUtility.SetAnimationClipSettings(clip, settings);

            string clipName = $"{characterName}{type}{directions[dirIndex]}";
            string clipPath = $"{outputPath}/{clipName}.anim";
            AssetDatabase.CreateAsset(clip, clipPath);
        }
    }

    private Sprite[] GetSprites(string path)
    {
        Object[] assets = AssetDatabase.LoadAllAssetsAtPath(path);
        System.Collections.Generic.List<Sprite> sprites = new System.Collections.Generic.List<Sprite>();
        foreach (Object asset in assets)
            if (asset is Sprite) sprites.Add((Sprite)asset);

        // İsme göre sırala (sprite_0, sprite_1 gibi)
        sprites.Sort((a, b) =>
        {
            int numA = ExtractNumber(a.name);
            int numB = ExtractNumber(b.name);
            return numA.CompareTo(numB);
        });

        return sprites.ToArray();
    }

    private int ExtractNumber(string name)
    {
        string num = "";
        for (int i = name.Length - 1; i >= 0; i--)
        {
            if (char.IsDigit(name[i])) num = name[i] + num;
            else break;
        }
        return num.Length > 0 ? int.Parse(num) : 0;
    }
}

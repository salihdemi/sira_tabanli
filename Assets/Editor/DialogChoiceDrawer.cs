using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DialogAction))]
public class DialogActionDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var actionType = (DialogActionType)property.FindPropertyRelative("actionType").enumValueIndex;
        float h = EditorGUIUtility.singleLineHeight;
        float pad = EditorGUIUtility.standardVerticalSpacing;
        float total = h + pad;

        if (actionType == DialogActionType.GiveItem)
            total += h + pad;
        else if (actionType == DialogActionType.StartFight)
            total += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("enemies"), true) + pad;

        return total;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        float h = EditorGUIUtility.singleLineHeight;
        float pad = EditorGUIUtility.standardVerticalSpacing;
        Rect rect = new Rect(position.x, position.y, position.width, h);

        EditorGUI.PropertyField(rect, property.FindPropertyRelative("actionType"));
        rect.y += h + pad;

        var actionType = (DialogActionType)property.FindPropertyRelative("actionType").enumValueIndex;

        if (actionType == DialogActionType.GiveItem)
            EditorGUI.PropertyField(rect, property.FindPropertyRelative("rewardObject"));
        else if (actionType == DialogActionType.StartFight)
        {
            var enemiesProp = property.FindPropertyRelative("enemies");
            rect.height = EditorGUI.GetPropertyHeight(enemiesProp, true);
            EditorGUI.PropertyField(rect, enemiesProp, true);
        }

        EditorGUI.EndProperty();
    }
}

[CustomPropertyDrawer(typeof(DialogChoice))]
public class DialogChoiceDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float h = EditorGUIUtility.singleLineHeight;
        float pad = EditorGUIUtility.standardVerticalSpacing;
        float total = (h + pad) * 2; // choiceText, nextDialog
        total += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("actions"), true) + pad;
        return total;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        float h = EditorGUIUtility.singleLineHeight;
        float pad = EditorGUIUtility.standardVerticalSpacing;
        Rect rect = new Rect(position.x, position.y, position.width, h);

        EditorGUI.PropertyField(rect, property.FindPropertyRelative("choiceText"));
        rect.y += h + pad;
        EditorGUI.PropertyField(rect, property.FindPropertyRelative("nextDialog"));
        rect.y += h + pad;

        var actionsProp = property.FindPropertyRelative("actions");
        rect.height = EditorGUI.GetPropertyHeight(actionsProp, true);
        EditorGUI.PropertyField(rect, actionsProp, true);

        EditorGUI.EndProperty();
    }
}

[CustomPropertyDrawer(typeof(DialogLine))]
public class DialogLineDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float h = EditorGUIUtility.singleLineHeight;
        float pad = EditorGUIUtility.standardVerticalSpacing;
        float total = (h + pad) * 2; // speakerName, portrait
        total += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("text"), true) + pad;
        total += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("actions"), true) + pad;
        total += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("choices"), true) + pad;
        return total;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        float h = EditorGUIUtility.singleLineHeight;
        float pad = EditorGUIUtility.standardVerticalSpacing;
        Rect rect = new Rect(position.x, position.y, position.width, h);

        EditorGUI.PropertyField(rect, property.FindPropertyRelative("speakerName"));
        rect.y += h + pad;
        EditorGUI.PropertyField(rect, property.FindPropertyRelative("portrait"));
        rect.y += h + pad;

        var textProp = property.FindPropertyRelative("text");
        rect.height = EditorGUI.GetPropertyHeight(textProp, true);
        EditorGUI.PropertyField(rect, textProp);
        rect.y += rect.height + pad;

        var actionsProp = property.FindPropertyRelative("actions");
        rect.height = EditorGUI.GetPropertyHeight(actionsProp, true);
        EditorGUI.PropertyField(rect, actionsProp, true);
        rect.y += rect.height + pad;
        rect.height = h;

        var choicesProp = property.FindPropertyRelative("choices");
        rect.height = EditorGUI.GetPropertyHeight(choicesProp, true);
        EditorGUI.PropertyField(rect, choicesProp, true);

        EditorGUI.EndProperty();
    }
}

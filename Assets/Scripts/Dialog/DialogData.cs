using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DialogData", menuName = "Scriptable Objects/DialogData")]
public class DialogData : ScriptableObject
{
    public List<DialogLine> lines = new List<DialogLine>();
}

[System.Serializable]
public class DialogLine
{
    public string speakerName;
    public Sprite portrait;
    [TextArea(2, 5)] public string text;
    public List<DialogChoice> choices = new List<DialogChoice>();
}

[System.Serializable]
public class DialogChoice
{
    public string choiceText;
    public DialogData nextDialog;         // null ise dialog kapanır
    public DialogActionType actionType;
}

public enum DialogActionType
{
    None,
    CollectItem,
    StartFight,
    ProgressStory
}

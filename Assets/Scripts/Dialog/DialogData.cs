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
    public List<DialogAction> beforeActions = new List<DialogAction>();
    public List<DialogAction> afterActions = new List<DialogAction>();
    public List<DialogChoice> choices = new List<DialogChoice>();
}

[System.Serializable]
public class DialogChoice
{
    public string choiceText;
    public DialogData nextDialog;
    public List<DialogAction> actions = new List<DialogAction>();
}

[System.Serializable]
public class DialogAction
{
    public DialogActionType actionType;
    [HideInInspector] public ScriptableObject rewardObject;
    [HideInInspector] public List<CharacterData> enemies;
    [HideInInspector] public string animationTrigger;
    [HideInInspector] public AnimationParamType animationParamType;
    [HideInInspector] public bool animationBoolValue;
}

public enum AnimationParamType { Trigger, Bool }

public enum DialogActionType
{
    GiveItem,
    StartFight,
    ProgressStory,
    PlayAnimation
}

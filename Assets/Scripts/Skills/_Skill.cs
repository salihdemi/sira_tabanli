using TMPro;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public abstract class _Skill : ScriptableObject
{
    //private AnimationClip clip;
    //private AudioClip clip;
    public string _name;
    public TargetingSystem.TargetType targetType = TargetingSystem.TargetType.enemy;
    public abstract void Method(Profile user, Profile target);

}

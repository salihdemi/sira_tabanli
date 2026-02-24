using System.Collections;
using TMPro;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public abstract class Skill : ScriptableObject
{
    public string _name;

    public TargetType targetType = TargetType.enemy;//aþaðý inecek

    //private AnimationClip clip;
    //private AudioClip clip;


    public abstract IEnumerator Method(Profile user, Profile target);

    protected void PlayAudio()
    {

    }
    protected void PlayAnimation()
    {

    }



}

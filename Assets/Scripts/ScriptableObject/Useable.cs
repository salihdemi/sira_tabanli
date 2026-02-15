using System.Collections;
using TMPro;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public abstract class Useable : ScriptableObject
{
    public string _name;

    public TargetType targetType = TargetType.enemy;

    //private AnimationClip clip;
    //private AudioClip clip;

    [Header("Need")]
    public float healthCost;
    public float staminaCost;
    public float manaCost;

    public abstract void Method(Profile user, Profile target);

    protected void PlayAudio()
    {

    }
    protected void PlayAnimation()
    {

    }


    protected void Consume(Profile user)
    {
        user.AddToHealth(-healthCost, user);
        user.AddToMana(-manaCost);
        user.AddToStamina(-staminaCost);
    }


    public virtual int GetTime()
    {
        return 1;
    }

}
public enum TargetType
{
    enemy,
    ally,

    self,
    allEnemy,
    allAlly,
    all
}
